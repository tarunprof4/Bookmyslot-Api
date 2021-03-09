using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
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
        private const string AuthToken = "AuthToken";
        private const string EMAIL = "a@gmail.com";
        private const string INVALIDPROVIDER = "provider";
        private LoginCustomerBusiness loginCustomerBusiness;
        private Mock<IRegisterCustomerBusiness> registerCustomerBusinessMock;
        private Mock<ICustomerBusiness> customerBusinessMock;
        private Mock<ISocialLoginTokenValidator> socialLoginTokenValidatorMock;
        private Mock<IJwtTokenProvider> jwtTokenProviderMock;
        private Mock<ICurrentUser> currentUser;

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
        public async Task LoginGoogleCustomer_MissingSocialCustomerModel_ReturnValidationErrorResponse()
        {
            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(null);


            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.SocialLoginTokenDetailsMissing));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithGoogle(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task LoginGoogleCustomer_InValidSocialCustomerModel_ReturnValidationErrorResponse()
        {
            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(new SocialCustomerLoginModel() { Provider = LoginConstants.ProviderGoogle });


            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.AuthTokenRequired));
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.IdTokenRequired));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithGoogle(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task LoginGoogleCustomer_InValidSocialLoginToken_ReturnValidationErrorResponse()
        {
            var googleCustomerLoginModel = GetDefaultGoogleCustomerLoginModel();
            var inValidTokenResponse = new Response<SocialCustomerModel>() { ResultType = ResultType.ValidationError };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithGoogle(It.IsAny<string>())).Returns(Task.FromResult(inValidTokenResponse));

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(googleCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithGoogle(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task LoginGoogleCustomer_ValidTokenAndCustomerAlreadyExists_ReturnLoginSuccessResponse()
        {
            var googleCustomerLoginModel = GetDefaultGoogleCustomerLoginModel();
            var validTokenResponse = new Response<SocialCustomerModel>() { Result = GetDefaultSocialCustomerModel(LoginConstants.ProviderGoogle) };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithGoogle(It.IsAny<string>())).Returns(Task.FromResult(validTokenResponse));
            var customerModelResponse = new Response<CustomerModel>() { Result = GetDefaultCustomerModel() };
            customerBusinessMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponse));
            jwtTokenProviderMock.Setup(a => a.GenerateToken(It.IsAny<string>())).Returns(TOKEN);

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(googleCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.Success);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithGoogle(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Once());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task LoginGoogleCustomer_ValidTokenAndNewCustomer_ReturnLoginSuccessResponse()
        {
            var googleCustomerLoginModel = GetDefaultGoogleCustomerLoginModel();
            var validTokenResponse = new Response<SocialCustomerModel>() { Result = GetDefaultSocialCustomerModel(LoginConstants.ProviderGoogle) };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithGoogle(It.IsAny<string>())).Returns(Task.FromResult(validTokenResponse));
            var customerModelResponse = new Response<CustomerModel>() { ResultType = ResultType.Empty };
            customerBusinessMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponse));
            var validRegisterCustomerResponse = new Response<string>() { Result = EMAIL };
            registerCustomerBusinessMock.Setup(a => a.RegisterCustomer(It.IsAny<RegisterCustomerModel>())).Returns(Task.FromResult(validRegisterCustomerResponse));
            jwtTokenProviderMock.Setup(a => a.GenerateToken(It.IsAny<string>())).Returns(TOKEN);


            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(googleCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.Success);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithGoogle(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Once());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Once());
        }








        [Test]
        public async Task LoginFacebookCustomer_MissingSocialCustomerModel_ReturnValidationErrorResponse()
        {
            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(null);


            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.SocialLoginTokenDetailsMissing));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task LoginFacebookCustomer_InValidSocialCustomerModel_ReturnValidationErrorResponse()
        {
            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(new SocialCustomerLoginModel() { Provider = LoginConstants.ProviderFacebook });


            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.AuthTokenRequired));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task LoginFacebookCustomer_InValidSocialLoginToken_ReturnValidationErrorResponse()
        {
            var facebookCustomerLoginModel = GetDefaultFacebookCustomerLoginModel();
            var inValidTokenResponse = new Response<SocialCustomerModel>() { ResultType = ResultType.ValidationError };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithFacebook(It.IsAny<string>())).Returns(Task.FromResult(inValidTokenResponse));

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(facebookCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task LoginFacebookCustomer_ValidTokenAndCustomerAlreadyExists_ReturnLoginSuccessResponse()
        {
            var facebookCustomerLoginModel = GetDefaultFacebookCustomerLoginModel();
            var validTokenResponse = new Response<SocialCustomerModel>() { Result = GetDefaultSocialCustomerModel(LoginConstants.ProviderFacebook) };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithFacebook(It.IsAny<string>())).Returns(Task.FromResult(validTokenResponse));
            var customerModelResponse = new Response<CustomerModel>() { Result = GetDefaultCustomerModel() };
            customerBusinessMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponse));
            jwtTokenProviderMock.Setup(a => a.GenerateToken(It.IsAny<string>())).Returns(TOKEN);

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(facebookCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.Success);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Once());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task LoginFacebookCustomer_ValidTokenAndNewCustomer_ReturnLoginSuccessResponse()
        {
            var facebookCustomerLoginModel = GetDefaultFacebookCustomerLoginModel();
            var validTokenResponse = new Response<SocialCustomerModel>() { Result = GetDefaultSocialCustomerModel(LoginConstants.ProviderFacebook) };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithFacebook(It.IsAny<string>())).Returns(Task.FromResult(validTokenResponse));
            var customerModelResponse = new Response<CustomerModel>() { ResultType = ResultType.Empty };
            customerBusinessMock.Setup(a => a.GetCustomerByEmail(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponse));
            var validRegisterCustomerResponse = new Response<string>() { Result = EMAIL };
            registerCustomerBusinessMock.Setup(a => a.RegisterCustomer(It.IsAny<RegisterCustomerModel>())).Returns(Task.FromResult(validRegisterCustomerResponse));
            jwtTokenProviderMock.Setup(a => a.GenerateToken(It.IsAny<string>())).Returns(TOKEN);


            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(facebookCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.Success);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Once());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Once());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Once());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task LoginSocialCustomer_InValidSocialProvider_ReturnLoginSuccessResponse()
        {
            var facebookCustomerLoginModel = GetDefaultFacebookCustomerLoginModel();
            facebookCustomerLoginModel.Provider = INVALIDPROVIDER;


            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(facebookCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(AppBusinessMessagesConstants.InValidTokenProvider));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
        }


        private SocialCustomerLoginModel GetDefaultGoogleCustomerLoginModel()
        {
            return new SocialCustomerLoginModel() { AuthToken = AuthToken, IdToken = TOKEN, Provider = LoginConstants.ProviderGoogle };
        }

        private SocialCustomerLoginModel GetDefaultFacebookCustomerLoginModel()
        {
            return new SocialCustomerLoginModel() { AuthToken = AuthToken, Provider = LoginConstants.ProviderFacebook };
        }

        private SocialCustomerModel GetDefaultSocialCustomerModel(string provider)
        {
            return new SocialCustomerModel() { Provider = provider };
        }



        private CustomerModel GetDefaultCustomerModel()
        {
            return new CustomerModel() { };
        }






    }
}