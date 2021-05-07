using GenericRepository.EFCore.Repositories;
using TextService.Data.Context;
using TextService.Data.Models;

namespace TextService.Data.Repositories
{
    internal class TextRepository : GenericRepository<TextContext, Text>, ITextRepository
    {
        public TextRepository(TextContext context) : base(context)
        {
        }
    }
}
