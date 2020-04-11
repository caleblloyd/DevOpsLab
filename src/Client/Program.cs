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

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddAuthorizationCore(options => options.AddAppPolicies());
            // 2 calls to AddApiAuthorization are necessary in 3.2-preview3
            // should be fixed in 3.2-preview4
            // https://github.com/dotnet/aspnetcore/issues/19854
            // https://github.com/dotnet/AspNetCore.Docs/issues/17649#issuecomment-612442543
            builder.Services.AddApiAuthorization();
            builder.Services.AddApiAuthorization(options =>
            {
                options.UserOptions.RoleClaim = "role";
            });
            builder.Services.AddSingleton<AdminHubClient>();
            builder.Services.AddSingleton<InstructHubClient>();
            builder.Services.AddSingleton<TrainHubClient>();

            await builder.Build().RunAsync();
        }
    }
}