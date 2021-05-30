using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Business.Validations;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookmyslot.Api.Customers.Business.Tests.Validators
{


    [TestFixture]
    public class SocialLoginCustomerValidatorTests
    {
        private const string TOKEN = "Token";
        private const string AuthToken = "AuthToken";
        private const string EMAIL = "a@gmail.com";
        private const string INVALIDPROVIDER = "provider";
        private IValidator<SocialCustomerLoginModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new SocialLoginCustomerValidator();
        }


        [Test]
        public void ValidateSocialCustomerLoginModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.SocialLoginTokenDetailsMissing));
        }


        [Test]
        public void ValidateSocialCustomerLoginModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new SocialCustomerLoginModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.AuthTokenRequired));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.TokenProviderRequired));
        }

        [Test]
        public void ValidateSocialCustomerLoginModel_GoogleProviderIdTokenMissingViewModel_ReturnValidationErrorResponse()
        {
            var googleSocialCustomerModel = GetDefaultGoogleCustomerLoginModel();
            googleSocialCustomerModel.IdToken = string.Empty;
            var validationResult = validator.Validate(googleSocialCustomerModel);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.IdTokenRequired));
        }

        [Test]
        public void ValidateSocialCustomerLoginModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(GetDefaultFacebookCustomerLoginModel());

            Assert.IsTrue(validationResult.IsValid);
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




    }
}
