using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace DevOpsLab.Server.Services
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1
            // Sending the access token in the query string is required due to
            // a limitation in Browser APIs. We restrict it to only calls to the
            // SignalR hub in this code.

            // save the original OnMessageReceived event
            var originalOnMessageReceived = options.Events.OnMessageReceived;

            options.Events.OnMessageReceived = async context =>
            {
                // call the original OnMessageReceived event
                await originalOnMessageReceived(context);
                
                if (string.IsNullOrEmpty(context.Token))
                {
                    // attempt to read the access token from the query string
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                }
            };
        }
    }
}