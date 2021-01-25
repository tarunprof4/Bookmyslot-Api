using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Net;

namespace Bookmyslot.Api.Common.ExceptionHandlers
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureGlobalExceptionHandler(this IApplicationBuilder app)
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
                        var errorLog = new ErrorLog()
                        {
                            RequestId = requestId,
                            Message = contextFeature.Error.Message,
                            Exception = contextFeature.Error
                        };

                        Log.Error("{@errorLog}", errorLog);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new List<string>() { "Internal Server Error" }));
                    }
                });
            });
        }
    }
}

