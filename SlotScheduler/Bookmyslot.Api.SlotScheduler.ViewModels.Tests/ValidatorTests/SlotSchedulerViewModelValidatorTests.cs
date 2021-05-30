using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void CreateBookAvailableSlotViewModel_EmptyBookAvailableSlotModel_ReturnsEmptyBookAvailableSlotViewModel()
        {
            var messages = slotSchedulerViewModelValidator.Validate(null);
        }

       
     

     

    }
}
