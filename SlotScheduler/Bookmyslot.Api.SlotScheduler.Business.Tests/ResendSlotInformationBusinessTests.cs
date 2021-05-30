using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests
{
    [TestFixture]
    public class ResendSlotInformationBusinessTests
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

        private ResendSlotInformationBusiness resendSlotInformationBusiness;
        private Mock<IEmailInteraction> emailInteractionMock;
        private Mock<ICustomerBusiness> customerBusinessMock;

        [SetUp]
        public void Setup()
        {
            emailInteractionMock = new Mock<IEmailInteraction>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            resendSlotInformationBusiness = new ResendSlotInformationBusiness(emailInteractionMock.Object, customerBusinessMock.Object);
        }

   
     
        //[Test]
        //public async Task ScheduleSlot_ValidDetails_ReturnsSuccessResponse()
        //{
        //    var slotModel = CreateValidSlotModel();
        //    var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel);

        //    slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<SlotModel>())), Times.Once());
        //}


        //[Test]
        //public async Task ResendSlotMeetingInformation_SlotMeetingTimeNotLessThanMinimumTime_ReturnsValidationResponse()
        //{
        //    var slotModel = CreateValidSlotModel();
        //    slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(OlderSlotDateUtc, TimeZoneConstants.IndianTimezone);

        //    var slotModelResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(slotModel, string.Empty);

        //    Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
        //    Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.MinimumDaysForSlotMeetingLink);
        //}



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