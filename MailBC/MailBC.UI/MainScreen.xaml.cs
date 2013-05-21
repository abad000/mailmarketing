using System;
using System.Windows;

namespace MailBC.UI
{
    public partial class MainScreen
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
