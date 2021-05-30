using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface ICustomerResponseAdaptor 
    {
        CustomerViewModel CreateCustomerViewModel(CustomerModel customerModel);
        IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerModel> customerModels);
        IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerSlotModel> customerSlotModels);
    }
}
