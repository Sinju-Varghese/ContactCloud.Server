using ContactCloud.Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCloud.Entity.Configuration;

public class ContactConfig : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(255);
    }
}
