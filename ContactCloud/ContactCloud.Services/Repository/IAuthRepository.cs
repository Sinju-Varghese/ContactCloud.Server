using ContactCloud.Common.Types;
using ContactCloud.Services.Dto;

namespace ContactCloud.Services.Repository;

public interface IAuthRepository
{
    Task<Result<string>> RegisterAsync(RegisterRequestDto request);

    Task<Result<UserResponseDto>> AuthenticateAsync(SignInDto request);
}
