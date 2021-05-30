using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System.Linq;

namespace Bookmyslot.Api.Customers.ViewModels.Tests.ValidatorTests
{

    [TestFixture]
    public class RegisterCustomerViewModelValidatorTests
    {
        private const string ValidFirstName = "ValidFirstName";
        private const string ValidLastName = "ValidLastName";
        private const string ValidEmail = "a@gmail.com";

        private const string InValidFirstName = "InValidFirstName12212";
        private const string InValidLastName = "InValidLastName121212";
        private const string InValidEmail = "InValidEmail";

        private IValidator<RegisterCustomerViewModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new RegisterCustomerViewModelValidator();
        }


        [Test]
        public void ValidateRegisterCustomerViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.RegisterCustomerDetailsMissing));
        }


        [Test]
        public void ValidateRegisterCustomerViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new RegisterCustomerViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.FirstNameRequired));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.LastNameRequired));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.EmailRequired));
        }

        [Test]
        public void ValidateRegisterCustomerViewModel_InValidViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(DefaultInValidRegisterCustomerViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(InValidFirstName));
            Assert.IsTrue(validationErrorMessages.Contains(InValidLastName));
            Assert.IsTrue(validationErrorMessages.Contains(InValidEmail));
        }

        [Test]
        public void ValidateRegisterCustomerViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(DefaultValidRegisterCustomerViewModel());

            Assert.IsTrue(validationResult.IsValid);
        }


        private RegisterCustomerViewModel DefaultValidRegisterCustomerViewModel()
        {
            return new RegisterCustomerViewModel()
            {
                FirstName = ValidFirstName,
                LastName = ValidLastName,
                Email = ValidEmail
            };
        }

        private RegisterCustomerViewModel DefaultInValidRegisterCustomerViewModel()
        {
            return new RegisterCustomerViewModel()
            {
                FirstName = InValidFirstName,
                LastName = InValidLastName,
                Email = InValidEmail
            };
        }
    }
}
