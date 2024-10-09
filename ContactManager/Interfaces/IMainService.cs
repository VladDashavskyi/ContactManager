

using ContactManager.BLL.DTO;
using ContactManager.DAL.Entity;

namespace ContactManager.Interfaces
{
    public interface IMainService
    {
        Task<IEnumerable<Person>> GetPersonsAsync();
        Task<IEnumerable<PersonDto>> UploadFileAsync(IFormFile file);
        Task SaveFileToDbAsync(List<PersonDto> personDtos);

    }
}
