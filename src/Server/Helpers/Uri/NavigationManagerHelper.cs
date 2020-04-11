using System;
using Microsoft.AspNetCore.Components;

namespace DevOpsLab.Server.Helpers.Uri
{
    public static class NavHelper
    {
        public static UriBuilder UriBuilder(this NavigationManager nm) => new UriBuilder(nm.Uri);
    }
}