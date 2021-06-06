using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Common.Web.ExceptionHandlers;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.BackgroundTasks.Api.Contracts.Configuration;
using Bookmyslot.BackgroundTasks.Api.Injections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System;

namespace Bookmyslot.BackgroundTasks.Api
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
           
            Injections(services);

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            RegisterFilters(services);

            SwaggerDocumentation(services);
        }



        private static void RegisterFilters(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<LoggingFilter>();
            });
        }

        private void Injections(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            var appConfiguration = new AppConfiguration(Configuration);
            services.AddSingleton(appConfiguration);


            AppConfigurationInjection.LoadInjections(services, Configuration);
            CommonInjection.LoadInjections(services);
            BackgroundInjection.LoadInjections(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            InitializeSerilog(serviceProvider);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();


            var loggerService = serviceProvider.GetService<ILoggerService>();
            app.ConfigureGlobalExceptionHandler(loggerService);

            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    
        private static void InitializeSerilog(IServiceProvider serviceProvider)
        {
            var appConfiguration = serviceProvider.GetService<AppConfiguration>();
            var defaultLogEnricher = serviceProvider.GetService<DefaultLogEnricher>();
            

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


            var loggerService = serviceProvider.GetService<ILoggerService>();
            loggerService.Debug("Starting Background web host");
        }

      
        private static void SwaggerDocumentation(IServiceCollection services)
        {
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Bookmyslot Customer API";
                    document.Info.Description = "Bookmyslot Customer API to manage customer data";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "TA",
                        Email = string.Empty,
                        //Url = "https://twitter.com/spboyer"
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "",
                        //Url = "https://example.com/license"
                    };
                };

                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("apiKey"));
                config.DocumentProcessors.Add(new SecurityDefinitionAppender("apiKey", new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Bearer token"
                }));
            });
        }

    }
}
