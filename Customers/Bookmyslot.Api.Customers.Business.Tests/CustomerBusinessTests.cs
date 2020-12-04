using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    public class CustomerBusinessTests
    {
        private CustomerBusiness customerBusiness;
        private Mock<ICustomerRepository> customerRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            customerRepositoryMock = new Mock<ICustomerRepository>();
            customerBusiness = new CustomerBusiness(customerRepositoryMock.Object);
        }

        [Test]
        public void CustomerBusiness_GetCustomerByEmail_CallsCustomerRepository()
        {
            customerBusiness.GetCustomer("");

            customerRepositoryMock.Verify((m => m.GetCustomer("")), Times.Once());
        }
    }
}