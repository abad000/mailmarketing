using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.Windows.Threading;
using MailBC.Domain.Entities;
using MailBC.Domain.Repositories;
using MailBC.UI.Infrastructure.Base;
using System.Threading.Tasks;

namespace MailBC.UI.Infrastructure.ViewModels
{
    public class ManageListsViewModel : ViewModelBase, IViewModel
    {
        private readonly IMailListRepository _mailListRepository;
        private ObservableCollection<MailList> _mailLists;

        #region Binding Properties

        public ObservableCollection<MailList> MailLists
        {
            get { return _mailLists; }
        }

        public MailList SelectedMailList { get; set; }

        #endregion

        public ManageListsViewModel(IMailListRepository mailListRepository)
        {
            _mailListRepository = mailListRepository;

            _mailLists = new ObservableCollection<MailList>(_mailListRepository.GetAll());
            SelectedMailList = _mailLists.Count > 0 ? _mailLists[0] : null;
        }

        public void Initialize()
        {
            // TODO: async initializations here...
        }
    }
}