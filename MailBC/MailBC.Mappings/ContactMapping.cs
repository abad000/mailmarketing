using MailBC.Domain.Entities;

namespace MailBC.Mappings
{
    public class ContactMapping : EntityMappingBase<Contact>
    {
        public ContactMapping()
        {
            // TODO: Specify max length for properties of type string.

            Property(c => c.Email)
                .IsRequired();
            Property(c => c.FirstName)
                .IsOptional();
            Property(c => c.LastName)
                .IsOptional();
            Property(c => c.Age)
                .IsOptional();
            Property(c => c.Company)
                .IsOptional();
            Property(c => c.Address)
                .IsOptional();
            Property(c => c.ZipCode)
                .IsOptional();
            Property(c => c.City)
                .IsOptional();
            Property(c => c.State)
                .IsOptional();
            Property(c => c.Country)
                .IsOptional();
            Property(c => c.Telephone)
                .IsOptional();
            Property(c => c.Fax)
                .IsOptional();
            Property(c => c.WebSite)
                .IsOptional();
            Property(c => c.Job)
                .IsOptional();
            Property(c => c.Business)
                .IsOptional();

            HasMany(c => c.MailLists)
                .WithMany(ml => ml.Contacts);

            ToTable("Contact");
        }
    }
}