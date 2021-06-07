using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests.IntegrationEvents
{
   
    [TestFixture]
    public class SlotScheduledIntegrationEventTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string EMAIL = "a@gmail.com";

        private const string Country = "Country";
        private readonly string SlotId = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string SlotMeetingLink = "SlotMeetingLink";
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
        public void CreateSlotScheduledIntegrationEvent()
        {
            var slotModel = CreateValidSlotModel();
            var customerModel = GetDefaultCustomerModel();
            var slotScheduledIntegrationEvent = new SlotScheduledIntegrationEvent(slotModel, customerModel, customerModel);

            Assert.AreEqual(slotScheduledIntegrationEvent.BookedByCustomerModel.FirstName, customerModel.FirstName);
            Assert.AreEqual(slotScheduledIntegrationEvent.BookedByCustomerModel.LastName, customerModel.LastName);
            Assert.AreEqual(slotScheduledIntegrationEvent.BookedByCustomerModel.Email, customerModel.Email);

            Assert.AreEqual(slotScheduledIntegrationEvent.CreatedByCustomerModel.FirstName, customerModel.FirstName);
            Assert.AreEqual(slotScheduledIntegrationEvent.CreatedByCustomerModel.LastName, customerModel.LastName);
            Assert.AreEqual(slotScheduledIntegrationEvent.CreatedByCustomerModel.Email, customerModel.Email);

            Assert.AreEqual(slotScheduledIntegrationEvent.Title, Title);
            Assert.AreEqual(slotScheduledIntegrationEvent.Country, Country);
            Assert.AreEqual(slotScheduledIntegrationEvent.TimeZone, TimeZoneConstants.IndianTimezone);
            Assert.AreEqual(slotScheduledIntegrationEvent.SlotStartTime, slotModel.SlotStartTime);
            Assert.AreEqual(slotScheduledIntegrationEvent.SlotEndTime, slotModel.SlotEndTime);
            Assert.AreEqual(slotScheduledIntegrationEvent.SlotDuration, slotModel.SlotDuration);
            Assert.AreEqual(slotScheduledIntegrationEvent.SlotMeetingLink, slotModel.SlotMeetingLink);
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
            slotModel.SlotMeetingLink = SlotMeetingLink;

            return slotModel;
        }

        private CustomerModel GetDefaultCustomerModel()
        {
            return new CustomerModel()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = EMAIL
            };
        }





    }
}
