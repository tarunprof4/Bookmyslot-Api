using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class CustomerBusinessTests
    {
        private const string CUSTOMERID = "CUSTOMERID";
        private const string EMAIL = "a@gmail.com";
        private const string Id = "Id";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string BioHeadLine = "BioHeadLine";
        private const bool IsVerified = true;
        private const string ProfilePicUrl = "ProfilePicUrl";
        private const string UserName = "UserName";
        private const string Email = "Email";

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


      

        [Test]
        public async Task GetCustomerByCustomerId_ValidCustomerId_CallsGetCustomerByCustomerIdRepository()
        {
            var customer = await customerBusiness.GetCustomerById(CUSTOMERID);

            customerRepositoryMock.Verify((m => m.GetCustomerById(CUSTOMERID)), Times.Once());
        }



        [Test]
        public async Task GetCustomersByCustomerIds_ValidCustomerIds_CallsGetCustomersByCustomerIdsRepository()
        {
            var customer = await customerBusiness.GetCustomersByCustomerIds(new List<string>() { CUSTOMERID, CUSTOMERID});

            customerRepositoryMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Once());
        }


        [Test]
        public async Task GetCurrentUserByEmail_ValidCustomer_ReturnsCustomerSuccessResponse()
        {
            var customerModel = DefaultCustomerModel();
            Response<CustomerModel> customerModelResponseMock = new Response<CustomerModel>() { Result = customerModel };
            customerRepositoryMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponseMock));

            var currentUserModelResponse = await customerBusiness.GetCurrentUserByEmail(EMAIL);
            var currentUserModel = currentUserModelResponse.Result;

            Assert.AreEqual(customerModel.Id, currentUserModel.Id);
            Assert.AreEqual(customerModel.FirstName, currentUserModel.FirstName);
            Assert.AreEqual(customerModel.LastName, currentUserModel.LastName);
            Assert.AreEqual(customerModel.BioHeadLine, currentUserModel.BioHeadLine);
            Assert.AreEqual(customerModel.IsVerified, currentUserModel.IsVerified);
            Assert.AreEqual(customerModel.ProfilePictureUrl, currentUserModel.ProfilePictureUrl);
            Assert.AreEqual(customerModel.UserName, currentUserModel.UserName);
            Assert.AreEqual(customerModel.Email, currentUserModel.Email);
            customerRepositoryMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
        }

        private CustomerModel DefaultCustomerModel()
        {
            return new CustomerModel()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                BioHeadLine = BioHeadLine,
                IsVerified = IsVerified,
                ProfilePictureUrl = ProfilePicUrl,
                UserName = UserName,
                Email = EMAIL
            };
        }

    }
}