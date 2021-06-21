using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business
{
    public interface INotificationBusiness
    {
        Task<Response<bool>> SendCustomerRegisteredNotification(CustomerModel customerModel);

        Task<Response<bool>> SlotScheduledNotificatiion(SlotModel slotModel, CustomerModel createdBy, CustomerModel bookedBy);

        Task<Response<bool>> SlotCancelledNotificatiion(SlotModel slotModel, CustomerModel cancelledBy, CustomerModel notCancelledBy);

        Task<Response<bool>> SlotMeetingInformationNotification(SlotModel slotModel, CustomerModel resendTo);
    }
}
