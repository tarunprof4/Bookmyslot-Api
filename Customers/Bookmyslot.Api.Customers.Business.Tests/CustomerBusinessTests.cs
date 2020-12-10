using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task GetCustomerByEmail_ValidCustomerEmail_CallsGetCustomerByEmailRepository()
        {
            var customer = await customerBusiness.GetCustomer(EMAIL);

            customerRepositoryMock.Verify((m => m.GetCustomer(EMAIL)), Times.Once());
        }


        [TestCase("")]
        [TestCase("   ")]
        public async Task GetCustomerByEmail_InvalidEmailId_ReturnsValidationErrorResponse(string email)
        {
            var customer = await customerBusiness.GetCustomer(email);

            Assert.AreEqual(customer.HasResult, false);
            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
            Assert.AreEqual(customer.Messages.First(), AppBusinessMessages.EmailIdNotValid);
        }

        [Test]
        public async Task GetAllCustomers_Valid_CallsGetAllCustomersRepository()
        {
            var customer = await customerBusiness.GetAllCustomers();

            customerRepositoryMock.Verify((m => m.GetAllCustomers()), Times.Once());
        }


        [Test]
        public async Task CreateCustomer_ValidCustomerDetails_ReturnsSuccess()
        {
            var customerModel = CreateCustomer();

            var customer = await customerBusiness.CreateCustomer(customerModel);

            customerRepositoryMock.Verify((m => m.CreateCustomer(customerModel)), Times.Once());
        }

        [Test]
        public async Task CreateCustomer_CreateMissingCustomerDetails_ReturnsValidationError()
        {
            var customer = await customerBusiness.CreateCustomer(null);

            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.CustomerDetailsMissing));
            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
        }

        [Test]
        public async Task CreateCustomer_CreateInvalidCustomer_ReturnsValidationError()
        {
            var customerModel = new CustomerModel();

            var customer = await customerBusiness.CreateCustomer(customerModel);

            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.GenderPrefixInValid));
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.FirstNameInValid));
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.LastNameInValid));
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.GenderNotValid));
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.EmailIdNotValid));
            Assert.AreEqual(customer.ResultType,ResultType.ValidationError);
        }


        [Test]
        public async Task CreateCustomer_WithInvalidCustomerNameAndEmail_ReturnsValidationError()
        {
            var customerModel = new CustomerModel() { FirstName="12", MiddleName = "2@", Email ="asdf.com" };

            var customer = await customerBusiness.CreateCustomer(customerModel);

            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.FirstNameInValid));
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.MiddleNameInValid));
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.EmailIdNotValid));
            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
        }


        [Test]
        public async Task UpdateCustomer_ValidCustomerDetails_ReturnsSuccess()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { Result = customerModel };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelResponse));

            var customer = await customerBusiness.UpdateCustomer(customerModel);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.UpdateCustomer(customerModel)), Times.Once());
        }

        [Test]
        public async Task UpdateCustomer_MissingCustomerDetails_ReturnsValidationError()
        {
            var customer = await customerBusiness.UpdateCustomer(null);

            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.CustomerDetailsMissing));
            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
        }

        [Test]
        public async Task UpdateCustomer_UpdateNotExistingCustomer_ReturnsCustomerNotFoundError()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelErrorResponse = new Response<CustomerModel>() { ResultType = ResultType.Error, Messages = new List<string> { AppBusinessMessages.CustomerNotFound } };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelErrorResponse));
            
            var customer = await customerBusiness.UpdateCustomer(customerModel);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.UpdateCustomer(customerModel)), Times.Never());
            Assert.AreEqual(customer.ResultType, ResultType.Error);
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.CustomerNotFound));
        }

        [Test]
        public async Task DeleteCustomer_ValidCustomerEmail_ReturnsSuccess()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { Result = customerModel };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelResponse));

            var customer = await customerBusiness.DeleteCustomer(customerModel.Email);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.DeleteCustomer(customerModel.Email)), Times.Once());
        }

        [Test]
        public async Task DeleteCustomer_DeletNotExistingCustomer_ReturnsCustomerNotFoundError()
        {
            var customerModel = CreateCustomer();
            Response<CustomerModel> customerModelErrorResponse = new Response<CustomerModel>() { ResultType = ResultType.Error, Messages = new List<string> { AppBusinessMessages.CustomerNotFound } };
            customerRepositoryMock.Setup(a => a.GetCustomer(customerModel.Email)).Returns(Task.FromResult(customerModelErrorResponse));

            var customer = await customerBusiness.DeleteCustomer(customerModel.Email);

            customerRepositoryMock.Verify((m => m.GetCustomer(customerModel.Email)), Times.Once());
            customerRepositoryMock.Verify((m => m.DeleteCustomer(customerModel.Email)), Times.Never());
            Assert.AreEqual(customer.ResultType, ResultType.Error);
            Assert.IsTrue(customer.Messages.Contains(AppBusinessMessages.CustomerNotFound));
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