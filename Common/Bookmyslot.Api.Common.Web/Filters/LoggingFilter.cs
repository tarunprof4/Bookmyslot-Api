using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Web.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly ILoggerService loggerService;
        public LoggingFilter(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var operationName = context.ActionDescriptor.DisplayName;
            var operationName  = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var user = "a@gmail.com";
            var actionLog = new ActionLog(operationName, user);

            this.loggerService.Information("Operation Started {@action}", actionLog);
            await next();

            this.loggerService.Information("Operation Ended {@action}", actionLog);
        }
    }
}
