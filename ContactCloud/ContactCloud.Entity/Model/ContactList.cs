using System.ComponentModel.DataAnnotations.Schema;
using ContactCloud.Entity.Model;

namespace ContactCloud.Entity.Models;

[Table("ContactList")]
public class ContactList
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

}
