using System;
using Microsoft.AspNetCore.Components;

namespace DevOpsLab.Client.Helpers
{
    public static class NavigationManagerHelper
    {
        public static UriBuilder UriBuilder(this NavigationManager navigationManager) =>
            new UriBuilder(navigationManager.Uri);

        public static bool PathActive(this NavigationManager navigationManager, string path, bool exact = false)
        {
            return exact
                ? navigationManager.UriBuilder().Path.Equals(path, StringComparison.OrdinalIgnoreCase)
                : navigationManager.PathStartsWithSegments(path);
        }

        public static string PathActiveClass(this NavigationManager navigationManager, string path, bool exact = false)
        {
            return navigationManager.PathActive(path, exact)
                ? "active"
                : "";
        }

        public static bool PathStartsWithSegments(this NavigationManager navigationManager, string path)
        {
            var value1 = navigationManager.UriBuilder().Path;
            var value2 = path ?? string.Empty;
            if (value2.StartsWith("/"))
            {
                value2 = value2.Substring(1);
            }

            value2 = new UriBuilder(navigationManager.BaseUri).Path + value2;
            if (value1.StartsWith(value2, StringComparison.OrdinalIgnoreCase))
            {
                return value1.Length == value2.Length || value1[value2.Length] == '/';
            }

            return false;
        }
    }
}
