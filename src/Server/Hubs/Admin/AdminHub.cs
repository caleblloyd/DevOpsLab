using System;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Models;
using DevOpsLab.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DevOpsLab.Server.Hubs.Admin
{
    [Authorize(Policy = PolicyTypes.RequireAdmin)]
    public partial class AdminHub : Hub
    {
        private readonly AppDb _db;
        private readonly ILogger<AdminHub> _logger;
        private readonly UserManager<AppUser> _userManager;

        public AdminHub(AppDb db, ILogger<AdminHub> logger, UserManager<AppUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        private void LogException(Exception e)
        {
            _logger.LogError(e.Message + "\n" + e.StackTrace, e);
        }
    }
}
