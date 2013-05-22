using System.Collections.ObjectModel;
using MailBC.Domain.Entities;
using MailBC.Domain.Repositories;
using MailBC.UI.Infrastructure.Base;

namespace MailBC.UI.Infrastructure.ViewModels
{
    public class ManageListsViewModel : ViewModelBase, IViewModel
    {
        private readonly IMailListRepository _mailListRepository;
        private ObservableCollection<MailList> _mailLists;
        private MailList _selectedMailList;

        #region Binding Properties

        public ObservableCollection<MailList> MailLists
        {
            get { return _mailLists; }
        }

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

        public ManageListsViewModel(IMailListRepository mailListRepository)
        {
            _mailListRepository = mailListRepository;
            _mailLists = new ObservableCollection<MailList>(_mailListRepository.GetAll());
            _selectedMailList = _mailLists.Count > 0 ? _mailLists[0] : null;
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}