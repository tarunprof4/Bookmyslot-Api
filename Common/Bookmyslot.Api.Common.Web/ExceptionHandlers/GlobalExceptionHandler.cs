using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Bookmyslot.Api.Common.Web.ExceptionHandlers
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureGlobalExceptionHandler(this IApplicationBuilder app, ILoggerService loggerService)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var requestId = context.Request.Headers[LogConstants.RequestId];

                        loggerService.Error(contextFeature.Error, string.Empty);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new List<string>() { "Internal Server Error" }));
                    }
                });
            });
        }
    }
}

