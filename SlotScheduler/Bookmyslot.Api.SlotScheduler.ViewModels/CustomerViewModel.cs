using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CustomerViewModel
    {
        public string Id{ get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public string ProfilePictureUrl { get; set; }

       
        public static CustomerViewModel CreateCustomerViewModel(CustomerModel customerModel, Func<string, string> encryptionFunction)
        {
            
            return new CustomerViewModel()
            {
                Id = encryptionFunction.Invoke(customerModel.Id),
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                BioHeadLine = customerModel.BioHeadLine,
                ProfilePictureUrl = customerModel.ProfilePictureUrl
            };
        }

        public static IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerModel> customerModels, Func<string, string> encryptionFunction)
        {
            List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();
            foreach(var customerModel in customerModels)
            {
                customerViewModels.Add(CreateCustomerViewModel(customerModel, encryptionFunction));
            }

            return customerViewModels;
        }


        public static IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerSlotModel> customerSlotModels, Func<string, string> encryptionFunction)
        {
            List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();
            foreach (var customerSlotModel in customerSlotModels)
            {
                customerViewModels.Add(CreateCustomerViewModel(customerSlotModel.CustomerModel, encryptionFunction));
            }

            return customerViewModels;
        }
    }
}
