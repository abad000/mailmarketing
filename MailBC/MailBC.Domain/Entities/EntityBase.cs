using System;

namespace MailBC.Domain.Entities
{
    public abstract class EntityBase
    {
        public virtual long Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public bool IsTransiant
        {
            get { return Id == default(long); }
        }
    }
}