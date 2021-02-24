using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
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
    public class LoginBusinessTests
    {
        private const string TOKEN = "Token";
        private const string PROVIDER = "Provider";
        private const string EMAIL = "a@gmail.com";
        private LoginCustomerBusiness loginCustomerBusiness;
        private Mock<IRegisterCustomerBusiness> registerCustomerBusinessMock;
        private Mock<ICustomerBusiness> customerBusinessMock;
        private Mock<ISocialLoginTokenValidator> socialLoginTokenValidatorMock;
        private Mock<IJwtTokenProvider> jwtTokenProviderMock;
        private Mock<ICurrentUser>  currentUser;

        [SetUp]
        public void SetUp()
        {
            registerCustomerBusinessMock = new Mock<IRegisterCustomerBusiness>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            socialLoginTokenValidatorMock = new Mock<ISocialLoginTokenValidator>();
            jwtTokenProviderMock = new Mock<IJwtTokenProvider>();
            currentUser = new Mock<ICurrentUser>();
            loginCustomerBusiness = new LoginCustomerBusiness(registerCustomerBusinessMock.Object, customerBusinessMock.Object,
                socialLoginTokenValidatorMock.Object, jwtTokenProviderMock.Object, currentUser.Object);
        }

        [Test]
        public async Task LoginSocialCustomer_MissingSocialCustomerModel_ReturnValidationErrorResponse()
        {
            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(null);


            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.SocialLoginTokenDetailsMissing));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.ValidateToken(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task LoginSocialCustomer_InValidSocialCustomerModel_ReturnValidationErrorResponse()
        {
            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(new SocialCustomerModel());


            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.TokenRequired));
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.TokenProviderRequired));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.ValidateToken(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task LoginSocialCustomer_InValidSocialLoginToken_ReturnValidationErrorResponse()
        {
            var socialCustomerModel = GetDefaultSocialCustomerModel();
            var inValidTokenResponse = new Response<SocialCustomerModel>() { ResultType = ResultType.ValidationError };
            socialLoginTokenValidatorMock.Setup(a => a.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(inValidTokenResponse));

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(socialCustomerModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.LoginFailed));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.ValidateToken(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task LoginSocialCustomer_ValidTokenAndCustomerAlreadyExists_ReturnLoginSuccessResponse()
        {
            var socialCustomerModel = GetDefaultSocialCustomerModel();
            var validTokenResponse = new Response<SocialCustomerModel>() { Result = socialCustomerModel };
            socialLoginTokenValidatorMock.Setup(a => a.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(validTokenResponse));
            var customerModelResponse = new Response<CustomerModel>() { Result = GetDefaultCustomerModel() };
            customerBusinessMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponse));
            jwtTokenProviderMock.Setup(a => a.GenerateToken(It.IsAny<string>())).Returns(TOKEN);

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(socialCustomerModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.Success);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
            socialLoginTokenValidatorMock.Verify((m => m.ValidateToken(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Once());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task LoginSocialCustomer_ValidTokenAndNewCustomer_ReturnLoginSuccessResponse()
        {
            var socialCustomerModel = GetDefaultSocialCustomerModel();
            var validTokenResponse = new Response<SocialCustomerModel>() { Result = socialCustomerModel };
            socialLoginTokenValidatorMock.Setup(a => a.ValidateToken(It.IsAny<string>())).Returns(Task.FromResult(validTokenResponse));
            var customerModelResponse = new Response<CustomerModel>() { ResultType = ResultType.Empty };
            customerBusinessMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponse));
            var validRegisterCustomerResponse = new Response<string>() { Result = EMAIL }; 
            registerCustomerBusinessMock.Setup(a => a.RegisterCustomer(It.IsAny<RegisterCustomerModel>())).Returns(Task.FromResult(validRegisterCustomerResponse));
            jwtTokenProviderMock.Setup(a => a.GenerateToken(It.IsAny<string>())).Returns(TOKEN);


            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(socialCustomerModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.Success);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
            socialLoginTokenValidatorMock.Verify((m => m.ValidateToken(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Once());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Once());
        }


        private SocialCustomerModel GetDefaultSocialCustomerModel()
        {
            return new SocialCustomerModel() { IdToken = TOKEN, Provider = PROVIDER };
        }


        private CustomerModel GetDefaultCustomerModel()
        {
            return new CustomerModel() {  };
        }

     




    }
}