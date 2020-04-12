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

        protected override bool ShouldConnect =>
            NavigationManager.PathActive(PathHelper.AdminPath);

        protected override string Endpoint { get; set; } = "/hubs/admin";
    }
}
