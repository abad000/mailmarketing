using System.Linq;
using MailBC.Domain.Entities;

namespace MailBC.Domain.Repositories
{
    public interface IMailListRepository
    {
        IQueryable<MailList> GetAll();
        MailList GetById(long id);
        MailList GetByName(string name);
        bool SaveMailList(MailList list);
        bool DeleteMailLit(MailList list);
    }
}