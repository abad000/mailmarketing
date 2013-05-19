using MailBC.Domain.Entities;

namespace MailBC.Domain.Repositories
{
    public interface IMailListRepository
    {
        MailList GetById(long id);
        MailList GetByName(string name);
        bool SaveMailList(MailList list);
        bool DeleteMailLit(MailList list);
    }
}