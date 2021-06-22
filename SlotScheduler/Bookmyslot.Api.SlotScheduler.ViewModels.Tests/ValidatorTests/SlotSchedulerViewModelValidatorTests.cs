using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System.Linq;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.ValidatorTests
{
    [TestFixture]
    public class SlotSchedulerViewModelValidatorTests
    {
        private IValidator<SlotSchedulerViewModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new SlotSchedulerViewModelValidator();
        }


        [Test]
        public void ValidateSlotSchedulerViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.SlotScheduleInfoMissing));
        }


        [Test]
        public void ValidateSlotSchedulerViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new SlotSchedulerViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.SlotScheduleInfoRequired));
        }

        [Test]
        public void ValidateSlotSchedulerViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(new SlotSchedulerViewModel() { SlotModelKey = "SlotModelKey" });

            Assert.IsTrue(validationResult.IsValid);
        }






    }
}
