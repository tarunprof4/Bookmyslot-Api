using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Facebook.Configuration;
using Bookmyslot.Api.Authentication.Google.Configuration;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly IKeyEncryptor keyEncryptor;

        private readonly IResendSlotInformationBusiness resendSlotInformationBusiness;
        private readonly AppConfiguration appConfiguration;
        private readonly AuthenticationConfiguration authenticationConfiguration;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly EmailConfiguration emailConfiguration;
        private readonly GoogleAuthenticationConfiguration googleAuthenticationConfiguration;
        private readonly FacebookAuthenticationConfiguration facebookAuthenticationConfiguration;



        public TestController(AppConfiguration appConfiguration, AuthenticationConfiguration authenticationConfiguration,
            CacheConfiguration cacheConfiguration, EmailConfiguration emailConfiguration, GoogleAuthenticationConfiguration googleAuthenticationConfiguration,
            FacebookAuthenticationConfiguration facebookAuthenticationConfiguration)
        {
            this.appConfiguration = appConfiguration;
            this.authenticationConfiguration = authenticationConfiguration;
            this.cacheConfiguration = cacheConfiguration;
            this.emailConfiguration = emailConfiguration;
            this.googleAuthenticationConfiguration = googleAuthenticationConfiguration;
            this.facebookAuthenticationConfiguration = facebookAuthenticationConfiguration;
        }

        //[HttpGet()]
        //[Route("api/v1/test/TestingAsync")]
        //public async Task<IActionResult> TestingAsync()
        //{
        //    var slotModel = GetDefaultSlotModel();

        //    //this.appLogContext.SetSlotModelInfoToContext(slotModel);
        //    //this.loggerService.LogError("Slot Model Logged");
        //    //this.loggerService.LogInfo("Slot Model Logged");
        //    //this.loggerService.LogDebug("Slot Model Logged");
        //    //this.loggerService.LogVerbose("Slot Model Logged");
        //    //this.loggerService.LogFatal("Slot Model Logged");
        //    //this.loggerService.LogWarning("Slot Model Logged");

        //    ResendSlotInformation resendSlotInformation = new ResendSlotInformation();
        //    var resendSlotInformationResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(null, resendSlotInformation.ResendTo);
        //    return this.CreatePostHttpResponse(resendSlotInformationResponse);
        //}

        [HttpGet()]
        [Route("api/v1/test/Testing")]
        public async Task<IActionResult> Testing()
        {
            
            var configurations = new Dictionary<string, object>();
            configurations.Add("appConfiguration", this.appConfiguration);
            configurations.Add("authenticationConfiguration", this.authenticationConfiguration);
            configurations.Add("cacheConfiguration", this.cacheConfiguration);
            configurations.Add("emailConfiguration", this.emailConfiguration);
            configurations.Add("googleAuthenticationConfiguration", this.googleAuthenticationConfiguration);
            configurations.Add("facebookAuthenticationConfiguration", this.facebookAuthenticationConfiguration);

            configurations.Add("currentDate", DateTime.UtcNow);

            //configurations.Add("CurrentCulture", Thread.CurrentThread.CurrentCulture);
            //configurations.Add("CurrentUICulture", Thread.CurrentThread.CurrentUICulture);

            //var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            //foreach (var ci in allCultures)
            //{
            //    // Display the name of each culture.
            //    Console.Write($"{ci.EnglishName} ({ci.Name}): ");
            //    // Indicate the culture type.
            //    if (ci.CultureTypes.HasFlag(CultureTypes.NeutralCultures))
            //        Console.Write(" NeutralCulture");
            //    if (ci.CultureTypes.HasFlag(CultureTypes.SpecificCultures))
            //        Console.Write(" SpecificCulture");
            //    Console.WriteLine();
            //}

            return this.Ok(await Task.FromResult(configurations));
        }

     
    }



}
