using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsLab.Client.Helpers
{
    public static class HubHelper
    {
        public static async Task WrapAsync(Action<string> exceptFn, Func<Task> fn)
        {
            try
            {
                await fn();
            }
            catch (HubException e)
            {
                var split = e.Message.Split("HubException: ", StringSplitOptions.RemoveEmptyEntries);
                exceptFn(split.Length > 1 ? split[1] : split[0]);
                throw;
            }
            catch (Exception)
            {
                exceptFn("An unspecified error has occurred");
                throw;
            }
        }
    }
}
