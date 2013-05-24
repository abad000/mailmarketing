using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using MailBC.Domain.Entities;
using MailBC.Domain.Repositories;
using MailBC.UI.Infrastructure.Base;

namespace MailBC.UI.Infrastructure.ViewModels
{
    public class ManageListsViewModel : ViewModelBase, IViewModel
    {
        private readonly IMailListRepository _mailListRepository;

        private ICollectionView _mailListsView;
        private MailList _selectedMailList;

        public ManageListsViewModel(IMailListRepository mailListRepository)
        {
            _mailListRepository = mailListRepository;
        }

        #region Binding Properties

        public ObservableCollection<MailList> MailLists { get; private set; }

        public MailList SelectedMailList
        {
            get { return _selectedMailList; }
            set
            {
                if (_selectedMailList == value) return;
                _selectedMailList = value;
                OnPropertyChanged("SelectedMailList");
            }
        }

        #endregion

        public void Initialize()
        {
            MailLists = new ObservableCollection<MailList>(_mailListRepository.GetAll());
            SelectedMailList = MailLists.Count > 0 ? MailLists[0] : null;

            _mailListsView = CollectionViewSource.GetDefaultView(MailLists);
            _mailListsView.CurrentChanged += OnCurrentChanged; 
        }

        private void OnCurrentChanged(object sender, EventArgs e)
        {
            MailList current = _mailListsView.CurrentItem as MailList;
        }
    }
}