using System.Collections.Generic;
using MailBC.Domain.Entities;

namespace MailBC.Domain.Repositories
{
    public interface IContactRepository
    {
        IList<Contact> GetMailListContacts(MailList list);
        Contact GetById(long id);
        Contact GetByEMail(string email);
        bool SaveContact(Contact contact);
        bool DeleteContact(Contact contact);
    }
}