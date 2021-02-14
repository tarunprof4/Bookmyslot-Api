using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
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
        private Guid SlotId = Guid.NewGuid();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private readonly string ValidSlotDate = DateTime.UtcNow.AddDays(2).ToString(DateTimeConstants.ApplicationInputDatePattern);
        private readonly DateTime ValidSlotDateUtc = DateTime.UtcNow.AddDays(2);
        private readonly string OlderSlotDate = DateTime.UtcNow.AddDays(-2).ToString(DateTimeConstants.ApplicationInputDatePattern);
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
            slotModel.SlotDate = OlderSlotDate;
            slotModel.SlotDateUtc = OlderSlotDateUtc;

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.SlotScheduleDateInvalid);
            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<SlotModel>())), Times.Never());
        }

        [Test]
        public async Task ScheduleSlot_ValidDetails_ReturnsSuccessResponse()
        {
            var slotModel = CreateValidSlotModel();
            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel);

            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<SlotModel>())), Times.Once());
        }


        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.SlotDate = ValidSlotDate;
            slotModel.SlotDateUtc = ValidSlotDateUtc;
            slotModel.TimeZone = TimeZoneConstants.IndianTimezone;
            slotModel.Title = Title;
            slotModel.CreatedBy = CreatedBy;
            slotModel.SlotStartTime = ValidSlotStartTime;
            slotModel.SlotEndTime = ValidSlotEndTime;

            return slotModel;
        }

        private SlotModel CreateDefaultSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.TimeZone = TimeZoneConstants.IndianTimezone;
            return slotModel;
        }
    }
}