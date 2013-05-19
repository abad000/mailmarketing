using System.Data.Entity.ModelConfiguration;
using MailBC.Domain.Entities;

namespace MailBC.Mappings
{
    public abstract class EntityMappingBase<T>
        : EntityTypeConfiguration<T> where T : EntityBase
    {
        protected EntityMappingBase()
        {
            HasKey(t => t.Id);

            Property(t => t.CreationDate).IsRequired();
            Property(t => t.ModificationDate).IsOptional();
        }
    }
}