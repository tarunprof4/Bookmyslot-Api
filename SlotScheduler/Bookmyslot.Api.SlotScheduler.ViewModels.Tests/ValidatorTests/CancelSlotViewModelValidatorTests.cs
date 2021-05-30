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
    public class CancelSlotViewModelValidatorTests
    {
        private IValidator<CancelSlotViewModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new CancelSlotViewModelValidator();
        }


        [Test]
        public void ValidateCancelSlotViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.CancelSlotInfoMissing));
        }


        [Test]
        public void ValidateCancelSlotViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new CancelSlotViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.CancelSlotRequired));
        }

        [Test]
        public void ValidateCancelSlotViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(new CancelSlotViewModel() { SlotKey = "SlotKey" });

            Assert.IsTrue(validationResult.IsValid);
        }






    }
}
