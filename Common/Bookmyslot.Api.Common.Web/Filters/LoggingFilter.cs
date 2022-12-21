using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.Logging.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Web.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly ICurrentUser currentUser;
        private readonly ILoggerService loggerService;
        public LoggingFilter(ICurrentUser currentUser, ILoggerService loggerService)
        {
            this.currentUser = currentUser;
            this.loggerService = loggerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var operationName = context.ActionDescriptor.DisplayName;
            var operationName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var userName = currentUserResponse.Value.UserName;

            var actionLog = new ActionLog(operationName, userName);
            this.loggerService.Information("Operation Started {@action}", actionLog);
            await next();

            this.loggerService.Information("Operation Ended {@action}", actionLog);
        }
    }
}
