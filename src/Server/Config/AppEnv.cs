using System;
using System.IO;
using System.Reflection;

namespace DevOpsLab.Server.Config
{
    public static class AppEnv
    {
        public static IServiceProvider ServiceProvider;
        
        public static string AspNetCoreEnvironment =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // directories
        private static readonly Lazy<string> LazyBaseDir =
            new Lazy<string>(() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        public static string BaseDir => LazyBaseDir.Value;

        public static string DataDir => Path.Combine(BaseDir, "Data");

        public static bool IsLocalDevelopment =>
            AspNetCoreEnvironment == "Development" || AspNetCoreEnvironment == "DockerCompose";

        public static bool IsNonProd => !IsLocalDevelopment && !IsProd;

        public static bool IsProd => AspNetCoreEnvironment == "prod";
    }
}