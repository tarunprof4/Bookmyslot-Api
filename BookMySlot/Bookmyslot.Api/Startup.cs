using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Common.Web.ExceptionHandlers;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.Injections;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

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

            Dictionary<string, string> appConfigurations = GetAppConfigurations();

            Injections(services, appConfigurations);

            InitializeJwtAuthentication(services);

            RegisterFilters(services);

            services.AddControllers();

            SwaggerDocumentation(services);

            BadRequestConfiguration(services);
        }

        private static void InitializeJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                //ValidIssuer = JwtTokenConstants.Issuer,
                //ValidAudience = JwtTokenConstants.Audience,
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtTokenConstants.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
        }

        private static void RegisterFilters(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<LoggingFilter>();
            });
        }

        private static void Injections(IServiceCollection services, Dictionary<string, string> appConfigurations)
        {
            services.AddHttpContextAccessor();

            AppInjection.LoadInjections(services);
            AuthenticationInjection.LoadInjections(services);
            CacheInjection.LoadInjections(services, appConfigurations);
            DataBaseInjection.LoadInjections(services, appConfigurations);
            CommonInjection.LoadInjections(services);
            CustomerInjection.LoadInjections(services);
            SearchInjection.LoadInjections(services);
            SlotSchedulerInjection.LoadInjections(services);
        }

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


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Dictionary<string, string> GetAppConfigurations()
        {
            Dictionary<string, string> appConfigurations = new Dictionary<string, string>();
            var bookMySlotConnectionString = Configuration.GetConnectionString(AppSettingKeysConstants.BookMySlotDatabase);
            var cacheConnectionString = Configuration.GetConnectionString(AppSettingKeysConstants.CacheDatabase);
            appConfigurations.Add(AppSettingKeysConstants.BookMySlotDatabase, bookMySlotConnectionString);
            appConfigurations.Add(AppSettingKeysConstants.CacheDatabase, cacheConnectionString);

            return appConfigurations;
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

        private static void BadRequestConfiguration(IServiceCollection services)
        {
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
