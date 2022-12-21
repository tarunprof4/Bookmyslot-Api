using Bookmyslot.Api.Common.Web.ExceptionHandlers;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Configuration;
using Bookmyslot.BackgroundTasks.Api.Injections;
using Bookmyslot.BackgroundTasks.Api.Logging.Enrichers;
using Bookmyslot.Bookmyslot.Api.Common.Search.Constants;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using NSwag;
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


            services.AddControllers();
            //RegisterFilters(services);

            SwaggerDocumentation(services);
        }





        private void Injections(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            var appConfiguration = new AppConfiguration(Configuration);
            services.AddSingleton(appConfiguration);


            AppConfigurationInjection.LoadInjections(services, Configuration);
            CommonInjection.LoadInjections(services, appConfiguration);
            BackgroundInjection.LoadInjections(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            InitializeSerilog(serviceProvider);

            InitializeElasticSearchIndexes(serviceProvider);


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

        private static void InitializeElasticSearchIndexes(IServiceProvider serviceProvider)
        {
            var elasticClient = serviceProvider.GetService<ElasticClient>();

            var searchAsYouType = ElasticSearchConstants.SearchAsYouTypeField;
            if (!elasticClient.Indices.Exists(ElasticSearchConstants.CustomerIndex).Exists)
            {
                var createIndexResponse = elasticClient.Indices.Create(ElasticSearchConstants.CustomerIndex, c => c
       .Map<CustomerModel>(mm => mm
       .AutoMap<CustomerModel>()

       .Properties(p => p
       .Text(t => t.Name(n => n.FirstName)
       .Fields(ff => ff.SearchAsYouType(v => v.Name(searchAsYouType)))))

       .Properties(p => p
       .Text(t => t.Name(n => n.LastName)
       .Fields(ff => ff.SearchAsYouType(v => v.Name(searchAsYouType)))))

       .Properties(p => p
       .Text(t => t.Name(n => n.FullName)
       .Fields(ff => ff.SearchAsYouType(v => v.Name(searchAsYouType)))))

       ));

            }

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

                //config.OperationProcessors.Add(new OperationSecurityScopeProcessor("apiKey"));
                //config.DocumentProcessors.Add(new SecurityDefinitionAppender("apiKey", new OpenApiSecurityScheme()
                //{
                //    Type = OpenApiSecuritySchemeType.ApiKey,
                //    Name = "Authorization",
                //    In = OpenApiSecurityApiKeyLocation.Header,
                //    Description = "Bearer token"
                //}));
            });
        }

    }
}
