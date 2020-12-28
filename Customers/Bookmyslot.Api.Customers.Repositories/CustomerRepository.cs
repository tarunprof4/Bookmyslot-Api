﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection connection;

        public CustomerRepository(IDbConnection connection)
        {
            this.connection = connection;
        }
        public async Task<Response<string>> CreateCustomer(CustomerModel customerModel)
        {
            var customerEntity = EntityFactory.EntityFactory.CreateCustomerEntity(customerModel);
            await this.connection.InsertAsync<string, CustomerEntity>(customerEntity);
            return new Response<string>() { Result = customerModel.Email };
        }

        public async Task<Response<bool>> DeleteCustomer(string email)
        {
            await this.connection.DeleteAsync<CustomerEntity>(email.ToLowerInvariant());
            return new Response<bool>() { Result = true };
        }

        public async Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            var customerEntities = await this.connection.GetListAsync<CustomerEntity>();
            var customerModels = ModelFactory.ModelFactory.CreateCustomerModels(customerEntities);
            if (customerModels.Count == 0)
            {
                return Response<IEnumerable<CustomerModel>>.Empty(AppBusinessMessages.NoRecordsFound);
            }

            return new Response<IEnumerable<CustomerModel>>() { Result = customerModels };
        }

        public async Task<Response<CustomerModel>> GetCustomer(string email)
        {
            var customerEntity = await this.connection.GetAsync<CustomerEntity>(email);
            if (customerEntity == null)
            {
                return Response<CustomerModel>.Empty(AppBusinessMessages.CustomerNotFound);
            }

            var customerModel = ModelFactory.ModelFactory.CreateCustomerModel(customerEntity);
            return Response<CustomerModel>.Success(customerModel);
        }

        public async Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            var customerEntity = EntityFactory.EntityFactory.UpdateCustomerEntity(customerModel);
            await this.connection.UpdateAsync<CustomerEntity>(customerEntity);
            return new Response<bool>() { Result = true };
        }
    }
}
