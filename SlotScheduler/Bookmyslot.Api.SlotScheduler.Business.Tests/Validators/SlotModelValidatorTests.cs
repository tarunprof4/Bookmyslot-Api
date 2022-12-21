using Bookmyslot.Api.SlotScheduler.Business.Validations;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.Helpers;
using FluentValidation;
using NodaTime;
using NUnit.Framework;
using System;
using System.Linq;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests.Validators
{

    [TestFixture]
    public class SlotModelValidatorTests
    {
        private readonly string SlotId = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string BookedBy = "BookedBy";
        private const string deletedBy = "deletedBy";
        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
        private readonly string InValidSlot = "InValidSlot";
        private readonly DateTime InValidSlotDate = DateTime.UtcNow.AddDays(-2);
        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, SlotConstants.MinimumSlotDuration, 0);

        private IValidator<SlotModel> validator;

        [SetUp]
        public void Setup()
        {
            validator = new SlotModelValidator();
        }





        [Test]
        public void ValidateSlotModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new SlotModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidSlotDate));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotEndTimeInvalid));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotDurationInvalid));
        }

        [Test]
        public void ValidateSlotModel_InValidViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(CreateInvalidSlotModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidSlotDate));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotEndTimeInvalid));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotDurationInvalid));
        }


        [Test]
        public void ValidateSlotModel_DayLightSavingDaySlotViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(CreateDayLightSavingDaySlotModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.DayLightSavinngDateNotAllowed));
        }

        [Test]
        public void ValidateSlotModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(CreateValidSlotModel());

            Assert.IsTrue(validationResult.IsValid);
        }



        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.Title = Title;
            slotModel.CreatedBy = CreatedBy;
            slotModel.SlotStartTime = ValidSlotStartTime;
            slotModel.SlotEndTime = ValidSlotEndTime;

            return slotModel;
        }


        private SlotModel CreateInvalidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(InValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.SlotStartTime = new TimeSpan(23, 0, 0);
            return slotModel;
        }

        private SlotModel CreateDayLightSavingDaySlotModel()
        {
            var slotModel = new SlotModel();
            var localDate = new LocalDateTime(2030, 03, 31, 1, 10, 0);
            var london = DateTimeZoneProviders.Tzdb[TimeZoneConstants.LondonTimezone];

            slotModel.SlotStartZonedDateTime = london.AtLeniently(localDate);
            slotModel.SlotStartTime = new TimeSpan(23, 0, 0);
            return slotModel;
        }


    }

}
