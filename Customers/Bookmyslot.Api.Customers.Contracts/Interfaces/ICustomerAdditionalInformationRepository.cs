using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerAdditionalInformationRepository
    {
        Task<Response<CustomerAdditionalInformationModel>> GetCustomerAdditionalInformation(string customerId);
        Task<Response<bool>> UpdateCustomerAdditionalInformation(string customerId, CustomerAdditionalInformationModel customerAdditionalInformationModel);
    }
}
