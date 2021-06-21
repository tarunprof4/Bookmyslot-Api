using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business
{
    public interface INotificationBusiness
    {
        Task<Response<bool>> SendCustomerRegisteredNotification(SearchCustomerModel customerModel);

        Task<Response<bool>> SlotScheduledNotificatiion(SlotModel slotModel, SearchCustomerModel createdBy, SearchCustomerModel bookedBy);

        Task<Response<bool>> SlotCancelledNotificatiion(SlotModel slotModel, SearchCustomerModel cancelledBy, SearchCustomerModel notCancelledBy);

        Task<Response<bool>> SlotMeetingInformationNotification(SlotModel slotModel, SearchCustomerModel resendTo);
    }
}
