using GenericRepository.EFCore.Models;

namespace TextService.Data.Models
{
    public class Text : BaseModel
    {
        public string Value { get; set; }
    }
}
