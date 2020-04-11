using System;
using System.Linq;
using System.Threading.Tasks;
using DevOpsLab.Server.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsLab.Server.Hubs
{
    [Authorize]
    public class TrainHub : Hub
    {
        private AppDb _db;
        
        public TrainHub(AppDb db)
        {
            _db = db;
        }
        
        public Task ListTrainingCodes()
        {
            Console.WriteLine(Context.UserIdentifier);
            return Task.CompletedTask;
        }
    }
}