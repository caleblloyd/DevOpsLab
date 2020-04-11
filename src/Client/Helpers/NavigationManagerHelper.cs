using System;
using Microsoft.AspNetCore.Components;

namespace DevOpsLab.Client.Helpers
{
    public static class NavigationManagerHelper
    {
        public static UriBuilder UriBuilder(this NavigationManager navigationManager) =>
            new UriBuilder(navigationManager.Uri);
    }
}