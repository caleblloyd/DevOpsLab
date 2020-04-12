using System;
using System.Threading;
using System.Threading.Tasks;
using DevOpsLab.Server.Config;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Grains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace DevOpsLab.Server.Services
{
    public class HostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IGrainFactory _grainFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HostedService(
            IHostApplicationLifetime appLifetime,
            IServiceScopeFactory serviceScopeFactory,
            IGrainFactory grainFactory)
        {
            _appLifetime = appLifetime;
            _grainFactory = grainFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (AppEnv.IsLocalDevelopment)
            {
                // local development - clear out membership table on restart
                using var scope = _serviceScopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetService<AppDb>();
                await db.Database.ExecuteSqlRawAsync("DELETE FROM OrleansMembershipTable", cancellationToken);
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
