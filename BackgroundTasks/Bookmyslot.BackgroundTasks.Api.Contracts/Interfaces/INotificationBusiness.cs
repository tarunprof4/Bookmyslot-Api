using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces
{
    public interface INotificationBusiness
    {
        Task<Response<bool>> SendCustomerRegisteredNotification(CustomerModel customerModel);

        Task<Response<bool>> SlotScheduledNotificatiion(SlotModel slotModel, CustomerModel createdBy, CustomerModel bookedBy);

        Task<Response<bool>> SlotCancelledNotificatiion(SlotModel slotModel, CustomerModel cancelledBy);
    }
}
