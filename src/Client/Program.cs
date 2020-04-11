using System.Threading.Tasks;
using DevOpsLab.Client.Services;
using DevOpsLab.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

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
            builder.Services.AddApiAuthorization();
            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<
                    IPostConfigureOptions<RemoteAuthenticationOptions<ApiAuthorizationProviderOptions>>,
                    ApiAuthorizationOptionsConfiguration>());

            await builder.Build().RunAsync();
        }
    }
}