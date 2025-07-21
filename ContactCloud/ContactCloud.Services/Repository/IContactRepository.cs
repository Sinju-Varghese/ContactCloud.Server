using ContactCloud.Common.Types;
using ContactCloud.Services.Dto.Contact;

namespace ContactCloud.Services.Repository;

public interface IContactRepository
{
    Task<Result<int?>> CreateContactAsync(ContactCreateDto dto);
    Task<Result<bool>> UpdateContactAsync(int id, ContactUpdateDto dto);
    Task<Result<bool>> DeleteContactAsync(int id);
    Task<Result<ContactViewDto[]>> GetAllContactsAsync();
    Task<Result<ContactViewDto?>> GetByIdAsync(int id);

}
