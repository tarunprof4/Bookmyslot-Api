using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common
{
    public class LogAttributeFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.HttpContext.Request.Headers.Add(LogConstants.RequestId, requestId);
            Log.Information(requestId);
            Log.Information(string.Format("Action Method {0} executing at {1}", context.ActionDescriptor.DisplayName, DateTime.UtcNow));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var requestId = context.HttpContext.Request.Headers[LogConstants.RequestId];
            Log.Information(requestId);
            Log.Information(string.Format("Action Method {0} executed at {1}", context.ActionDescriptor.DisplayName, DateTime.UtcNow));
        }
    }
}
