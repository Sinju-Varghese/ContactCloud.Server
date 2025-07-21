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
            var result = await _contactRepository.CreateContactAsync(dto);
            return result.ToActionResult();
        }

        //delete
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _contactRepository.DeleteContactAsync(id);
            return result.ToActionResult();
        }

        //update
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] ContactUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _contactRepository.UpdateContactAsync(id, dto);
            return result.ToActionResult();
        }

        //getall

        [HttpGet]
        [ProducesResponseType(typeof(Result<ContactViewDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ContactViewDto[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ContactViewDto[]>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllContacts()
        {
            var result = await _contactRepository.GetAllContactsAsync();
            return result.ToActionResult();
        }

        //getbyID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<ContactViewDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ContactViewDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ContactViewDto?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _contactRepository.GetByIdAsync(id);
            return result.ToActionResult();
        }
    }
}
