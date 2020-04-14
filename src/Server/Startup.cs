using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
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
using DevOpsLab.Server.Hubs;
using DevOpsLab.Server.Services;
using DevOpsLab.Server.Models;
using DevOpsLab.Shared;
using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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
                    options.IdentityResources["openid"].UserClaims.Add(JwtClaimTypes.Name);
                    options.IdentityResources["openid"].UserClaims.Add(JwtClaimTypes.Role);
                    options.ApiResources.Single().UserClaims.Add(JwtClaimTypes.Role);
                });
            // Need to do this as it maps "role" to ClaimTypes.Role and causes issues
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove(JwtClaimTypes.Role);

            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>());

            services.AddAuthorization(options => options.AddAppPolicies());

            services.AddControllersWithViews();
            services.AddSignalR();
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
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AdminHub>("/hubs/admin");
                endpoints.MapHub<InstructHub>("/hubs/instruct");
                endpoints.MapHub<TrainHub>("/hubs/train");
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
