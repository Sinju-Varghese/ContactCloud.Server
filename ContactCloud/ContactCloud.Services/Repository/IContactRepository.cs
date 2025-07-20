using ContactCloud.Common.Types;
using ContactCloud.Services.Dto.Contact;

namespace ContactCloud.Services.Repository;

public interface IContactRepository
{
    Task<Result<long?>> CreateContactAsync(ContactCreateDto model, string userId);
    Task<Result<bool>> DeleteContactAsync(long contactId);

}
