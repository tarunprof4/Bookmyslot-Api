using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests
{
    [TestFixture]
    public class SlotSchedulerBusinessTests
    {
        private readonly string SlotId = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private readonly string ValidSlotDate = DateTime.UtcNow.AddDays(2).ToString(DateTimeConstants.ApplicationDatePattern);
        private readonly DateTime ValidSlotDateUtc = DateTime.UtcNow.AddDays(2);
        private readonly string OlderSlotDate = DateTime.UtcNow.AddDays(-2).ToString(DateTimeConstants.ApplicationDatePattern);
        private readonly DateTime OlderSlotDateUtc = DateTime.UtcNow.AddDays(-2);
        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, SlotConstants.MinimumSlotDuration, 0);

        private SlotSchedulerBusiness slotSchedulerBusiness;
        private Mock<ISlotRepository> slotRepositoryMock;

        [SetUp]
        public void Setup()
        {
            slotRepositoryMock = new Mock<ISlotRepository>();
            slotSchedulerBusiness = new SlotSchedulerBusiness(slotRepositoryMock.Object);
        }


        [Test]
        public async Task ScheduleSlot_SlotDateOlderThanCurrentDate_ReturnsValidationResponse()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(OlderSlotDateUtc, TimeZoneConstants.IndianTimezone);
            var bookedByCustomerSummaryModel = new CustomerSummaryModel(new CurrentUserModel() { Id = slotModel.BookedBy });

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel, bookedByCustomerSummaryModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.SlotScheduleDateInvalid);
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Never());
        }


        [Test]
        public async Task ScheduleSlot_CustomerBookingOwnSlot_ReturnsValidationResponse()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(OlderSlotDateUtc, TimeZoneConstants.IndianTimezone);
            slotModel.BookedBy = CreatedBy;
            var bookedByCustomerSummaryModel = new CustomerSummaryModel(new CurrentUserModel() { Id = slotModel.BookedBy });

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel, bookedByCustomerSummaryModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.SlotScheduleCannotBookOwnSlot);
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Never());
        }


        [Test]
        public async Task ScheduleSlot_ValidDetails_ReturnsSuccessResponse()
        {
            var slotModel = CreateValidSlotModel();
            var bookedByCustomerSummaryModel = new CustomerSummaryModel(new CurrentUserModel() { Id = slotModel.BookedBy });

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel, bookedByCustomerSummaryModel);

            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Once());
        }


        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDateUtc, TimeZoneConstants.IndianTimezone);
            slotModel.Title = Title;
            slotModel.CreatedBy = CreatedBy;
            slotModel.SlotStartTime = ValidSlotStartTime;
            slotModel.SlotEndTime = ValidSlotEndTime;

            return slotModel;
        }

      
    }
}