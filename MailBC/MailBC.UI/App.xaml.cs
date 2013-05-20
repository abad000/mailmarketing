using System.Windows;
using MailBC.UI.Infrastructure.BootStrapper;

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
            AppBootStrapper.RunInitializations();

            screen.Close();
        }
    }
}
