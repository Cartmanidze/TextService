using TextService.Data.Models;
using GenericRepository.EFCore.Repositories;

namespace TextService.Data.Repositories
{
    public interface ITextRepository : IGenericRepository<Text>
    {
    }
}
