using System.Configuration;
using MailBC.DataStore;
using MailBC.DataStore.ContextStorages;

namespace MailBC.UI.Infrastructure.BootStrapper
{
    public class AppBootStrapper
    {
        private static readonly string connectionStringName = ConfigurationManager.AppSettings.Get("connectionStringName");
        private static readonly string[] mappingAssemblies = ConfigurationManager.AppSettings.Get("mappingAssemblies").Split(';');

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
                    DbContextManager.Init(connectionStringName, mappingAssemblies, false, true);
                });
        }

        private static void InitializeDependencies()
        {
            // TODO: Initialize dependency container here...
        }
    }
}