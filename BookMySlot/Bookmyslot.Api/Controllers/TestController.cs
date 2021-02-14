using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public TestController(IKeyEncryptor keyEncryptor, IResendSlotInformationBusiness resendSlotInformationBusiness)
        {
            this.keyEncryptor = keyEncryptor;
            this.resendSlotInformationBusiness = resendSlotInformationBusiness;
        }

        [HttpGet()]
        [Route("api/v1/test/TestingAsync")]
        public async Task<IActionResult> TestingAsync()
        {
            var slotModel = GetDefaultSlotModel();

            //this.appLogContext.SetSlotModelInfoToContext(slotModel);
            //this.loggerService.LogError("Slot Model Logged");
            //this.loggerService.LogInfo("Slot Model Logged");
            //this.loggerService.LogDebug("Slot Model Logged");
            //this.loggerService.LogVerbose("Slot Model Logged");
            //this.loggerService.LogFatal("Slot Model Logged");
            //this.loggerService.LogWarning("Slot Model Logged");

            ResendSlotInformation resendSlotInformation = new ResendSlotInformation();
            var resendSlotInformationResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(null, resendSlotInformation.ResendTo);
            return this.CreatePostHttpResponse(resendSlotInformationResponse);
        }

        [HttpGet()]
        [Route("api/v1/test/Testing")]
        public async Task<IActionResult> Testing()
        {
            CustomerModel customer = new CustomerModel() { FirstName = "Fir", LastName = "Las" };
            return this.Ok(await Task.FromResult(customer));
        }

        private SlotModel GetDefaultSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = Guid.NewGuid();
            slotModel.Title = "Title";
            slotModel.CreatedBy = "CreatedBy";
            slotModel.BookedBy = "BookedBy";
            slotModel.TimeZone = "TimeZone";
            slotModel.SlotDate = "2012-2-2";
            slotModel.SlotDateUtc = DateTime.UtcNow;
            slotModel.SlotStartTime = new TimeSpan(10, 10, 10);
            slotModel.SlotEndTime = new TimeSpan(11, 11, 11);

            return slotModel;
        }
    }



}
