using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.Helpers;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests.IntegrationEvents
{

    [TestFixture]
    public class SlotCancelledIntegrationEventTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string EMAIL = "a@gmail.com";

        private const string Country = "Country";
        private const string Title = "Title";
        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, SlotConstants.MinimumSlotDuration, 0);

        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void CreateSlotCancelledIntegrationEvent()
        {
            var cancelledSlotModel = CreateValidCancelledSlotModel();
            var customerModel = GetDefaultCustomerModel();
            var slotCancelledIntegrationEvent = new SlotCancelledIntegrationEvent(cancelledSlotModel, customerModel);

            Assert.AreEqual(slotCancelledIntegrationEvent.CancelledByCustomerModel.FirstName, customerModel.FirstName);
            Assert.AreEqual(slotCancelledIntegrationEvent.CancelledByCustomerModel.LastName, customerModel.LastName);
            Assert.AreEqual(slotCancelledIntegrationEvent.CancelledByCustomerModel.Email, customerModel.Email);

            Assert.AreEqual(slotCancelledIntegrationEvent.Title, Title);
            Assert.AreEqual(slotCancelledIntegrationEvent.Country, Country);
            Assert.AreEqual(slotCancelledIntegrationEvent.TimeZone, TimeZoneConstants.IndianTimezone);
            Assert.AreEqual(slotCancelledIntegrationEvent.SlotStartTime, cancelledSlotModel.SlotStartTime);
            Assert.AreEqual(slotCancelledIntegrationEvent.SlotEndTime, cancelledSlotModel.SlotEndTime);
            Assert.AreEqual(slotCancelledIntegrationEvent.SlotDuration, cancelledSlotModel.SlotDuration);
        }

        private CancelledSlotModel CreateValidCancelledSlotModel()
        {
            return new CancelledSlotModel()
            {
                Title = Title,
                Country = Country,
                SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone),
                SlotStartTime = ValidSlotStartTime,
                SlotEndTime = ValidSlotEndTime,
            };
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
