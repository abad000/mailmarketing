using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace MailBC.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainScreen : ModernWindow
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
