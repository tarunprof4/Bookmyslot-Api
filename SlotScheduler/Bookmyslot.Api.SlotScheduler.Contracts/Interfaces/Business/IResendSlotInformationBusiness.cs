using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface IResendSlotInformationBusiness
    {
        Task<Response<bool>> ResendSlotInformation(SlotModel slotModel, string resendTo);
    }
}
