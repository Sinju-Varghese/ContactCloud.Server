using ContactCloud.Entity.Model;
using ContactCloud.Services.Dto;
using ContactCloud.Services.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactCloud.Services.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;


    public TokenRepository(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    public async Task<TokenResponseDto> GenerateToken(ApplicationUser userInfo)
    {
        //create claims details based on the user information
        var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, userInfo.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                };
        var roles = await _userManager.GetRolesAsync(userInfo);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signIn);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return new TokenResponseDto
        {

            Token = tokenString,
            Expires = token.ValidTo // UTC expiry time
        };
    }
}

