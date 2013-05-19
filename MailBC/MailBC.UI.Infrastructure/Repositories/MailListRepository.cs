using MailBC.DataStore;
using MailBC.Domain.Entities;
using MailBC.Domain.Repositories;

namespace MailBC.UI.Infrastructure.Repositories
{
    public class MailListRepository : Repository, IMailListRepository
    {
        #region Implementation of IMailListRepository

        public MailList GetById(long id)
        {
            throw new System.NotImplementedException();
        }

        public MailList GetByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveMailList(MailList list)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteMailLit(MailList list)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}