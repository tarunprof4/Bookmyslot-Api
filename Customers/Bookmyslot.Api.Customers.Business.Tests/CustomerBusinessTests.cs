using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class CustomerBusinessTests
    {
        private const string EMAIL = "a@gmail.com";
        private const string GENDERPREFIX = "genderprefix";
        private const string FIRSTNAME = "fisrtname";
        private const string MIDDLENAME = "middlename";
        private const string LASTNAME = "lastname";
        private const string GENDER = "gender";
        private CustomerBusiness customerBusiness;
        private Mock<ICustomerRepository> customerRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            customerRepositoryMock = new Mock<ICustomerRepository>();
            customerBusiness = new CustomerBusiness(customerRepositoryMock.Object);
        }

        [Test]
        public async Task CustomerBusiness_GetCustomerByEmail_CallsGetCustomerByEmailRepository()
        {
            var customer = await customerBusiness.GetCustomer(EMAIL);

            customerRepositoryMock.Verify((m => m.GetCustomer(EMAIL)), Times.Once());
        }


        [TestCase("")]
        [TestCase("   ")]
        public async Task CustomerBusiness_InvalidEmailId_ReturnsValidationErrorResponse(string email)
        {
            var customer = await customerBusiness.GetCustomer(email);

            Assert.AreEqual(customer.HasResult, false);
            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
            Assert.AreEqual(customer.Messages.First(), Constants.EmailIdNotValid);
        }

        [Test]
        public async Task CustomerBusiness_GetAllCustomers_CallsGetAllCustomersRepository()
        {
            var customer = await customerBusiness.GetAllCustomers();

            customerRepositoryMock.Verify((m => m.GetAllCustomers()), Times.Once());
        }


        [Test]
        public async Task CustomerBusiness_CreateCustomers_ReturnsSuccess()
        {
            var customerModel = CreateCustomer();

            var customer = await customerBusiness.CreateCustomer(customerModel);

            customerRepositoryMock.Verify((m => m.CreateCustomer(customerModel)), Times.Once());
        }

        [Test]
        public async Task CustomerBusiness_CreateInvalidCustomer_ReturnsValidationError()
        {
            var customerModel = new CustomerModel();

            var customer = await customerBusiness.CreateCustomer(customerModel);

            Assert.IsTrue(customer.Messages.Contains(Constants.GenderPrefixInValid));
            Assert.IsTrue(customer.Messages.Contains(Constants.FirstNameInValid));
            Assert.IsTrue(customer.Messages.Contains(Constants.LastNameInValid));
            Assert.IsTrue(customer.Messages.Contains(Constants.GenderNotValid));
            Assert.IsTrue(customer.Messages.Contains(Constants.EmailIdNotValid));
            Assert.AreEqual(customer.ResultType,ResultType.ValidationError);
        }

        [Test]
        public async Task CustomerBusiness_CreateCustomerWithInvalidCustomerNameAndEmail_ReturnsValidationError()
        {
            var customerModel = new CustomerModel() { FirstName="12", MiddleName = "2@", Email ="asdf.com" };

            var customer = await customerBusiness.CreateCustomer(customerModel);

            Assert.IsTrue(customer.Messages.Contains(Constants.FirstNameInValid));
            Assert.IsTrue(customer.Messages.Contains(Constants.MiddleNameInValid));
            Assert.IsTrue(customer.Messages.Contains(Constants.EmailIdNotValid));
            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
        }


        [Test]
        public async Task CustomerBusiness_UpdateCustomer_ReturnsSuccess()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { Result = customerModel };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelResponse));

            var customer = await customerBusiness.UpdateCustomer(customerModel);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.UpdateCustomer(customerModel)), Times.Once());
        }

        [Test]
        public async Task CustomerBusiness_UpdateNotExistingCustomer_ReturnsCustomerNotFoundError()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelErrorResponse = new Response<CustomerModel>() { ResultType = ResultType.Error, Messages = new List<string> { Constants.CustomerNotFound } };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelErrorResponse));
            
            var customer = await customerBusiness.UpdateCustomer(customerModel);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.UpdateCustomer(customerModel)), Times.Never());
            Assert.AreEqual(customer.ResultType, ResultType.Error);
            Assert.IsTrue(customer.Messages.Contains(Constants.CustomerNotFound));
        }

        [Test]
        public async Task CustomerBusiness_DeleteCustomer_ReturnsSuccess()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { Result = customerModel };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelResponse));

            var customer = await customerBusiness.DeleteCustomer(customerModel.Email);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.DeleteCustomer(customerModel.Email)), Times.Once());
        }

        [Test]
        public async Task CustomerBusiness_DeletNotExistingCustomer_ReturnsCustomerNotFoundError()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelErrorResponse = new Response<CustomerModel>() { ResultType = ResultType.Error, Messages = new List<string> { Constants.CustomerNotFound } };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelErrorResponse));

            var customer = await customerBusiness.DeleteCustomer(customerModel.Email);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.DeleteCustomer(customerModel.Email)), Times.Never());
            Assert.AreEqual(customer.ResultType, ResultType.Error);
            Assert.IsTrue(customer.Messages.Contains(Constants.CustomerNotFound));
        }

        private CustomerModel CreateCustomer()
        {
            var customerModel = new CustomerModel();
            customerModel.GenderPrefix = GENDERPREFIX;
            customerModel.FirstName = FIRSTNAME;
            customerModel.MiddleName = MIDDLENAME;
            customerModel.LastName = LASTNAME;
            customerModel.Gender = GENDER;
            customerModel.Email = EMAIL;
            return customerModel;
        }
    }
}