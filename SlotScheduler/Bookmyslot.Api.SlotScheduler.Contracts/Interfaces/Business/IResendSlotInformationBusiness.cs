using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface IResendSlotInformationBusiness
    {
        Task<Response<bool>> ResendSlotMeetingInformation(SlotModel slotModel, string resendTo);
    }
}
