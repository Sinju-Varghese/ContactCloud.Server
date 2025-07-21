using ContactCloud.Entity.Model;
using ContactCloud.Entity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactCloud.Entity.Data
{
    public class ContactCloudDbContext : IdentityDbContext<ApplicationUser>
    {
        public ContactCloudDbContext(DbContextOptions<ContactCloudDbContext> options)
            : base(options) { }

        
         public DbSet<ContactList> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactCloudDbContext).Assembly);
        }
    }
}
