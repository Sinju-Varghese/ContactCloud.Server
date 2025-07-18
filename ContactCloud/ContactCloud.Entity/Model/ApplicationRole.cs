using Microsoft.AspNetCore.Identity;
namespace ContactCloud.Entity.Model;

public class ApplicationRole : IdentityRole<string>
{
    public bool IsEditable { get; set; } = false;
    public string? Description { get; set; }
}
