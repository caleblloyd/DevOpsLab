using System;
using DevOpsLab.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevOpsLab.Client.Services.Hub
{
    public class AdminHubClient : HubClient
    {
        public AdminHubClient(
            IAccessTokenProvider accessTokenProvider,
            NavigationManager navigationManager)
            : base(accessTokenProvider, navigationManager)
        {
        }

        protected override bool ShouldConnect(UriBuilder uriBuilder) =>
            uriBuilder.IsAdminPath();
        
        protected override string Endpoint { get; set; } = "/hubs/admin";
    }
}