using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        private Mock<ICustomerBusiness> customerBusinessMock;
        

        [SetUp]
        public void Setup()
        {
            slotRepositoryMock = new Mock<ISlotRepository>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            slotSchedulerBusiness = new SlotSchedulerBusiness(slotRepositoryMock.Object, customerBusinessMock.Object);
        }


        [Test]
        public async Task ScheduleSlot_SlotDateOlderThanCurrentDate_ReturnsValidationResponse()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(OlderSlotDateUtc, TimeZoneConstants.IndianTimezone);

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel, slotModel.BookedBy);

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

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel, slotModel.BookedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.SlotScheduleCannotBookOwnSlot);
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Never());
        }


        [Test]
        public async Task ScheduleSlot_ValidDetails_ReturnsSuccessResponse()
        {
            var slotModel = CreateValidSlotModel();
            var customerModelsResponse = GetValidCustomerModels(slotModel);
            customerBusinessMock.Setup(a => a.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult(customerModelsResponse));

            var slotModelResponse = await this.slotSchedulerBusiness.ScheduleSlot(slotModel, slotModel.BookedBy);

            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Once());
        }

        private static Response<List<CustomerModel>> GetValidCustomerModels(SlotModel slotModel)
        {
            var customerModels = new List<CustomerModel>();
            var bookedByCustomerModel = new CustomerModel() { Id = slotModel.BookedBy };
            var createdByCustomerModel = new CustomerModel() { Id = slotModel.CreatedBy };
            customerModels.Add(bookedByCustomerModel);
            customerModels.Add(createdByCustomerModel);
            var customerModelsResponse = new Response<List<CustomerModel>>();
            customerModelsResponse.Result = customerModels;
            return customerModelsResponse;
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