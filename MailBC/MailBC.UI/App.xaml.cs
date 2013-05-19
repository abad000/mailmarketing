using System.Configuration;
using System.Windows;
using MailBC.DataStore;
using MailBC.DataStore.ContextStorages;

namespace MailBC.UI
{
    public partial class App : Application
    {
        /// <summary>
        /// Represents the Application's entry point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="startupEventArgs"></param>
        private void Run(object sender, StartupEventArgs startupEventArgs)
        {
            Boot.SplashScreen screen = new Boot.SplashScreen();
            screen.Show();

            // TODO: move initializations in a separated thread (asynchronous initializations) inside the spalshscreen

            InitializeDatabases();
            InitializeDependencies();

            screen.Close();
        }

        private static void InitializeDatabases()
        {
            string connectionStringName = ConfigurationManager.AppSettings.Get("connectionStringName");
            string[] mappingAssemblies = ConfigurationManager.AppSettings.Get("mappingAssemblies").Split(';');

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
