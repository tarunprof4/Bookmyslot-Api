using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using NUnit.Framework;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests.IntegrationEvents
{

    [TestFixture]
    public class SlotCancelledIntegrationEventTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string EMAIL = "a@gmail.com";

        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void CreateSlotCancelledIntegrationEvent()
        {
            var registerCustomerModel = GetDefaultRegisterCustomerModel();
            var slotCancelledIntegrationEvent = new SlotCancelledIntegrationEvent(registerCustomerModel);

            Assert.AreEqual(registerCustomerModel.FirstName, registerCustomerIntegrationEvent.FirstName);
            Assert.AreEqual(registerCustomerModel.LastName, registerCustomerIntegrationEvent.LastName);
            Assert.AreEqual(registerCustomerModel.Email, registerCustomerIntegrationEvent.Email);
        }


        private RegisterCustomerModel GetDefaultRegisterCustomerModel()
        {
            return new RegisterCustomerModel()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = EMAIL
            };
        }





    }
}
