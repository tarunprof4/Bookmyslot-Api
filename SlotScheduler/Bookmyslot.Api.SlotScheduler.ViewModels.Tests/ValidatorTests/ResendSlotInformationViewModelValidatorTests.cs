using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System.Linq;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.ValidatorTests
{

    [TestFixture]
    public class ResendSlotInformationViewModelValidatorTests
    {
        private IValidator<ResendSlotInformationViewModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new ResendSlotInformationViewModelValidator();
        }


        [Test]
        public void ValidateResendSlotInformationViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoMissing));
        }


        [Test]
        public void ValidateResendSlotInformationViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new ResendSlotInformationViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoRequired));
        }

        [Test]
        public void ValidateResendSlotInformationViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(new ResendSlotInformationViewModel() { ResendSlotModel = "ResendSlotModel" });

            Assert.IsTrue(validationResult.IsValid);
        }






    }
}
