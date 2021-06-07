using Bookmyslot.Api.Customers.Business.IntegrationEvents;
using Bookmyslot.Api.Customers.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Business.Tests.IntegrationEvents
{

    [TestFixture]
    public class RegisterCustomerIntegrationEventTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string EMAIL = "a@gmail.com";

        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void ValidateSocialCustomerLoginModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var registerCustomerModel = GetDefaultRegisterCustomerModel();
            var registerCustomerIntegrationEvent= new RegisterCustomerIntegrationEvent(registerCustomerModel);

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
