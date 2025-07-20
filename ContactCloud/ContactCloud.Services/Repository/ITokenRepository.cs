using ContactCloud.Entity.Model;
using ContactCloud.Services.Dto;

namespace ContactCloud.Services.Repository;

public interface ITokenRepository
{
    public Task<TokenResponseDto> GenerateToken(ApplicationUser userInfo);
}
