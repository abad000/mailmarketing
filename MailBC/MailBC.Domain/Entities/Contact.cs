using System.Collections.Generic;

namespace MailBC.Domain.Entities
{
    public class Contact : EntityBase
    {
        public Contact()
        {
            MailLists = new List<MailList>();
        }

        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual byte Age { get; set; }
        public virtual string Company { get; set; }
        public virtual string Address { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string WebSite { get; set; }
        public virtual string Job { get; set; }
        public virtual string Business { get; set; }
        public virtual IList<MailList> MailLists { get; set; }
    }
}