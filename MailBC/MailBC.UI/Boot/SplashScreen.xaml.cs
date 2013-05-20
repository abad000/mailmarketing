using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MailBC.UI.Infrastructure.BootStrapper;

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

                    MainWindow window = new MainWindow();
                    window.Show();

                    return null;

                }), null);
        }
    }
}
