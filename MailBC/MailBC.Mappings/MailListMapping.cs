using MailBC.Domain.Entities;

namespace MailBC.Mappings
{
    public class MailListMapping : EntityMappingBase<MailList>
    {
        public MailListMapping()
        {
            // TODO: Specify max length for properties of type string.

            Property(ml => ml.Name)
                .IsRequired();

            HasMany(ml => ml.Contacts)
                .WithMany(c => c.MailLists);

            ToTable("MailList");
        }
    }
}