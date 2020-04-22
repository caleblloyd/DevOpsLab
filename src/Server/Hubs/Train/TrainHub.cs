using System;
using System.Threading.Tasks;
using DevOpsLab.Server.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DevOpsLab.Server.Hubs.Train
{
    [Authorize]
    public partial class TrainHub : Hub
    {
        private readonly AppDb _db;
        private readonly ILogger<TrainHub> _logger;

        public TrainHub(AppDb db, ILogger<TrainHub> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        private void LogException(Exception e)
        {
            _logger.LogError(e.Message + "\n" + e.StackTrace, e);
        }

        public Task ListTrainingCodes()
        {
            Console.WriteLine(Context.UserIdentifier);
            return Task.CompletedTask;
        }
    }
}
