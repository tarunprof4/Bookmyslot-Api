//using Bookmyslot.Api.Common.Contracts.Constants;
//using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
//using FluentValidation;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.ValidatorTests
//{
    
//    [TestFixture]
//    public class SlotViewModelValidatorTests
//    {
//        private IValidator<SlotViewModel> slotViewModelValidator;

//        [SetUp]
//        public void Setup()
//        {
//            slotViewModelValidator = new SlotViewModelValidator();
//        }


//        [Test]
//        public void ValidateSlotViewModel_NullViewModel_ReturnValidationErrorResponse()
//        {
//            var validationResult = slotViewModelValidator.Validate(null);
//            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

//            Assert.IsFalse(validationResult.IsValid);
//            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.SlotDetailsMissing));
//        }


//        [Test]
//        public void ValidateSlotViewModel_EmptyViewModel_ReturnValidationErrorResponse()
//        {
//            var validationResult = slotViewModelValidator.Validate(new ResendSlotInformationViewModel());
//            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

//            Assert.IsFalse(validationResult.IsValid);
//            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoRequired));
//        }

//        [Test]
//        public void ValidateSlotViewModel_ValidViewModel_ReturnSuccessResponse()
//        {
//            var validationResult = slotViewModelValidator.Validate(new ResendSlotInformationViewModel() { ResendSlotModel = "ResendSlotModel" });

//            Assert.IsTrue(validationResult.IsValid);
//        }






//    }
//}
