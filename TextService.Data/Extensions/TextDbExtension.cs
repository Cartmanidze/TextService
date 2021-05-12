using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextService.Data.Context;
using TextService.Data.Repositories;

namespace TextService.Data.Extensions
{
    public static class TextDbExtension
    {
        public static void AddTextDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TextContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("AppConnectionString");
                opt.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<ITextRepository, TextRepository>();
        }
    }
}
