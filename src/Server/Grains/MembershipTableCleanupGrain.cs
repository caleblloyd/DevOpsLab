using System;
using System.Threading.Tasks;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Grains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace DevOpsLab.Server.Grains
{
    public class MembershipTableCleanupGrain : Grain, IMembershipTableCleanupGrain, IRemindable
    {
        private const int CleanupAfterMinutes = 15;
        private const int PeriodMinutes = 1;
        private readonly AppDb _db;
        private readonly ILogger<MembershipTableCleanupGrain> _logger;

        public MembershipTableCleanupGrain(AppDb appDb, ILogger<MembershipTableCleanupGrain> logger)
        {
            _db = appDb;
            _logger = logger;
            
        }

        public async Task Cleanup()
        {
            var aliveTime = DateTime.UtcNow - TimeSpan.FromMinutes(CleanupAfterMinutes);
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM OrleansMembershipTable WHERE Status = {SiloStatus.Dead} AND IAmAliveTime < {aliveTime}");
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (reminderName == "cleanup")
            {
                await Cleanup();
            }
        }

        public async Task RegisterReminders()
        {
            if (this.GetPrimaryKey() == Guid.Empty)
            {
                await RegisterOrUpdateReminder("cleanup", TimeSpan.Zero, TimeSpan.FromMinutes(PeriodMinutes));
            }
            else
            {
                _logger.LogError("Reminders can only be registered on key Guid.Empty");
            }
        }
    }
}