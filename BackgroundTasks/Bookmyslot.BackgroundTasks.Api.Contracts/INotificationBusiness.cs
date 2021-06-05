using Bookmyslot.Api.Common.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Contracts
{
    public interface INotificationBusiness
    {
        Response<bool> SendNotification();
    }
}
