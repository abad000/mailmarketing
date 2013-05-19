using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Reflection;

namespace MailBC.DataStore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DbContextBuilder<T> : DbModelBuilder, IDbContextBuilder<T> where T : DbContext
    {
        private readonly DbProviderFactory _providerFactory;
        private readonly ConnectionStringSettings _connectionStringSettings;
        private readonly bool _recreateDatabaseIfExists;
        private readonly bool _lazyLoadingEnabled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="recreateDatabaseIfExists"></param>
        /// <param name="lazyLoadingEnabled"></param>
        public DbContextBuilder(string connectionStringName, string[] mappingAssemblies, bool recreateDatabaseIfExists, bool lazyLoadingEnabled)
        {
            Conventions.Remove<IncludeMetadataConvention>();

            _connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            _providerFactory = DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName);
            _recreateDatabaseIfExists = recreateDatabaseIfExists;
            _lazyLoadingEnabled = lazyLoadingEnabled;

            AddConfigurations(mappingAssemblies);
        }

        /// <summary>
        /// Adds mapping classes contained in provided assemblies and register entities as well.
        /// </summary>
        /// <param name="mappingAssemblies"></param>
        private void AddConfigurations(string[] mappingAssemblies)
        {
            if (mappingAssemblies == null || mappingAssemblies.Length == 0)
            {
                throw new ArgumentNullException("mappingAssemblies", "You must specify at least one mapping assembly");
            }

            bool hasMappingClass = false;

            foreach (string mappingAssembly in mappingAssemblies)
            {
                Assembly assembly = Assembly.LoadFrom(MakeLoadReadyAssemblyName(mappingAssembly));

                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsAbstract)
                    {
                        if (type.BaseType != null && (type.BaseType.IsGenericType && IsMappingClass(type.BaseType)))
                        {
                            hasMappingClass = true;

                            // http://areaofinterest.wordpress.com/2010/12/08/dynamically-load-entity-configurations-in-ef-codefirst-ctp5/
                            dynamic configInstance = Activator.CreateInstance(type);
                            Configurations.Add(configInstance);
                        }
                    }
                }
            }

            if (!hasMappingClass)
            {
                throw new ArgumentException("No mapping class found!");
            }
        }

        #region Implementation of IDbContextBuilder<T>

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T BuildDbContext()
        {
            DbConnection connection = _providerFactory.CreateConnection();
            connection.ConnectionString = _connectionStringSettings.ConnectionString;

            DbModel dbModel = Build(connection);

            ObjectContext context = dbModel.Compile().CreateObjectContext<ObjectContext>(connection);
            context.ContextOptions.LazyLoadingEnabled = _lazyLoadingEnabled;

            if (!context.DatabaseExists())
            {
                context.CreateDatabase();
            }
            else if (_recreateDatabaseIfExists)
            {
                context.DeleteDatabase();
                context.CreateDatabase();
            }

            return (T)new DbContext(context, true);
        }

        #endregion

        /// <summary>
        /// Determines whether a type is a subclass of entity mapping type
        /// </summary>
        /// <param name="mappingType"></param>
        /// <returns>
        /// <c>true</c> if it is mapping class; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMappingClass(Type mappingType)
        {
            Type baseType = typeof(EntityTypeConfiguration<>);
            if (mappingType.GetGenericTypeDefinition() == baseType)
            {
                return true;
            }
            if ((mappingType.BaseType != null) &&
                !mappingType.BaseType.IsAbstract &&
                mappingType.BaseType.IsGenericType)
            {
                return IsMappingClass(mappingType.BaseType);
            }
            return false;
        }

        /// <summary>
        /// Ensure the assembly name is qualified
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private static string MakeLoadReadyAssemblyName(string assemblyName)
        {
            return assemblyName.IndexOf(".dll", StringComparison.Ordinal) == -1
                ? assemblyName.Trim() + ".dll"
                : assemblyName.Trim();
        }
    }
}