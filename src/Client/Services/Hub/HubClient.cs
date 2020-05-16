using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevOpsLab.Client.Services.Hub
{
    public abstract class HubClient
    {
        public readonly HubConnection HubConnection;
        protected readonly NavigationManager NavigationManager;

        private bool _connected;
        private readonly SemaphoreSlim _connectedSemaphoreSlim = new SemaphoreSlim(1);
        private readonly List<SemaphoreSlim> _connectedWaitSemaphoreSlims = new List<SemaphoreSlim>();
        private bool _shouldConnect;
        private readonly object _shouldConnectLock = new object();

        protected HubClient(IAccessTokenProvider accessTokenProvider, NavigationManager navigationManager)
        {
            NavigationManager = navigationManager;
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
            Task.Run(LocationChanged);
            navigationManager.LocationChanged += (sender, args) => { Task.Run(LocationChanged); };
        }

        protected abstract string Endpoint { get; set; }

        protected abstract bool ShouldConnect { get; }

        private async Task LocationChanged()
        {
            // set latest value of _shouldConnect
            lock (_shouldConnectLock)
            {
                _shouldConnect = ShouldConnect;
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

                if (_connected)
                {
                    // clear out all waiting semaphores
                    foreach (var connectedWaitSemaphoreSlim in _connectedWaitSemaphoreSlims)
                    {
                        connectedWaitSemaphoreSlim.Release();
                    }

                    _connectedWaitSemaphoreSlims.Clear();
                }
            }
            finally
            {
                // release the semaphore
                _connectedSemaphoreSlim.Release();
            }
        }

        public async Task WaitConnectedAsync()
        {
            SemaphoreSlim connectedWaitSemaphoreSlim;
            await _connectedSemaphoreSlim.WaitAsync();
            try
            {
                if (_connected)
                {
                    return;
                }

                connectedWaitSemaphoreSlim = new SemaphoreSlim(1);
                await connectedWaitSemaphoreSlim.WaitAsync();
                _connectedWaitSemaphoreSlims.Add(connectedWaitSemaphoreSlim);
            }
            finally
            {
                // release the semaphore
                _connectedSemaphoreSlim.Release();
            }

            await connectedWaitSemaphoreSlim.WaitAsync();
        }
    }
}
