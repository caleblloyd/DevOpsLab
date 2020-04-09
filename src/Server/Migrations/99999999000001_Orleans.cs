using System;
using System.IO;
using System.Threading.Tasks;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsLab.Server.Migrations
{
    [DbContext(typeof(AppDb))]
    [Migration("99999999000001_Orleans")]
    public class Orleans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(
                AppEnv.DataDir, "Orleans", "Ado.Net", "PostgreSQL-Main.sql"
            )));
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(
                AppEnv.DataDir, "Orleans", "Ado.Net", "PostgreSQL-Clustering.sql"
            )));
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(
                AppEnv.DataDir, "Orleans", "Ado.Net", "PostgreSQL-Reminders.sql"
            )));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION delete_reminder_row;
                DROP FUNCTION upsert_reminder_row;
                DROP FUNCTION update_membership;
                DROP FUNCTION insert_membership;
                DROP FUNCTION insert_membership_version;
                DROP FUNCTION update_i_am_alive_time;
                DROP TABLE OrleansQuery;
                DROP TABLE OrleansRemindersTable;
                DROP TABLE OrleansMembershipTable;
                DROP TABLE OrleansMembershipVersionTable;
            ");
        }
    }
}