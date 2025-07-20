using Microsoft.AspNetCore.Identity;

namespace ContactCloud.Entity.Model;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }

}
