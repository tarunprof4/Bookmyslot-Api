using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Web.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var operationName = context.ActionDescriptor.DisplayName;
            var operationName  = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var user = "a@gmail.com";
            var actionLog = new ActionLog(operationName, user);

            Log.Information("Operation Started {@action}", actionLog);
            await next();

            Log.Information("Operation Ended {@action}", actionLog);
        }
    }
}
