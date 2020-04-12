using DevOpsLab.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsLab.Server.Hubs
{
    [Authorize(Policy = PolicyTypes.RequireAdmin)]
    public class AdminHub : Hub
    {
    }
}
