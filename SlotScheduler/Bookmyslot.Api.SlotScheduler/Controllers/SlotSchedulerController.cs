using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class SlotSchedulerController : BaseApiController
    {
        private readonly ISlotSchedulerBusiness slotSchedulerBusiness;
        private readonly IKeyEncryptor keyEncryptor;
        public SlotSchedulerController(ISlotSchedulerBusiness slotSchedulerBusiness, IKeyEncryptor keyEncryptor)
        {
            this.slotSchedulerBusiness = slotSchedulerBusiness;
            this.keyEncryptor = keyEncryptor;
        }


        [Route("api/v1/SlotScheduler")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SlotScheduleModel slotScheduleModel)
        {
            Log.Information("Get all available slots for the customer");
            var customerSlotModel = JsonConvert.DeserializeObject<BmsKeyValuePair<SlotModel, string>>(this.keyEncryptor.Decrypt(slotScheduleModel.SlotModelKey));

            if (customerSlotModel != null)
            {
                var slotScheduleResponse = await this.slotSchedulerBusiness.ScheduleSlot(customerSlotModel.Key, slotScheduleModel.BookedBy);
                return this.CreatePostHttpResponse(slotScheduleResponse);
            }

            var validationErrorResponse = Response<List<CustomerSlotModel>>.ValidationError(new List<string>() { AppBusinessMessages.CorruptData });
            return this.CreatePostHttpResponse(validationErrorResponse);
        }
    }
}
