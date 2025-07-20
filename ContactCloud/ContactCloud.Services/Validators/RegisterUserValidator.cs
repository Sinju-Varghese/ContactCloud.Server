using ContactCloud.Services.Dto;
using FluentValidation;

namespace ContactCloud.Services.Validators;


public class RegisterUserValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterUserValidator()
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

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

    }
}
