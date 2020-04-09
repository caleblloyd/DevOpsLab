using System;
using System.Threading.Tasks;
using DevOpsLab.Server.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsLab.Server.Migrations
{
    [DbContext(typeof(AppDb))]
    [Migration("99999999000000_Roles")]
    public class Roles : Migration
    {
        private readonly string[] _roleNames = {"Admin", "Instructor", "Student"};

        private static void AwaitWithRoleManager(Func<RoleManager<IdentityRole>, Task> fn)
        {
            var hostBuilder = Program.CreateHostBuilder(new string[]{});
            using var host = hostBuilder.Build();
            using var scope = host.Services.CreateScope();
            using var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            fn(roleManager).GetAwaiter().GetResult();
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            AwaitWithRoleManager(UpAsync);
        }

        private async Task UpAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in _roleNames)
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            AwaitWithRoleManager(DownAsync);
        }

        private async Task DownAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in _roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                await roleManager.DeleteAsync(role);
            }
        }
    }
}
