using System.Threading.Tasks;
using Orleans;

namespace DevOpsLab.Server.Grains.Interfaces
{
    public interface IMembershipTableCleanupGrain : IGrainWithGuidKey
    {
        public Task Cleanup();

        public Task RegisterReminders();
    }
}
