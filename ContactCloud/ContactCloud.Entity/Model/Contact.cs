namespace ContactCloud.Entity.Model;

public class Contact : ModelBase
{
    public int Id { get; set; } // Primary key
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
