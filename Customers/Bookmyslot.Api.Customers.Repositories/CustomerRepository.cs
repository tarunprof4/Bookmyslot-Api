using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection connection;

        public CustomerRepository()
        {
            this.connection = new SqlConnection("Data Source=.;Initial Catalog=Bookmyslot;Integrated Security=True");
        }
        public Response<bool> CreateCustomer(CustomerModel customerModel)
        {
            var customerEntity = EntityFactory.EntityFactory.CreateCustomerEntity(customerModel);
            this.connection.Insert<string, CustomerEntity>(customerEntity);
            return new Response<bool>() { Result = true };
        }

        public Response<bool> DeleteCustomer(string email)
        {
            this.connection.Delete<CustomerEntity>(email);
            return new Response<bool>() { Result = true };
        }

        public Response<IEnumerable<CustomerModel>> GetAllCustomers()
        {
            var customerEntities = this.connection.GetList<CustomerEntity>();
            var customerModels = ModelFactory.ModelFactory.CreateCustomerModels(customerEntities);
            return new Response<IEnumerable<CustomerModel>>() { Result = customerModels };
        }

        public Response<CustomerModel> GetCustomer(string email)
        {
            var customerEntity = this.connection.Get<CustomerEntity>(email);
            var customerModel = ModelFactory.ModelFactory.CreateCustomerModel(customerEntity);
            return new Response<CustomerModel>() { Result = customerModel };
        }

        public Response<bool> UpdateCustomer(CustomerModel customerModel)
        {
            var customerEntity = EntityFactory.EntityFactory.UpdateCustomerEntity(customerModel);
            this.connection.Update<CustomerEntity>(customerEntity);
            return new Response<bool>() { Result = true };
        }
    }
}
