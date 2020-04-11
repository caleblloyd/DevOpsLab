using System;
using Microsoft.AspNetCore.Components.Routing;

namespace DevOpsLab.Client.Helpers
{
    public static class NavHelper
    {
        public static UriBuilder UriBuilder(this LocationChangedEventArgs locationChanged) =>
            new UriBuilder(locationChanged.Location);
    }
}