
using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Logging.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
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
        private readonly ILoggerService loggerService;
        private readonly IAppLogContext appLogContext;
        private readonly IKeyEncryptor keyEncryptor;
        private readonly IResendSlotInformationBusiness resendSlotInformationBusiness;
        public TestController(IKeyEncryptor keyEncryptor, IResendSlotInformationBusiness resendSlotInformationBusiness, ILoggerService loggerService, IAppLogContext appLogContext)
        {
            this.keyEncryptor = keyEncryptor;
            this.resendSlotInformationBusiness = resendSlotInformationBusiness;
            this.loggerService = loggerService;
            this.appLogContext = appLogContext;
        }

        [HttpGet()]
        [Route("api/v1/test/Testing")]
        public async Task<IActionResult> Testing()
        {
            var slotModel = GetDefaultSlotModel();

            this.appLogContext.SetSlotModelInfoToContext(slotModel);
            this.loggerService.LogDebug("Slot Model Logged");

            ResendSlotInformation resendSlotInformation = new ResendSlotInformation();
            var resendSlotInformationResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(null, resendSlotInformation.ResendTo);
            return this.CreatePostHttpResponse(resendSlotInformationResponse);
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
