using System;

namespace DevOpsLab.Client.Helpers
{
    public static class UriBuilderHelper
    {
        public static bool IsRoot(this UriBuilder uriBuilder) =>
            uriBuilder.Path == "/";

        public static bool IsError(this UriBuilder uriBuilder) =>
            uriBuilder.Path == "/error";

        public static bool IsAdminPath(this UriBuilder uriBuilder) =>
            uriBuilder.Path == "/admin" || uriBuilder.Path.StartsWith("/admin/");

        public static bool IsInstructPath(this UriBuilder uriBuilder) =>
            uriBuilder.Path == "/instruct" || uriBuilder.Path.StartsWith("/instruct/");

        public static bool IsTrainPath(this UriBuilder uriBuilder) =>
            uriBuilder.Path == "/train" || uriBuilder.Path.StartsWith("/train/");
    }
}