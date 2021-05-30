using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
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
        private const string InvalidSocialCustomerLoginModel = "InvalidSocialCustomerLoginModel";
        private LoginCustomerBusiness loginCustomerBusiness;
        private Mock<IRegisterCustomerBusiness> registerCustomerBusinessMock;
        private Mock<ICustomerBusiness> customerBusinessMock;
        private Mock<ISocialLoginTokenValidator> socialLoginTokenValidatorMock;
        private Mock<IJwtTokenProvider> jwtTokenProviderMock;
        private Mock<ICurrentUser> currentUser;
        private Mock<IValidator<SocialCustomerLoginModel>> socialLoginCustomerValidatorMock;

        [SetUp]
        public void SetUp()
        {
            registerCustomerBusinessMock = new Mock<IRegisterCustomerBusiness>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            socialLoginTokenValidatorMock = new Mock<ISocialLoginTokenValidator>();
            jwtTokenProviderMock = new Mock<IJwtTokenProvider>();
            currentUser = new Mock<ICurrentUser>();
            socialLoginCustomerValidatorMock = new Mock<IValidator<SocialCustomerLoginModel>>();
            loginCustomerBusiness = new LoginCustomerBusiness(registerCustomerBusinessMock.Object, customerBusinessMock.Object,
                socialLoginTokenValidatorMock.Object, jwtTokenProviderMock.Object, currentUser.Object,
                socialLoginCustomerValidatorMock.Object);
        }


        

        [Test]
        public async Task LoginGoogleCustomer_InValidSocialCustomerLoginModel_ReturnValidationErrorResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailurer();
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult(validationFailures));
            var googleCustomerLoginModel = GetDefaultGoogleCustomerLoginModel();
            var inValidTokenResponse = new Response<SocialCustomerModel>() { ResultType = ResultType.ValidationError };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithGoogle(It.IsAny<string>())).Returns(Task.FromResult(inValidTokenResponse));

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(googleCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(loginResponse.Messages.Contains(InvalidSocialCustomerLoginModel));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithGoogle(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
        }

   

        [Test]
        public async Task LoginGoogleCustomer_ValidTokenAndCustomerAlreadyExists_ReturnLoginSuccessResponse()
        {
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult());
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
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
        }


        [Test]
        public async Task LoginGoogleCustomer_ValidTokenAndNewCustomer_ReturnLoginSuccessResponse()
        {
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult());
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
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
        }



       

        [Test]
        public async Task LoginFacebookCustomer_InValidSocialCustomerLoginModel_ReturnValidationErrorResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailurer();
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult(validationFailures));
            var facebookCustomerLoginModel = GetDefaultFacebookCustomerLoginModel();
            var inValidTokenResponse = new Response<SocialCustomerModel>() { ResultType = ResultType.ValidationError };
            socialLoginTokenValidatorMock.Setup(a => a.LoginWithFacebook(It.IsAny<string>())).Returns(Task.FromResult(inValidTokenResponse));

            var loginResponse = await loginCustomerBusiness.LoginSocialCustomer(facebookCustomerLoginModel);

            Assert.AreEqual(loginResponse.ResultType, ResultType.ValidationError);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerByEmail(It.IsAny<string>())), Times.Never());
            socialLoginTokenValidatorMock.Verify((m => m.LoginWithFacebook(It.IsAny<string>())), Times.Never());
            jwtTokenProviderMock.Verify((m => m.GenerateToken(It.IsAny<string>())), Times.Never());
            currentUser.Verify((m => m.SetCurrentUserInCache(It.IsAny<string>())), Times.Never());
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
        }


        [Test]
        public async Task LoginFacebookCustomer_ValidTokenAndCustomerAlreadyExists_ReturnLoginSuccessResponse()
        {
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult());
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
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
        }


        [Test]
        public async Task LoginFacebookCustomer_ValidTokenAndNewCustomer_ReturnLoginSuccessResponse()
        {
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult());
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
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
        }


        [Test]
        public async Task LoginSocialCustomer_InValidSocialProvider_ReturnLoginSuccessResponse()
        {
            socialLoginCustomerValidatorMock.Setup(a => a.Validate(It.IsAny<SocialCustomerLoginModel>())).Returns(new ValidationResult());
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
            socialLoginCustomerValidatorMock.Verify((m => m.Validate(It.IsAny<SocialCustomerLoginModel>())), Times.Once());
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

        private static List<ValidationFailure> CreateDefaultValidationFailurer()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InvalidSocialCustomerLoginModel);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }




    }
}