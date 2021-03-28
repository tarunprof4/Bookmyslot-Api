﻿using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CustomerViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

      
        public static CustomerViewModel CreateCustomerViewModel(CustomerModel customerModel)
        {
            return new CustomerViewModel()
            {
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                BioHeadLine = customerModel.BioHeadLine
            };
        }

        public static IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerModel> customerModels)
        {
            List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();
            foreach(var customerModel in customerModels)
            {
                customerViewModels.Add(CreateCustomerViewModel(customerModel));
            }

            return customerViewModels;
        }


        public static IEnumerable<CustomerViewModel> CreateCustomerViewModels(IEnumerable<CustomerSlotModel> customerSlotModels)
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
