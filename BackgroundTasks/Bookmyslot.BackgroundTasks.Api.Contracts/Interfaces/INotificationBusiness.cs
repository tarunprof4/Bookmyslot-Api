using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces
{
    public interface INotificationBusiness
    {
        Task<Response<bool>> SendCustomerRegisterNotification(CustomerModel customerModel);
    }
}
