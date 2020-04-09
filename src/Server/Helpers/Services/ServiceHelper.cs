using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsLab.Server.Helpers.Services
{
    public static class ServiceHelper
    {
        public static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, AppConfig config)
        {
            return services.AddDbContext<AppDb>(options => options.UseNpgsql(config.ConnectionString),
                ServiceLifetime.Transient);
        }
    }
}
