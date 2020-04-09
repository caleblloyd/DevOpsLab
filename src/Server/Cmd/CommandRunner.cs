using System;
using DevOpsLab.Server.Cmd.Commands;
using DevOpsLab.Server.Config;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsLab.Server.Cmd
{
    public static class CommandRunner
    {
        private static void Help()
        {
            Console.Error.WriteLine(@"app
    dropDb                     drop database
    migrate                    run migrations
    seedData                   insert seed data into database
    -h, --help                 show this message
            ");
        }

        public static int Run(IServiceProvider serviceProvider, string[] args)
        {
            var cmd = args[0];

            try
            {
                using var scope = serviceProvider.CreateScope();
                switch (cmd)
                {
                    case "dropDb":
                        scope.ServiceProvider.GetService<DropDbCommand>()
                            .RunAsync()
                            .GetAwaiter()
                            .GetResult();
                        break;
                    case "migrate":
                        scope.ServiceProvider.GetService<MigrateCommand>()
                            .RunAsync()
                            .GetAwaiter()
                            .GetResult();
                        break;
                    case "seedData":
                        scope.ServiceProvider.GetService<SeedDataCommand>()
                            .RunAsync()
                            .GetAwaiter()
                            .GetResult();
                        break;
                    case "-h":
                    case "--help":
                        Help();
                        break;
                    default:
                        Help();
                        return 1;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                throw;
            }

            return 0;
        }
    }
}