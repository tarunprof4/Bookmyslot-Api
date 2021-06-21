﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public async Task<Response<bool>> CreateCustomer(CustomerModel customerModel)
        {
            return await this.customerRepository.CreateCustomer(customerModel);
        }

        public Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            throw new NotImplementedException();
        }
    }
}
