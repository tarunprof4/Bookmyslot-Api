using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class RegisterCustomerBusinessTests
    {
        private const string EMAIL = "a@gmail.com";
        private const string FIRSTNAME = "fisrtname";
        private const string LASTNAME = "lastname";
        private RegisterCustomerBusiness registerCustomerBusiness;
        private Mock<IRegisterCustomerRepository> registerCustomerRepositoryMock;
        private Mock<ICustomerRepository> customerRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            registerCustomerRepositoryMock = new Mock<IRegisterCustomerRepository>();
            customerRepositoryMock = new Mock<ICustomerRepository>();
            registerCustomerBusiness = new RegisterCustomerBusiness(registerCustomerRepositoryMock.Object, customerRepositoryMock.Object);
        }

        [Test]
        public async Task RegisterCustomer_ValidCustomerDetails_ReturnsSuccess()
        {
            var registerCustomerModel = CreateRegisterCustomerModel();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { ResultType = ResultType.Empty };
            customerRepositoryMock.Setup(a => a.GetCustomerByEmail(registerCustomerModel.Email)).Returns(Task.FromResult(customerModelResponse));

            await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);

            customerRepositoryMock.Verify((m => m.GetCustomerByEmail(registerCustomerModel.Email)), Times.Once());
            registerCustomerRepositoryMock.Verify((m => m.CreateCustomer(registerCustomerModel)), Times.Once());
        }

        [Test]
        public async Task RegisterCustomer_CreateMissingCustomerDetails_ReturnsValidationError()
        {
            var registerCustomerResponse = await registerCustomerBusiness.RegisterCustomer(null);


            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.RegisterCustomerDetailsMissing));
            Assert.AreEqual(registerCustomerResponse.ResultType, ResultType.ValidationError);
        }

        [Test]
        public async Task RegisterCustomer_CreateInvalidCustomer_ReturnsValidationError()
        {
            var registerCustomerModel = new RegisterCustomerModel();

            var registerCustomerResponse = await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);

            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.FirstNameInValid));
            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.LastNameInValid));
            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.EmailIdNotValid));
            Assert.AreEqual(registerCustomerResponse.ResultType, ResultType.ValidationError);
        }

        [Test]
        public async Task RegisterCustomer_CustomerWithSameEmailIdAlreadyExists_ReturnsError()
        {
            var registerCustomerModel = CreateRegisterCustomerModel();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { ResultType = ResultType.Success };
            customerRepositoryMock.Setup(a => a.GetCustomerByEmail(registerCustomerModel.Email)).Returns(Task.FromResult(customerModelResponse));

            var registerCustomerResponse = await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);

            customerRepositoryMock.Verify((m => m.GetCustomerByEmail(registerCustomerModel.Email)), Times.Once());
            registerCustomerRepositoryMock.Verify((m => m.CreateCustomer(registerCustomerModel)), Times.Never());
            Assert.AreEqual(registerCustomerResponse.ResultType, ResultType.ValidationError);
        }


        [Test]
        public async Task CreateCustomer_WithInvalidCustomerNameAndEmail_ReturnsValidationError()
        {
            var registerCustomerModel = new RegisterCustomerModel() { FirstName = " 12 ", Email = "asdf.com", LastName="12" };

            var registerCustomerResponse = await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);

            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.FirstNameInValid));
            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.LastNameInValid));
            Assert.IsTrue(registerCustomerResponse.Messages.Contains(AppBusinessMessagesConstants.EmailIdNotValid));
            Assert.AreEqual(registerCustomerResponse.ResultType, ResultType.ValidationError);
        }

        private RegisterCustomerModel CreateRegisterCustomerModel()
        {
            var registerCustomerModel = new RegisterCustomerModel();
            registerCustomerModel.FirstName = FIRSTNAME;
            registerCustomerModel.LastName = LASTNAME;
            registerCustomerModel.Email = EMAIL;
            return registerCustomerModel;
        }
    }
}