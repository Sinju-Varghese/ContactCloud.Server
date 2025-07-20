using System.Security.Claims;
using ContactCloud.Common.Types;
using ContactCloud.Services.Dto.Contact;
using ContactCloud.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactCloud.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        public readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(Result<long?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<long?>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ContactCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _contactRepository.CreateContactAsync(dto, userId);
            return result.ToActionResult();
        }

        //delete
        [HttpDelete("{id:long}")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _contactRepository.DeleteContactAsync(id);
            return result.ToActionResult();
        }
    }
}
