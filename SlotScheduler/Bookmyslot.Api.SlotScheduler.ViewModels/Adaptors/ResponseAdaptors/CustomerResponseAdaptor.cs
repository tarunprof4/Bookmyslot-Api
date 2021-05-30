using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors
{
    public class CustomerResponseAdaptor : ICustomerResponseAdaptor
    {
        private readonly ISymmetryEncryption symmetryEncryption;

        public CustomerResponseAdaptor(ISymmetryEncryption symmetryEncryption)
        {
            this.symmetryEncryption = symmetryEncryption;
        }


        public CustomerViewModel CreateCustomerViewModel(CustomerModel customerModel)
        {
            return new CustomerViewModel()
            {
                Id = this.symmetryEncryption.Encrypt(customerModel.Id),
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                BioHeadLine = customerModel.BioHeadLine,
                ProfilePictureUrl = customerModel.ProfilePictureUrl
            };
        }

        public IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerModel> customerModels)
        {
            List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();
            foreach (var customerModel in customerModels)
            {
                customerViewModels.Add(CreateCustomerViewModel(customerModel));
            }

            return customerViewModels;
        }


        public IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerSlotModel> customerSlotModels)
        {
            List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();
            foreach (var customerSlotModel in customerSlotModels)
            {
                customerViewModels.Add(CreateCustomerViewModel(customerSlotModel.CustomerModel));
            }

            return customerViewModels;
        }
    }
}
