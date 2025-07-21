using ContactCloud.Entity.Models;
using Microsoft.AspNetCore.Identity;

namespace ContactCloud.Entity.Model;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<ContactList> Contacts { get; set; }
}
