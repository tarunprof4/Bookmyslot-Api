using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestId = context.HttpContext.Request.Headers[LogConstants.RequestId];
            //var operationName = context.ActionDescriptor.DisplayName;
            var operationName  = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var user = "a@gmail.com";
            var actionLog = new ActionLog(requestId, operationName, user);

            Log.Information("Operation Started {@action}", actionLog);
            await next();

            Log.Information("Operation Ended {@action}", actionLog);
        }
    }
}
