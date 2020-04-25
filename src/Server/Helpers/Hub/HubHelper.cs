using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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
            catch (HubException e)
            {
                logger.LogWarning(e.Message + "\n" + e.StackTrace, e);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace, e);
                throw;
            }
        }

        public static void Validate<T>(T model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, validationContext, validationResults))
            {
                throw new HubException(
                    "Please fix the following validation errors: " +
                    string.Join(" ", validationResults.Select(m => m.ErrorMessage)));
            }
        }
    }
}
