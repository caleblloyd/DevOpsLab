using System;
using System.Threading;
using System.Threading.Tasks;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Grains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace DevOpsLab.Server.Services
{
    public class HostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly AppDb _db;
        private readonly IGrainFactory _grainFactory;

        public HostedService(
            IHostApplicationLifetime appLifetime,
            AppDb db,
            IGrainFactory grainFactory)
        {
            _db = db;
            _appLifetime = appLifetime;
            _grainFactory = grainFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (AppEnv.IsLocalDevelopment)
            {
                // local development - clear out membership table on restart
                await _db.Database.ExecuteSqlRawAsync("DELETE FROM OrleansMembershipTable", cancellationToken);
            }

            _appLifetime.ApplicationStarted.Register(async () =>
            {
                // register reminders
                var membershipTableCleanupGrain = _grainFactory.GetGrain<IMembershipTableCleanupGrain>(Guid.Empty);
                await membershipTableCleanupGrain.RegisterReminders();
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}