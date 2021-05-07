using Microsoft.EntityFrameworkCore;
using TextService.Data.Models;

namespace TextService.Data.Context
{
    internal sealed class TextContext : DbContext
    {
        public DbSet<Text> Texts { get; set; }

        public TextContext()
        {

        }

        public TextContext(DbContextOptions<TextContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
