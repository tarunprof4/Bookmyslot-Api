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
    public class ResendSlotInformationViewModelValidatorTests
    {
        private IValidator<ResendSlotInformationViewModel> resendSlotInformationViewModelValidator;

        [SetUp]
        public void Setup()
        {
            resendSlotInformationViewModelValidator = new ResendSlotInformationViewModelValidator();
        }


        [Test]
        public void ValidateResendSlotInformationViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = resendSlotInformationViewModelValidator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoMissing));
        }


        [Test]
        public void ValidateResendSlotInformationViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = resendSlotInformationViewModelValidator.Validate(new ResendSlotInformationViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoRequired));
        }

        [Test]
        public void ValidateResendSlotInformationViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = resendSlotInformationViewModelValidator.Validate(new ResendSlotInformationViewModel() { ResendSlotModel = "ResendSlotModel" });

            Assert.IsTrue(validationResult.IsValid);
        }






    }
}
