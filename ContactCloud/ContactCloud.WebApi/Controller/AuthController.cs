using ContactCloud.Common.Types;
using ContactCloud.Services.Dto;
using ContactCloud.Services.Repository;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactCloud.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Result<RegisterUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<RegisterUserDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<RegisterUserDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authRepository.RegisterAsync(request);
            return result.ToActionResult();
        }

        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(Result<SignInDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<SignInDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<SignInDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] SignInDto request)
        {
            var result = await _authRepository.AuthenticateAsync(request);
            return result.ToActionResult();
        }


    }
}
