using DevOpsLab.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsLab.Server.Hubs
{
    [Authorize(Policy = PolicyTypes.RequireInstructor)]
    public class InstructHub : Hub
    {
    }
}
