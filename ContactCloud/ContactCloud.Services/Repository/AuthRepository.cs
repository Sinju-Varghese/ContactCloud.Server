using ContactCloud.Common.Types;
using ContactCloud.Entity.Data;
using ContactCloud.Entity.Model;
using ContactCloud.Services.Dto;
using ContactCloud.Services.Repositories;
using ContactCloud.Services.Types;
using ContactCloud.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ContactCloud.Services.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    public readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly IValidator<RegisterRequestDto> _registerUserValidator;
    private readonly IValidator<SignInDto> _signInValidator;
    private readonly ContactCloudDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthRepository(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenRepository tokenRepository,
        IValidator<RegisterRequestDto> registerUserValidator,
        IValidator<SignInDto> signInValidator,
        ContactCloudDbContext context, 
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenRepository = tokenRepository;
        _registerUserValidator = registerUserValidator;
        _signInValidator = signInValidator;
        _context = context;
        _configuration = configuration;
    }

    public async Task<Result<UserResponseDto>> AuthenticateAsync(SignInDto request)
    {
        //1) Run FluentValidation on request
        var validationResult = await _signInValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var allMessages = validationResult.Errors.Select(e => e.ErrorMessage);
            return new Result<UserResponseDto>(ResultTypes.InvalidData)
            {
                Errors = allMessages.ToList()
            };
        }

        // 2) Attempt to find the user by email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new Result<UserResponseDto>(ResultTypes.CompletedWithErrors)
            {

                Message = "Login Failed",
                Errors = ["Invalid Email"]
            };
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        // 3) Check the password -> InvalidData (400) if incorrect
        if (!signInResult.Succeeded)

        {
            return new Result<UserResponseDto>(ResultTypes.CompletedWithErrors)
            {
                Message = "Login Failed",
                Errors = ["Invalid Password"]
            };

        }

        //4) Generate Token (e.g., JWT)
        var tokenResponse = await _tokenRepository.GenerateToken(user);
        if (string.IsNullOrEmpty(tokenResponse))
        {
            return new Result<UserResponseDto>(ResultTypes.CompletedWithErrors)
                .AddError("Token generation failed", ResultTypes.CompletedWithErrors);
        }

        // 6) Fetch role names and build a list of RoleResponseDto

        //var roleNames = await _userManager.GetRolesAsync(user);
        //var roles = new List<RoleResponseDto>();
        //foreach (var roleName in roleNames)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    if (role != null)
        //    {
        //        roles.Add(new RoleResponseDto
        //        {
        //            Id = role.Id,
        //            RoleName = role.Name
        //        });
        //    }
        //}

        var userInfo = new UserResponseDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            TokenResponse = tokenResponse,
            //Roles = roles
        };

        return new Result<UserResponseDto>(userInfo); ;
    }


    public async Task<Result<string>> RegisterAsync(RegisterRequestDto request)
    {
        // 1. Validate input
        var validationResult = await _registerUserValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new Result<string>(ResultTypes.InvalidData)
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }

        // 2. Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new Result<string>(ResultTypes.InvalidData)
                .AddError("Email already exists", ResultTypes.InvalidData);
        }

        // 3. Create the user
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = Guid.NewGuid().ToString().Replace("-", ""),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            //CreatedAt = DateTime.UtcNow,
        };

        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            var allErrors = identityResult.Errors.Select(e => e.Description);
            return new Result<string>(ResultTypes.InvalidData)
                .AddError(string.Join("; ", allErrors), ResultTypes.InvalidData);
        }

        // 4. Ensure "user" role exists
        //if (!await _roleManager.RoleExistsAsync(ApplicationRoles.User))
        //{
        //    await _roleManager.CreateAsync(new ApplicationRole()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Name = ApplicationRoles.User,
        //        NormalizedName = ApplicationRoles.User.ToUpperInvariant(),
        //        ConcurrencyStamp = Guid.NewGuid().ToString()
        //    });
        //}

        // 5. Assign user to "user" role
        //var roleResult = await _userManager.AddToRoleAsync(user, ApplicationRoles.User);
        //if (!roleResult.Succeeded) 
        //{
        //    var roleErrors = roleResult.Errors.Select(e => e.Description);
        //    return new Result<string>(ResultTypes.CompletedWithErrors)
        //        .AddError("User registered, but failed to assign role: " + string.Join("; ", roleErrors), ResultTypes.CompletedWithErrors);
        //}

        // 6. Return user ID
        return new Result<string>(user.Id);
    }

}
