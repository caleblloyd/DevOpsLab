using System;
using System.Linq;
using DevOpsLab.Server.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Grains;
using DevOpsLab.Server.Helpers.Services;
using DevOpsLab.Server.Services;
using DevOpsLab.Shared.Models;
using DevOpsLab.Shared;
using Microsoft.AspNetCore.Identity;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace DevOpsLab.Server
{
    public class Startup
    {
        private readonly AppConfig _config;

        public Startup(IConfiguration configuration)
        {
            _config = new AppConfig(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureEntityFramework(_config);

            services.AddDefaultIdentity<AppUser>(options => { options.SignIn.RequireConfirmedAccount = true; })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDb>();

            services.AddIdentityServer()
                .AddApiAuthorization<AppUser, AppDb>(options =>
                {
                    // https://github.com/dotnet/AspNetCore.Docs/issues/17649
                    options.IdentityResources["openid"].UserClaims.Add("role");
                    options.ApiResources.Single().UserClaims.Add("role");
                });

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options => options.AddAppPolicies());

            services.AddControllersWithViews();
            services.AddHealthChecks();
            services.AddRazorPages();

            services.ConfigureEntityFramework(_config)
                .AddSingleton(_config)
                .AddSingleton<IHostedService, HostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHealthChecks("/health");
                endpoints.MapRazorPages();
            });
        }

        // This method gets called from Program.cs
        public static void ConfigureOrleans(HostBuilderContext hostBuilderContext, ISiloBuilder siloBuilder)
        {
            var config = new AppConfig(hostBuilderContext.Configuration);
            siloBuilder
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "DevOpsLab";
                    options.ServiceId = "DevOpsLab.Server";
                })
                .Configure<ClusterMembershipOptions>(options =>
                {
                    // ADO.NET cluster membership provider does not support Defunct Silo Cleanup
                    options.DefunctSiloCleanupPeriod = TimeSpan.MaxValue;
                })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = "Npgsql";
                    options.ConnectionString = config.ConnectionString;
                })
                .UseAdoNetReminderService(options =>
                {
                    options.Invariant = "Npgsql";
                    options.ConnectionString = config.ConnectionString;
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(MembershipTableCleanupGrain).Assembly).WithReferences();
                })
                .ConfigureEndpoints(5010, 5011);
        }
    }
}