using DevOpsLab.Server.Cmd.Commands;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Helpers.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsLab.Server.Cmd
{
    public class CommandStartup
    {
        private readonly AppConfig _config;
        
        public CommandStartup(IConfiguration configuration)
        {
            _config = new AppConfig(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureEntityFramework(_config)
                .AddSingleton(_config)
                .AddScoped<DropDbCommand>()
                .AddScoped<MigrateCommand>()
                .AddScoped<SeedDataCommand>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}