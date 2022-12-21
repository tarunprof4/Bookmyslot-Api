using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business
{
    public interface INotificationBusiness
    {
        Task<Result<bool>> SendCustomerRegisteredNotification(CustomerModel customerModel);

        Task<Result<bool>> SlotScheduledNotificatiion(SlotModel slotModel, CustomerModel createdBy, CustomerModel bookedBy);

        Task<Result<bool>> SlotCancelledNotificatiion(SlotModel slotModel, CustomerModel cancelledBy, CustomerModel notCancelledBy);

        Task<Result<bool>> SlotMeetingInformationNotification(SlotModel slotModel, CustomerModel resendTo);
    }
}
