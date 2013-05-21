using System.Configuration;
using MailBC.DataStore;
using MailBC.DataStore.ContextStorages;
using MailBC.UI.Infrastructure.Dependency;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace MailBC.UI.Infrastructure.BootStrapper
{
    public static class AppBootStrapper
    {
        private static readonly string ConnectionStringName = ConfigurationManager.AppSettings.Get("connectionStringName");
        private static readonly string[] MappingAssemblies = ConfigurationManager.AppSettings.Get("mappingAssemblies").Split(';');

        private static UnityContainer _container;
        private static UnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                    _container = new UnityContainer();
                    section.Configure(_container);
                }
                return _container;
            }
        }

        public static void RunInitializations()
        {
            InitializeDatabases();
            InitializeDependencies();
        }

        private static void InitializeDatabases()
        {
            DbContextInitializer.Instance().InitializeDbContextOnce(() =>
                {
                    DbContextManager.InitStorage(new SimpleDbContextStorage());
                    DbContextManager.Init(ConnectionStringName, MappingAssemblies, false, true);
                });
        }

        private static void InitializeDependencies()
        {
            ApplicationContext.DependencyResolver = new UnityDependencyResolver(Container);
        }
    }
}