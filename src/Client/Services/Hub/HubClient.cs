using System;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using DevOpsLab.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevOpsLab.Client.Services.Hub
{
    public abstract class HubClient
    {
        protected HubConnection HubConnection;

        // workaround https://github.com/dotnet/aspnetcore/pull/20466
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly NavigationManager _navigationManager;
        private readonly SemaphoreSlim _hubConnectionWorkaroundSemaphoreSlim = new SemaphoreSlim(1);

        private HubConnection _hubConnectionWorkaround;
        // end workaround

        private bool _connected;
        private readonly SemaphoreSlim _connectedSemaphoreSlim = new SemaphoreSlim(1);
        private bool _shouldConnect;
        private readonly object _shouldConnectLock = new object();

        protected HubClient(IAccessTokenProvider accessTokenProvider, NavigationManager navigationManager)
        {
            // workaround https://github.com/dotnet/aspnetcore/pull/20466
            _accessTokenProvider = accessTokenProvider;
            _navigationManager = navigationManager;
            // end workaround
            HubConnection = new HubConnectionBuilder()
                // ReSharper disable once VirtualMemberCallInConstructor
                .WithUrl(navigationManager.ToAbsoluteUri(Endpoint), options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var tokenResult = await accessTokenProvider.RequestAccessToken();
                        // got a token
                        if (tokenResult.TryGetToken(out var token))
                        {
                            return token.Value;
                        }

                        // did not get a token
                        navigationManager.NavigateTo(tokenResult.RedirectUrl);
                        return null;
                    };
                })
                .WithAutomaticReconnect()
                .Build();
            Task.Run(() => LocationChanged(navigationManager.UriBuilder()));
            navigationManager.LocationChanged += (sender, args) =>
            {
                Task.Run(() => LocationChanged(args.UriBuilder()));
            };
        }

        protected abstract string Endpoint { get; set; }

        protected abstract bool ShouldConnect(UriBuilder uriBuilder);

        private async Task LocationChanged(UriBuilder uriBuilder)
        {
            // workaround https://github.com/dotnet/aspnetcore/pull/20466
            if (_hubConnectionWorkaround == null)
            {
                await _hubConnectionWorkaroundSemaphoreSlim.WaitAsync();
                try
                {
                    if (_hubConnectionWorkaround == null)
                    {
                        string tokenValue;
                        var tokenResult = await _accessTokenProvider.RequestAccessToken();
                        // got a token
                        if (tokenResult.TryGetToken(out var token))
                        {
                            tokenValue = token.Value;
                            Console.WriteLine(tokenValue);
                        }
                        else
                        {
                            _navigationManager.NavigateTo(tokenResult.RedirectUrl);
                            return;
                        }

                        var accessTokenEncoded = UrlEncoder.Default.Encode(tokenValue);
                        var url = _navigationManager.ToAbsoluteUri(Endpoint) + "?access_token=" + accessTokenEncoded;
                        _hubConnectionWorkaround = new HubConnectionBuilder()
                            // ReSharper disable once VirtualMemberCallInConstructor
                            .WithUrl(url,
                                options => { options.AccessTokenProvider = () => Task.FromResult(tokenValue); })
                            .WithAutomaticReconnect()
                            .Build();
                        HubConnection = _hubConnectionWorkaround;
                    }
                }
                finally
                {
                    _hubConnectionWorkaroundSemaphoreSlim.Release();
                }
            }
            // end workaround

            // set latest value of _shouldConnect
            lock (_shouldConnectLock)
            {
                _shouldConnect = ShouldConnect(uriBuilder);
            }

            // acquire semaphore
            await _connectedSemaphoreSlim.WaitAsync();

            try
            {
                // get the latest value, after semaphore has been acquired, and save it to a local variable
                bool shouldConnectLocal;
                lock (_shouldConnectLock)
                {
                    shouldConnectLocal = _shouldConnect;
                }

                // start or stop connection, if necessary
                if (_connected != shouldConnectLocal)
                {
                    if (shouldConnectLocal)
                    {
                        await HubConnection.StartAsync();
                        _connected = true;
                    }
                    else
                    {
                        await HubConnection.StopAsync();
                        _connected = false;
                    }
                }
            }
            finally
            {
                // release the semaphore
                _connectedSemaphoreSlim.Release();
            }
        }
    }
}