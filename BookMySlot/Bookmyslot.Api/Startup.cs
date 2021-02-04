using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Bookmyslot.Api.Common.ExceptionHandlers;
using Bookmyslot.Api.Common.Injections;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Customers.Injections;
using Bookmyslot.Api.SlotScheduler.Injections;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            DependencyInjections(services);

            services.AddControllers();

            InitializeSwagger(services);

            InitializeModelInValidState(services);


            //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))


            //             .AddGoogle(googleOptions =>
            //                         {
            //                             googleOptions.ClientId = "952200248622-4dhfmtdcf4u1b3ektt6giacpotc60vkl.apps.googleusercontent.com";
            //                             googleOptions.ClientSecret = "b0jpd7fNb6D5MgLi21x3atTn";

            //                         })
            //.AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = "2817970748513714";
            //    facebookOptions.AppSecret = "12a2014e0c481e0b0d0b428a506a6fb1";

            //})
            //;

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //    {
            //        options.Authority = "";
            //        options.Audience = "";

            //    });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
            //    AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options)).
            //    AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options));




            //services.AddMvc(options => {
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});








            var key = Encoding.ASCII.GetBytes("b0jpd7fNb6D5MgLi21x3atTn");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = "accounts.google.com",

                ValidateAudience = false,
                ValidAudience = "952200248622-8cn9oq0n1fnp0rjga6vsb9oh67kkkt8s.apps.googleusercontent.com",

                ValidateIssuerSigningKey = false,
                //IssuerSigningKey = new SymmetricSecurityKey(key),

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = "accounts.google.com";
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            //services.AddAuthentication(options =>
            //{
            //    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = GoogleDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //}) .AddGoogle(googleOptions =>
            //{


            //    googleOptions.ClientId = "952200248622-4dhfmtdcf4u1b3ektt6giacpotc60vkl.apps.googleusercontent.com";
            //                                 googleOptions.ClientSecret = "b0jpd7fNb6D5MgLi21x3atTn";
            //});


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

        private static void InitializeModelInValidState(IServiceCollection services)
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

        private void DependencyInjections(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            Dictionary<string, string> appConfigurations = GetAppConfigurations();

            CommonInjection.CommonInjections(services);

            CustomerInjection.CustomerBusinessInjections(services);
            CustomerInjection.CustomerRepositoryInjections(services, appConfigurations);

            SlotSchedulerInjection.SlotSchedulerCommonInjections(services);
            SlotSchedulerInjection.SlotSchedulerBusinessInjections(services);
            SlotSchedulerInjection.SlotSchedulerRepositoryInjections(services, appConfigurations);
        }

        private static void InitializeSwagger(IServiceCollection services)
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
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "",
                        //Url = "https://example.com/license"
                    };
                    //document.OperationProcessors.Add(new OperationSecurityScopeProcessor("apiKey"));
                    //document.DocumentProcessors.Add(new SecurityDefinitionAppender("apiKey", new NSwag.SwaggerSecurityScheme()
                    //{
                    //    Type = NSwag.SwaggerSecuritySchemeType.ApiKey,
                    //    Name = "Authorization",
                    //    In = NSwag.SwaggerSecurityApiKeyLocation.Header,
                    //    Description = "Bearer token"
                    //}));

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

        private Dictionary<string, string> GetAppConfigurations()
        {
            Dictionary<string, string> appConfigurations = new Dictionary<string, string>();
            var bookMySlotConnectionString = Configuration.GetConnectionString(AppConfigurationConstants.BookMySlotDatabase);
            appConfigurations.Add(AppConfigurationConstants.BookMySlotDatabaseConnectionString, bookMySlotConnectionString);

            return appConfigurations;
        }




    }
}
