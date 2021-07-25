using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Common.Web.ExceptionHandlers;
using Bookmyslot.Api.Customers.Business.DomainEventHandlers;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Bookmyslot.Api.Injections;
using Bookmyslot.Api.NodaTime.Contracts.Constants;
using Bookmyslot.Api.NodaTime.Interfaces;
using Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers;
using Bookmyslot.Api.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Text;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System;
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
            services.AddMediatR(typeof(CustomerRegisteredDomainEventHandler).Assembly,
                typeof(SlotCancelledDomainEventHandler).Assembly,
                typeof(SlotMeetingInformationRequestedDomainEventHandler).Assembly,
                typeof(SlotMeetingInformationRequestedDomainEventHandler).Assembly);


            var appConfiguration = new AppConfiguration(Configuration);
            services.AddSingleton(appConfiguration);
            var authenticationConfiguration = new AuthenticationConfiguration(Configuration);
            services.AddSingleton(authenticationConfiguration);

            Injections(services, appConfiguration);

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            InitializeJwtAuthentication(services, authenticationConfiguration);

            RegisterFilters(services);

            ConfigureNodaTimeDatePattern(services);

            SwaggerDocumentation(services);

            BadRequestConfiguration(services);
        }

        private static void ConfigureNodaTimeDatePattern(IServiceCollection services)
        {
            var pattern = ZonedDateTimePattern.CreateWithInvariantCulture(NodaTimeConstants.ApplicationZonedDateTimePattern, DateTimeZoneProviders.Tzdb);
            var settings = new JsonSerializerSettings
            {
                Converters = { new NodaPatternConverter<ZonedDateTime>(pattern) }
            };
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.DateParseHandling = DateParseHandling.None;
                s.SerializerSettings.Converters = settings.Converters;
            });
        }

        private static void InitializeJwtAuthentication(IServiceCollection services, AuthenticationConfiguration authenticationConfiguration)
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
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationConfiguration.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
        }

        private static void RegisterFilters(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //options.Filters.Add<LoggingFilter>();
            });
        }

        private void Injections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            services.AddHttpContextAccessor();

            WebInjections.LoadInjections(services);
            AppConfigurationInjection.LoadInjections(services, Configuration);
            AuthenticationInjection.LoadInjections(services);
            CacheInjection.LoadInjections(services, appConfiguration);
            DataBaseInjection.LoadInjections(services, appConfiguration);
            CommonInjection.LoadInjections(services);
            CustomerInjection.LoadInjections(services);
            SearchInjection.LoadInjections(services, appConfiguration);
            SlotSchedulerInjection.LoadInjections(services);
            HttpFactoryInjections.LoadInjections(services);
            NodaTimeInjection.LoadInjections(services);
            FileInjection.LoadInjections(services);
            AzureInjection.LoadInjections(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            InitializeSerilog(serviceProvider);

            BootStrapApp(serviceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestResponseLogging();

            app.UseHttpsRedirection();


            var loggerService = serviceProvider.GetService<ILoggerService>();
            app.ConfigureGlobalExceptionHandler(loggerService);

            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseAuthentication();
            app.UseAuthorization();

            SetRequestCulture(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SetRequestCulture(IApplicationBuilder app)
        {
            var supportedCultures = new[] { CultureConstants.IndiaCulture };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
            app.UseRequestLocalization(localizationOptions);
        }

        private static void BootStrapApp(IServiceProvider serviceProvider)
        {
            var nodaTimeZoneLocationBusiness = serviceProvider.GetService<INodaTimeZoneLocationBusiness>();
            nodaTimeZoneLocationBusiness.CreateNodaTimeZoneLocationInformation();

            var fileConfigurationBusiness = serviceProvider.GetService<IFileConfigurationBusiness>();
            fileConfigurationBusiness.CreateImageConfigurationInformation();
        }


        private static void InitializeSerilog(IServiceProvider serviceProvider)
        {

            var defaultLogEnricher = serviceProvider.GetService<DefaultLogEnricher>();
            var appConfiguration = serviceProvider.GetService<AppConfiguration>();




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
            loggerService.Debug("Starting Bookmyslot web host");
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
