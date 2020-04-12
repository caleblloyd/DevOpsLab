using System;
using DevOpsLab.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevOpsLab.Client.Services.Hub
{
    public class TrainHubClient : HubClient
    {
        public TrainHubClient(
            IAccessTokenProvider accessTokenProvider,
            NavigationManager navigationManager)
            : base(accessTokenProvider, navigationManager)
        {
        }

        protected override string Endpoint { get; set; } = "/hubs/train";

        protected override bool ShouldConnect(UriBuilder uriBuilder) =>
            uriBuilder.IsInstructPath()
            || uriBuilder.IsTrainPath();
    }
}
