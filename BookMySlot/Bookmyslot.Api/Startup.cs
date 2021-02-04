using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Bookmyslot.Api.Common.ExceptionHandlers;
using Bookmyslot.Api.Common.Injections;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Customers.Injections;
using Bookmyslot.Api.Search.Injections;
using Bookmyslot.Api.SlotScheduler.Injections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
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
            
            services.AddHttpContextAccessor();

            Dictionary<string, string> appConfigurations = GetAppConfigurations();

            CommonInjection.CommonInjections(services);

            CustomerInjection.CustomerBusinessInjections(services);
            CustomerInjection.CustomerRepositoryInjections(services, appConfigurations);

            SlotSchedulerInjection.SlotSchedulerCommonInjections(services);
            SlotSchedulerInjection.SlotSchedulerBusinessInjections(services);
            SlotSchedulerInjection.SlotSchedulerRepositoryInjections(services, appConfigurations);


            SearchInjection.SearchBusinessInjections(services);
            SearchInjection.SearchRepositoryInjections(services, appConfigurations);



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
            var bookMySlotConnectionString = Configuration.GetConnectionString(AppConfigurationConstants.BookMySlotDatabase);
            appConfigurations.Add(AppConfigurationConstants.BookMySlotDatabaseConnectionString, bookMySlotConnectionString);

            return appConfigurations;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            InitializeSerilog(serviceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRequestResponseLogging();

            app.UseHttpsRedirection();



            app.ConfigureGlobalExceptionHandler();

            app.UseRouting();


            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });





        }

        private static void InitializeSerilog(IServiceProvider serviceProvider)
        {

            var defaultLogEnricher = serviceProvider.GetService<DefaultLogEnricher>();
            var appConfiguration = serviceProvider.GetService<IAppConfiguration>();


            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.With(defaultLogEnricher)
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
             outputTemplate: appConfiguration.LogOutputTemplate)
            .CreateLogger();


            //Log.Logger = new LoggerConfiguration()
            //                       .MinimumLevel.Verbose()
            //                       .Enrich.With(defaultLogEnricher)
            //                       .Enrich.WithElasticApmCorrelationInfo()
            //                       .WriteTo.Async(a => a.Elasticsearch(new ElasticsearchSinkOptions(new Uri(appConfiguration.ElasticSearchUrl))
            //                       {
            //                           CustomFormatter = new EcsTextFormatter()
            //                       }))
            //                       .CreateLogger();



            Log.Debug("Starting Bookmyslot web host");
        }
    }
}
