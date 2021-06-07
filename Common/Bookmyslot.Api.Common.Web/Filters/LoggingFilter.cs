using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Logging.Constants;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Web.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUser currentUser;
        private readonly ILoggerService loggerService;
        public LoggingFilter(IHttpContextAccessor httpContextAccessor, ICurrentUser currentUser, ILoggerService loggerService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.currentUser = currentUser;
            this.loggerService = loggerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var operationName = context.ActionDescriptor.DisplayName;
            var operationName  = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var userName = currentUserResponse.Result.UserName;

            this.httpContextAccessor.HttpContext.Request.Headers.Add(LogConstants.Username, userName);
            var actionLog = new ActionLog(operationName);

            this.loggerService.Information("Operation Started {@action}", actionLog);
            await next();

            this.loggerService.Information("Operation Ended {@action}", actionLog);
        }
    }
}
