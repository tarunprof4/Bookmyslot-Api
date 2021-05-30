using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookmyslot.Api.Customers.ViewModels.Tests.ValidatorTests
{
    [TestFixture]
    public class AdditionalProfileSettingsViewModelValidatorTests
    {
        private IValidator<AdditionalProfileSettingsViewModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new AdditionalProfileSettingsViewModelValidator();
        }


        [Test]
        public void ValidateAdditionalProfileSettingsViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.AdditionalProfileSettingDetailsMissing));
        }


        [Test]
        public void ValidateAdditionalProfileSettingsViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new AdditionalProfileSettingsViewModel(""));
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.BioHeadLineRequired));
        }

        [Test]
        public void ValidateAdditionalProfileSettingsViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(new AdditionalProfileSettingsViewModel("bio") );

            Assert.IsTrue(validationResult.IsValid);
        }






    }
}
