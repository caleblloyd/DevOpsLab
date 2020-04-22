using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DevOpsLab.Server.Helpers.Hub
{
    public static class HubHelper
    {
        public static async Task<T> WrapAsync<T>(ILogger logger, Func<Task<T>> fn)
        {
            try
            {
                return await fn();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace, e);
                throw;
            }
        }
    }
}
