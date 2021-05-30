using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.ValidatorTests
{
    [TestFixture]
    public class SlotSchedulerViewModelValidatorTests
    {
        private IValidator<SlotSchedulerViewModel> slotSchedulerViewModelValidator;

        [SetUp]
        public void Setup()
        {
            slotSchedulerViewModelValidator = new SlotSchedulerViewModelValidator();
        }


        [Test]
        public void ValidateSlotSchedulerViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = slotSchedulerViewModelValidator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.SlotScheduleInfoMissing));
        }


        [Test]
        public void ValidateSlotSchedulerViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = slotSchedulerViewModelValidator.Validate(new SlotSchedulerViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.SlotScheduleInfoRequired));
        }

        [Test]
        public void ValidateSlotSchedulerViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = slotSchedulerViewModelValidator.Validate(new SlotSchedulerViewModel() {SlotModelKey = "SlotModelKey" });

            Assert.IsTrue(validationResult.IsValid);
        }






    }
}
