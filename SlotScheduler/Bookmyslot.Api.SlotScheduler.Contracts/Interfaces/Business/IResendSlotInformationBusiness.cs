using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface IResendSlotInformationBusiness
    {
        Task<Result<bool>> ResendSlotMeetingInformation(SlotModel slotModel, string resendTo);
    }
}
