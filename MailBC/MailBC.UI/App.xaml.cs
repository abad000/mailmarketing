using System.Windows;
using MailBC.UI.Boot;

namespace MailBC.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Run(object sender, StartupEventArgs e)
        {
            Boot.SplashScreen splash = new Boot.SplashScreen();
            splash.Show();
        }
    }
}
