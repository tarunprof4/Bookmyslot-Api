using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests.IntegrationEvents
{


    [TestFixture]
    public class SlotMeetingInformationRequestedIntegrationEventTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string EMAIL = "a@gmail.com";

        private const string Country = "Country";
        private readonly string SlotId = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string SlotMeetingLink = "SlotMeetingLink";
        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
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
            var slotMeetingInformationRequestedIntegrationEvent = new SlotMeetingInformationRequestedIntegrationEvent(slotModel, customerModel);

            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.ResendToCustomerModel.FirstName, customerModel.FirstName);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.ResendToCustomerModel.LastName, customerModel.LastName);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.ResendToCustomerModel.Email, customerModel.Email);

            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.Title, Title);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.Country, Country);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.TimeZone, TimeZoneConstants.IndianTimezone);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.SlotStartTime, slotModel.SlotStartTime);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.SlotEndTime, slotModel.SlotEndTime);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.SlotDuration, slotModel.SlotDuration);
            Assert.AreEqual(slotMeetingInformationRequestedIntegrationEvent.SlotMeetingLink, slotModel.SlotMeetingLink);
        }





        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.Title = Title;
            slotModel.Country = Country;
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
