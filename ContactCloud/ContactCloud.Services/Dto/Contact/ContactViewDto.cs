﻿namespace ContactCloud.Services.Dto.Contact;

public class ContactViewDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public string UserId { get; set; }
}
