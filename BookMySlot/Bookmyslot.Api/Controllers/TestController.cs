using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Facebook.Configuration;
using Bookmyslot.Api.Authentication.Google.Configuration;
using Bookmyslot.Api.Cache.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Email.Configuration;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Domain.DomainEvents;
using Bookmyslot.Api.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly AppConfiguration appConfiguration;
        private readonly AuthenticationConfiguration authenticationConfiguration;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly EmailConfiguration emailConfiguration;
        private readonly GoogleAuthenticationConfiguration googleAuthenticationConfiguration;
        private readonly FacebookAuthenticationConfiguration facebookAuthenticationConfiguration;

        private readonly ILoggerService loggerService;
        private readonly IMediator mediator;





        public TestController(AppConfiguration appConfiguration, AuthenticationConfiguration authenticationConfiguration,
            CacheConfiguration cacheConfiguration, EmailConfiguration emailConfiguration, GoogleAuthenticationConfiguration googleAuthenticationConfiguration,
            FacebookAuthenticationConfiguration facebookAuthenticationConfiguration, ILoggerService loggerService, IMediator mediator,
            IRegisterCustomerBusiness registerCustomerBusiness)
        {
            this.appConfiguration = appConfiguration;
            this.authenticationConfiguration = authenticationConfiguration;
            this.cacheConfiguration = cacheConfiguration;
            this.emailConfiguration = emailConfiguration;
            this.googleAuthenticationConfiguration = googleAuthenticationConfiguration;
            this.facebookAuthenticationConfiguration = facebookAuthenticationConfiguration;
            this.loggerService = loggerService;
            this.mediator = mediator;
        }



        [HttpGet()]
        [Route("api/v1/test/Testing")]
        public async Task<IActionResult> Testing()
        {
            decimal price = 2;
            var aa = price.ToString("C");
            var currentCulture = CultureInfo.CurrentCulture;
            var currentUICulture = CultureInfo.CurrentUICulture;
            var defaultCurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
            var defaultCurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;

            var registerCustomer = CreateRegisterCustomerModel();
            //registerCustomer.RegisterCustomer();

            BaseDomainEvent baseDomainEvent = new CustomerRegisteredDomainEvent(registerCustomer);

            this.loggerService.Debug("Mediator1 started");
            await this.mediator.Publish(baseDomainEvent);
            this.loggerService.Debug("Mediator1 ended");

            var configurations = new Dictionary<string, object>();
            configurations.Add("appConfiguration", this.appConfiguration);
            configurations.Add("authenticationConfiguration", this.authenticationConfiguration);
            configurations.Add("cacheConfiguration", this.cacheConfiguration);
            configurations.Add("emailConfiguration", this.emailConfiguration);
            configurations.Add("googleAuthenticationConfiguration", this.googleAuthenticationConfiguration);
            configurations.Add("facebookAuthenticationConfiguration", this.facebookAuthenticationConfiguration);

            configurations.Add("currentDate", DateTime.UtcNow);


            return this.Ok(await Task.FromResult(configurations));
        }


        private RegisterCustomerModel CreateRegisterCustomerModel()
        {
            return new RegisterCustomerModel()
            {
                FirstName = "Fir",
                LastName = "Fir",
                UserName = "Fir",
                Email = "a@gmail.com",
                Provider = "GOOGLE"
            };
        }


    }



}
