using System.Collections.Generic;
using System.Linq;
using MailBC.DataStore;
using MailBC.Domain.Entities;
using MailBC.Domain.Repositories;

namespace MailBC.UI.Infrastructure.Repositories
{
    public class MailListRepository : Repository, IMailListRepository
    {
        public MailListRepository() : base("MailBCDb") { }

        #region Implementation of IMailListRepository

        public IQueryable<MailList> GetAll()
        {
            return Query<MailList>();
        }

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