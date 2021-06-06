using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts
{
    public interface INotificationBusiness
    {
        Task<Response<bool>> SendCustomerRegisterNotification(CustomerModel registerCustomerModel);
    }
}
