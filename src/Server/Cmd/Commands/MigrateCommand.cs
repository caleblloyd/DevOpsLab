using System;
using System.Data;
using System.Threading.Tasks;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DevOpsLab.Server.Cmd.Commands
{
    public class MigrateCommand
    {
        private readonly AppConfig _config;
        private readonly AppDb _db;
        private readonly SeedDataCommand _seedDataCommand;

        public MigrateCommand(AppConfig appConfig, AppDb appDb, SeedDataCommand seedDataCommand)
        {
            _config = appConfig;
            _db = appDb;
            _seedDataCommand = seedDataCommand;
        }

        public async Task RunAsync()
        {
            var dbName = _config.DbName;
            if (string.IsNullOrWhiteSpace(dbName))
            {
                throw new Exception("Unable to identify database name");
            }

            bool dbExists;
            await using (var connection = new NpgsqlConnection(_config.MasterConnectionString))
            {
                await connection.OpenAsync();
                await using var command = connection.CreateCommand();
                command.CommandText =
                    @"SELECT EXISTS (SELECT ""datname"" FROM ""pg_database"" WHERE ""datname"" = @dbName)";
                command.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@dbName",
                    DbType = DbType.String,
                    Value = dbName,
                });
                dbExists = Convert.ToBoolean(await command.ExecuteScalarAsync());
            }

            Console.WriteLine("Running migrations...");
            await _db.Database.MigrateAsync();
            Console.WriteLine("Done");

            if (!dbExists)
            {
                await _seedDataCommand.RunAsync();
            }
        }
    }
}
