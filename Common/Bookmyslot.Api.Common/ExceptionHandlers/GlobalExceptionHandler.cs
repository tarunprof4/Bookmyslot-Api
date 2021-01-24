using Bookmyslot.Api.Common.Contracts.Constants;
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
                        Log.Error("{requestId} {message}",  requestId, contextFeature.Error.Message);
                        Log.Error(contextFeature.Error, "{requestId}", requestId);
                        //Log.Error(requestId + " " + contextFeature.Error.StackTrace);

                        //await context.Response.WriteAsync(new ErrorDetails()
                        //{
                        //    StatusCode = context.Response.StatusCode,
                        //    Message = "Internal Server Error."
                        //}.ToString());


                        

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new List<string>() { "Internal Server Error" }));
                    }
                });
            });
        }
    }
}

