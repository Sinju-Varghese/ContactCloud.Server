using ContactCloud.Common.Types;
using ContactCloud.Entity.Data;
using ContactCloud.Entity.Model;
using ContactCloud.Services.Dto.Contact;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactCloud.Services.Repository;

public class ContactRepository : IContactRepository
{
    public readonly UserManager<ApplicationUser> _userManager;
    public readonly ContactCloudDbContext _context;
    public readonly IValidator<ContactCreateDto> _contactCreateValidator;

    public ContactRepository(
        UserManager<ApplicationUser> userManager,
        ContactCloudDbContext context,
        IValidator<ContactCreateDto> contactCreateValidator)
    {
        _userManager = userManager;
        _context = context;
        _contactCreateValidator = contactCreateValidator;
    }

    public async Task<Result<long?>> CreateContactAsync(ContactCreateDto model, string userId )
    {
        // 1. Validate input using FluentValidation
        var validationResult = await _contactCreateValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            var result = new Result<long?>();
            foreach (var error in validationResult.Errors)
            {
                result.AddError(error.ErrorMessage, ResultTypes.InvalidData);
            }
            return result;
        }

        // 2. Duplicate check
        var existingContact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Email == model.Email || c.PhoneNumber == model.PhoneNumber);

        if (existingContact != null)
        {
            return new Result<long?>().AddError("A contact with the same email or phone number already exists.", ResultTypes.InvalidData);
        }

        // 3. Create new contact
        var contact = new Contact
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            CreatedAt = DateTime.UtcNow
        };

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        return new Result<long?>(contact.Id)
        {
            Message = "Contact created successfully"

        };
    }

    public async Task<Result<bool>> DeleteContactAsync(long contactId)
    {
        var contact = await _context.Contacts.FindAsync(contactId);
        if (contact == null)
            return new Result<bool>().AddError("Contact not found.", ResultTypes.NotFound);

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return new Result<bool>(true) 
        { 
            Message = "Contact deleted successfully." 
        };
    }

}
