using ContactCloud.Common.Types;
using ContactCloud.Entity.Data;
using ContactCloud.Entity.Model;
using ContactCloud.Entity.Models;
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
    public readonly IValidator<ContactUpdateDto> _contactUpdateValidator;

    public ContactRepository(
        UserManager<ApplicationUser> userManager,
        ContactCloudDbContext context,
        IValidator<ContactCreateDto> contactCreateValidator,
        IValidator<ContactUpdateDto> contactUpdateValidator)
    {
        _userManager = userManager;
        _context = context;
        _contactCreateValidator = contactCreateValidator;
        _contactUpdateValidator = contactUpdateValidator;
    }

    public async Task<Result<int?>> CreateContactAsync(ContactCreateDto dto )
    {
        // 1. Validate input using FluentValidation
        var validationResult = await _contactCreateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var result = new Result<int?>(ResultTypes.InvalidData);
            foreach (var error in validationResult.Errors)
            {
                result.AddError(error.ErrorMessage, ResultTypes.InvalidData);
            }
            return result;
        }

        //2.Duplicate check
       var existingContact = await _context.Contacts
           .FirstOrDefaultAsync(c => c.Email == dto.Email || c.PhoneNumber == dto.PhoneNumber);

        if (existingContact != null)
        {
            return new Result<int?>().AddError("A contact with the same email or phone number already exists.", ResultTypes.InvalidData);
        }

        // 3. Create new contact
        var contacts = new ContactList
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            UserId = dto.UserId
        };

        _context.Contacts.Add(contacts);
        await _context.SaveChangesAsync();

        return new Result<int?>(contacts.Id)
        {
            Message = "Contact created successfully"

        };
    }

    public async Task<Result<bool>> UpdateContactAsync(int id, ContactUpdateDto dto)
    {
        // Validate the DTO
        var validationResult = await _contactUpdateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var result = new Result<bool>(ResultTypes.InvalidData);
            foreach (var error in validationResult.Errors)
            {
                result.AddError(error.ErrorMessage, ResultTypes.InvalidData);
            }
            return result;
        }

        // Find the contact
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            return new Result<bool>(ResultTypes.NotFound)
                .AddError("Contact not found.", ResultTypes.NotFound);
        }

        // Update fields
        contact.FirstName = dto.FirstName;
        contact.LastName = dto.LastName;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;
        contact.Address = dto.Address;

        // Save changes
        await _context.SaveChangesAsync();

        return new Result<bool>(true) { Message = "Contact updated successfully." };
    }

    public async Task<Result<bool>> DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return new Result<bool>(ResultTypes.NotFound)
                .AddError("Contact not found.", ResultTypes.NotFound);

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return new Result<bool>(true) 
        { 
            Message = "Contact deleted successfully." 
        };
    }
    /// <summary>
    /// checked this code
    /// </summary>
    /// <returns></returns>
    public async Task<Result<ContactViewDto[]>> GetAllContactsAsync()
    {
        var contacts = await _context.Contacts
            .OrderBy(c=>c.FirstName)
            .Select(c => new ContactViewDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                UserId = c.UserId,
            })
            .ToArrayAsync();

        return new Result<ContactViewDto[]>(contacts)
        {
            Message = "Contacts retrieved successfully."
        };
    }

    public async Task<Result<ContactViewDto?>> GetByIdAsync(int id)
    {
        var contact = await _context.Contacts
            .Where(c => c.Id == id)
            .Select(c => new ContactViewDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                UserId = c.UserId,
            })
            .FirstOrDefaultAsync();

        if (contact == null)
            return new Result<ContactViewDto?>(ResultTypes.NotFound)
                .AddError("Contact not found.", ResultTypes.NotFound);

        return new Result<ContactViewDto?>(contact);
    }



}
