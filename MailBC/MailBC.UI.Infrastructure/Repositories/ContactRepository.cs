using System.Collections.Generic;
using MailBC.DataStore;
using MailBC.Domain.Entities;
using MailBC.Domain.Repositories;

namespace MailBC.UI.Infrastructure.Repositories
{
    public class ContactRepository : Repository, IContactRepository
    {
        #region Implementation of IContactRepository

        public IList<Contact> GetMailListContacts(MailList list)
        {
            throw new System.NotImplementedException();
        }

        public Contact GetById(long id)
        {
            throw new System.NotImplementedException();
        }

        public Contact GetByEMail(string email)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveContact(Contact contact)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteContact(Contact contact)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}