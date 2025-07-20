using ContactCloud.Services.Dto.Contact;
using FluentValidation;

namespace ContactCloud.Services.Validators
{
    public class ContactCreateDtoValidator : AbstractValidator<ContactCreateDto>
    {
        public ContactCreateDtoValidator() 
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("First name is required.")
                 .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                 .MaximumLength(50).WithMessage("First name must be at most 50 characters long.")
                 .Matches("^[A-Za-z]+$").WithMessage("First name must contain only alphabets.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(1).WithMessage("Last name must be at least 1 character long.")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters long.")
                .Matches("^[A-Za-z]+$").WithMessage("Last name must contain only alphabets.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^[0-9]{10,15}$").WithMessage("Phone number must be between 10 and 15 digits.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.Address)
                .MaximumLength(200)
                .WithMessage("Address cannot exceed 200 characters.");
                            
        }
    }
}
