using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System.Linq;

namespace Bookmyslot.Api.Customers.ViewModels.Tests.ValidatorTests
{
    [TestFixture]
    public class ProfileSettingsViewModelValidatorTests
    {

        private const string ValidFirstName = "ValidFirstName";
        private const string ValidLastName = "ValidLastName";
        private const string ValidGender = "ValidGender";

        private const string InValidFirstName = "InValidFirstName12212";
        private const string InValidLastName = "InValidLastName121212";
        private const string InValidGender = "InValidGender232323";

        private IValidator<ProfileSettingsViewModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new ProfileSettingsViewModelValidator();
        }


        [Test]
        public void ValidateProfileSettingsViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.ProfileSettingDetailsMissing));
        }


        [Test]
        public void ValidateProfileSettingsViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new ProfileSettingsViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.FirstNameRequired));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.LastNameRequired));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.GenderRequired));
        }

        [Test]
        public void ValidateProfileSettingsViewModel_InValidViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(DefaultInValidProfileSettingViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.FirstNameInValid));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.LastNameInValid));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.GenderNotValid));
        }

        [Test]
        public void ValidateProfileSettingsViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(DefaultValidProfileSettingViewModel());

            Assert.IsTrue(validationResult.IsValid);
        }


        private ProfileSettingsViewModel DefaultInValidProfileSettingViewModel()
        {
            return new ProfileSettingsViewModel()
            {
                FirstName = InValidFirstName,
                LastName = InValidLastName,
                Gender = InValidGender
            };
        }

        private ProfileSettingsViewModel DefaultValidProfileSettingViewModel()
        {
            return new ProfileSettingsViewModel()
            {
                FirstName = ValidFirstName,
                LastName = ValidLastName,
                Gender = ValidGender
            };
        }

    }
}
