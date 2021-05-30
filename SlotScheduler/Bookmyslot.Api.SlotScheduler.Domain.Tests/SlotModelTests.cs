using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using NodaTime;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.SlotScheduler.Domain.Tests
{
    public class SlotModelTests
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

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsSlotDateValid_InValidSlotDate_ReturnsFalse()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(InValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.IsSlotDateValid();

            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsSlotDateValid_ValidSlotDate_ReturnsTrue()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.IsSlotDateValid();

            Assert.IsTrue(isValid);
        }

        [Test]
        public void SlotNotAllowedOnDayLightSavingDay_DayLightSavingSlotDate_ReturnsFalse()
        {
            var slotModel = CreateDayLightSavingDaySlotModel();

            var isValid = slotModel.SlotNotAllowedOnDayLightSavingDay();

            Assert.IsFalse(isValid);
        }

        [Test]
        public void SlotNotAllowedOnDayLightSavingDay_NotDayLightSavingSlotDate_ReturnsTrue()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.SlotNotAllowedOnDayLightSavingDay();

            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsSlotEndTimeValid_InValidSlotTimings_ReturnFalse()
        {
            var slotModel = CreateInvalidSlotModel();

            var isValid = slotModel.IsSlotEndTimeValid();

            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsSlotEndTimeValid_ValidSlotTimings_ReturnTrue()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.IsSlotEndTimeValid();

            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsSlotDurationValid_InValidSlotTimings_ReturnsFalse()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotEndTime = new TimeSpan(0, 5, 5);
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.IsSlotDurationValid();

            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsSlotDurationValid_ValidSlotTimings_ReturnsTrue()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.IsSlotDurationValid();

            Assert.IsTrue(isValid);
        }


        [Test]
        public void CancelSlot_CancelledByBookedBy_ReturnsUpdateSlot()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.BookedBy = BookedBy;

            var slotStatus = slotModel.CancelSlot(BookedBy);

            Assert.AreEqual(slotStatus, SlotConstants.UpdateSlot);
        }

        [Test]
        public void CancelSlot_CancelledByCreatedBy_ReturnsDeleteSlot()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.BookedBy = deletedBy;

            var slotStatus = slotModel.CancelSlot(BookedBy);

            Assert.AreEqual(slotStatus, SlotConstants.DeleteSlot); ;
        }

        [Test]
        public void IsSlotBookedByHimself_BookedByHimself_ReturnsTrue()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.CreatedBy = BookedBy;

            var bookedByHimself = slotModel.IsSlotBookedByHimself(BookedBy);

            Assert.IsTrue(bookedByHimself);
        }

        [Test]
        public void IsSlotBookedByHimself_NotBookedByHimself_ReturnsFalse()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.CreatedBy = deletedBy;

            var bookedByHimself = slotModel.IsSlotBookedByHimself(BookedBy);

            Assert.IsFalse(bookedByHimself);
        }


        [Test]
        public void IsSlotScheduleDateValid_InValidDate_ReturnsFalse()
        {
            var slotModel = CreateInvalidSlotModel();

            var isValid = slotModel.IsSlotScheduleDateValid();

            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsSlotScheduleDateValid_ValidDate_ReturnsTrue()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);

            var isValid = slotModel.IsSlotScheduleDateValid();

            Assert.IsTrue(isValid);
        }


        [Test]
        public void ScheduleSlot_SetSlotRelatedProperties()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.BookedBy = string.Empty;
            slotModel.SlotMeetingLink = string.Empty;

            slotModel.ScheduleSlot(BookedBy);

            Assert.AreEqual(slotModel.BookedBy, BookedBy);
            Assert.AreNotEqual(slotModel.SlotMeetingLink, string.Empty);
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