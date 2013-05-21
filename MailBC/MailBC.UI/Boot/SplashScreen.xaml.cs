using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MailBC.UI.Infrastructure.BootStrapper;
using MailBC.UI.Infrastructure.Dependency;

namespace MailBC.UI.Boot
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashLoaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    LaunchMainScreen();
                }), null);
        }

        private void LaunchMainScreen()
        {
            AppBootStrapper.RunInitializations();
            Thread.Sleep(5000);

            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate
                {
                    this.Hide();

                    MainScreen window = ApplicationContext.DependencyResolver.LocateDependency<MainScreen>();
                    window.Show();

                    return null;

                }), null);
        }
    }
}
