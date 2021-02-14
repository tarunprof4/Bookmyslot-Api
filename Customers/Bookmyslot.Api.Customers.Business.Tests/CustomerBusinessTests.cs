using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class CustomerBusinessTests
    {
        private const string CUSTOMERID = "CUSTOMERID";
        private const string EMAIL = "a@gmail.com";
        private CustomerBusiness customerBusiness;
        private Mock<ICustomerRepository> customerRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            customerRepositoryMock = new Mock<ICustomerRepository>();
            customerBusiness = new CustomerBusiness(customerRepositoryMock.Object);
        }

        [Test]
        public async Task GetCustomerByEmail_ValidCustomerEmail_CallsGetCustomerByEmailRepository()
        {
            var customer = await customerBusiness.GetCustomerByEmail(EMAIL);

            customerRepositoryMock.Verify((m => m.GetCustomerByEmail(EMAIL)), Times.Once());
        }


        [TestCase("")]
        [TestCase("   ")]
        public async Task GetCustomerByEmail_InvalidEmailId_ReturnsValidationErrorResponse(string email)
        {
            var customer = await customerBusiness.GetCustomerByEmail(email);

            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
            Assert.AreEqual(customer.Messages.First(), AppBusinessMessagesConstants.EmailIdNotValid);
        }

        [Test]
        public async Task GetCustomerByCustomerId_ValidCustomerId_CallsGetCustomerByCustomerIdRepository()
        {
            var customer = await customerBusiness.GetCustomerById(CUSTOMERID);

            customerRepositoryMock.Verify((m => m.GetCustomerById(CUSTOMERID)), Times.Once());
        }


        [TestCase("")]
        [TestCase("   ")]
        public async Task GetCustomerByCustomerId_InvalidCustomerId_ReturnsValidationErrorResponse(string customerId)
        {
            var customer = await customerBusiness.GetCustomerById(customerId);

            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
            Assert.AreEqual(customer.Messages.First(), AppBusinessMessagesConstants.CustomerIdNotValid);
        }

    }
}