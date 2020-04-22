using System;
using DevOpsLab.Server.Db;
using DevOpsLab.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DevOpsLab.Server.Hubs.Instruct
{
    [Authorize(Policy = PolicyTypes.RequireInstructor)]
    public partial class InstructHub : Hub
    {
        private readonly AppDb _db;
        private readonly ILogger<InstructHub> _logger;

        public InstructHub(AppDb db, ILogger<InstructHub> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        private void LogException(Exception e)
        {
            _logger.LogError(e.Message + "\n" + e.StackTrace, e);
        }
    }
}
