using Moq;
using NUnit.Framework;
using System.Data;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class Tests
    {
        private CustomerRepository customerRepository;
        private Mock<IDbConnection> dbConnectionMock;

        [SetUp]
        public void SetUp()
        {
            //customerRepositoryMock = new Mock<ICustomerRepository>();
            //customerBusiness = new CustomerBusiness(customerRepositoryMock.Object);

            dbConnectionMock = new Mock<IDbConnection>();
            customerRepository = new CustomerRepository();
        }
    }
}