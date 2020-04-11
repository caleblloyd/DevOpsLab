using System;
using DevOpsLab.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevOpsLab.Client.Services.Hub
{
    public class InstructHubClient : HubClient
    {
        public InstructHubClient(
            IAccessTokenProvider accessTokenProvider,
            NavigationManager navigationManager)
            : base(accessTokenProvider, navigationManager)
        {
        }

        protected override string Endpoint { get; set; } = "/hubs/instruct";
        
        protected override bool ShouldConnect(UriBuilder uriBuilder) =>
            uriBuilder.IsInstructPath();
    }
}