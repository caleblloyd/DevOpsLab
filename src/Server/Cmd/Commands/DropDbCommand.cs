using System;
using System.Data;
using System.Threading.Tasks;
using DevOpsLab.Server.Config;
using Npgsql;

namespace DevOpsLab.Server.Cmd.Commands
{
    public class DropDbCommand
    {
        private readonly AppConfig _config;

        public DropDbCommand(AppConfig appConfig)
        {
            _config = appConfig;
        }

        public async Task RunAsync()
        {
            if (AppEnv.IsProd)
            {
                throw new Exception("Cowardly refusing to drop prod database");
            }

            var dbName = _config.DbName;
            if (string.IsNullOrWhiteSpace(dbName))
            {
                throw new Exception("Unable to identify database name");
            }

            Console.WriteLine($"Dropping database '{dbName}'...");

            await using var connection = new NpgsqlConnection(_config.MasterConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"SELECT pg_terminate_backend(""pid"")
FROM ""pg_stat_activity""
WHERE ""datname"" = @dbName
AND ""pid"" != pg_backend_pid();
DROP DATABASE IF EXISTS """ + dbName + @"""";
            command.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@dbName",
                DbType = DbType.String,
                Value = dbName,
            });
            await command.ExecuteNonQueryAsync();

            Console.WriteLine("Done");
        }
    }
}
