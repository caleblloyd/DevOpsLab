using System;
using DevOpsLab.Server.Cmd;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevOpsLab.Server
{
    public static class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            }

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        if (AppEnv.IsLocalDevelopment)
                        {
                            config.AddJsonFile("appsettings.Override.json", true);
                        }
                    });

                    if (args.Length == 0)
                    {
                        builder.UseStartup<Startup>();
                        if (AppEnv.IsLocalDevelopment)
                        {
                            builder.UseStaticWebAssets();
                        }
                    }
                    else
                    {
                        builder.UseStartup<CommandStartup>();
                    }
                });

            if (args.Length == 0)
            {
                hostBuilder.UseOrleans(Startup.ConfigureOrleans);
            }

            return hostBuilder;
        }

        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();

            if (args.Length == 0)
            {
                if (AppEnv.IsLocalDevelopment)
                {
                    // local development - clear out membership table on restart
                    using var scope = host.Services.CreateScope();
                    using var db = scope.ServiceProvider.GetService<AppDb>();
                    db.Database.ExecuteSqlRaw("DELETE FROM OrleansMembershipTable");
                }

                host.Run();
            }
            else
            {
                Environment.Exit(CommandRunner.Run(host.Services, args));
            }
        }
    }
}
