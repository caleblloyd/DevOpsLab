using System;
using System.Threading.Tasks;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;

namespace DevOpsLab.Server.Cmd.Commands
{
    public class SeedDataCommand
    {
        private readonly AppDb _db;

        public SeedDataCommand(AppDb appDb)
        {
            _db = appDb;
        }

        private Task SeedAllAsync()
        {
            return Task.CompletedTask;
        }

        private Task SeedLocalDevelopmentAsync()
        {
            return Task.CompletedTask;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Seeding database...");
            await SeedAllAsync();
            if (AppEnv.IsLocalDevelopment)
            {
                await SeedLocalDevelopmentAsync();
            }

            await _db.SaveChangesAsync();
            Console.WriteLine("Done");
        }
    }
}