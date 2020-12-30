using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Net;

namespace Bookmyslot.Api.SlotScheduler
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
                        Log.Error(contextFeature.Error.Message + "\n");
                        Log.Error(contextFeature.Error.StackTrace);

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new List<string>() { "Internal Server Error" }));
                    }
                });
            });
        }
    }
}
