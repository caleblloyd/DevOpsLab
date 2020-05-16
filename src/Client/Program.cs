using System;
using System.Net.Http;
using System.Threading.Tasks;
using DevOpsLab.Client.Services.Hub;
using DevOpsLab.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsLab.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddAuthorizationCore(options => options.AddAppPolicies());
            builder.Services.AddApiAuthorization(options => { options.UserOptions.RoleClaim = "role"; });
            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            builder.Services.AddSingleton<AdminHubClient>();
            builder.Services.AddSingleton<InstructHubClient>();
            builder.Services.AddSingleton<TrainHubClient>();

            await builder.Build().RunAsync();
        }
    }
}
