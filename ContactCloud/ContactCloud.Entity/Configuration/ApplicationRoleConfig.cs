using ContactCloud.Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCloud.Entity.Configuration;

public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.Property(a => a.Description)
            .HasMaxLength(250);

        builder.Property(a => a.IsEditable)
            .HasDefaultValue(false);
    }
}
