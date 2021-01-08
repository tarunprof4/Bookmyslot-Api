using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.ExceptionHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Bookmyslot.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();


            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Bookmyslot Customer API";
                    document.Info.Description = "Bookmyslot Customer API to manage customer data";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "TA",
                        Email = string.Empty,
                        //Url = "https://twitter.com/spboyer"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "",
                        //Url = "https://example.com/license"
                    };
                };
            });


            services.AddMvc()
       .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
       .ConfigureApiBehaviorOptions(options =>
       {
           options.InvalidModelStateResponseFactory = context =>
           {
               var problems = new BadRequestExceptionHandler(context);
               return new BadRequestObjectResult(problems.ErrorMessages);
           };
       });

        }


        private Dictionary<string, string> GetAppConfigurations()
        {
            Dictionary<string, string> appConfigurations = new Dictionary<string, string>();
            var bookMySlotConnectionString = Configuration.GetConnectionString(AppConfigurations.BookMySlotDatabase);
            appConfigurations.Add(AppConfigurations.BookMySlotDatabaseConnectionString, bookMySlotConnectionString);

            return appConfigurations;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
