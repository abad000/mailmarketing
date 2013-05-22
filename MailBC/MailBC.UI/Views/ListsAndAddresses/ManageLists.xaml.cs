using System;
using System.Windows.Controls;
using MailBC.UI.Infrastructure.Dependency;
using MailBC.UI.Infrastructure.ViewModels;

namespace MailBC.UI.Views.ListsAndAddresses
{
    public partial class ManageLists : UserControl
    {
        public ManageLists()
        {
            InitializeComponent();
        }

        #region Overrides

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = ApplicationContext.DependencyResolver.LocateDependency<ManageListsViewModel>();

            ((ManageListsViewModel)DataContext).Initialize();
        }

        #endregion
    }
}
