using Microsoft.AspNetCore.Authorization;

namespace DevOpsLab.Shared
{
    public static class PolicyTypes
    {
        public const string RequireAdmin = "RequireAdmin";
        public const string RequireInstructor = "RequireInstructor";

        public static AuthorizationOptions AddAppPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(RequireAdmin, policy =>
                policy.RequireRole(RoleTypes.Admin));
            options.AddPolicy(RequireInstructor, policy =>
                policy.RequireRole(RoleTypes.Admin, RoleTypes.Instructor));
            return options;
        }
    }
}
