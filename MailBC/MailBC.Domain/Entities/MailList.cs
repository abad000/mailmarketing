using System.Collections.Generic;

namespace MailBC.Domain.Entities
{
    public class MailList : EntityBase
    {
        public MailList()
        {
            Contacts = new List<Contact>();
        }

        public virtual string Name { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
    }
}