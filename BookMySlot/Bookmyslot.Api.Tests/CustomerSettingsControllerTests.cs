using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.NodaTime.Contracts.Configuration;
using Bookmyslot.Api.NodaTime.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class CustomerSettingsControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string InValidTimeZone = "InValidTimeZone";
        private const string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private const string ValidTimeZoneCountry = "India";
        private readonly string ValidSlotDate = DateTime.UtcNow.AddDays(2).ToString(DateTimeConstants.ApplicationDatePattern);

        private CustomerSettingsController customerSettingsController;
        private Mock<ICustomerSettingsBusiness> customerSettingsBusinessMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<INodaTimeZoneLocationBusiness> nodaTimeZoneLocationBusinessMock;

        [SetUp]
        public void Setup()
        {
            customerSettingsBusinessMock = new Mock<ICustomerSettingsBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            nodaTimeZoneLocationBusinessMock = new Mock<INodaTimeZoneLocationBusiness>();
            customerSettingsController = new CustomerSettingsController(customerSettingsBusinessMock.Object, currentUserMock.Object, nodaTimeZoneLocationBusinessMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
            nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());
        }


        [Test]
        public async Task GetCustomerSettings_ReturnsSuccessResponse()
        {
            Response<CustomerSettingsModel> customerSettingsMockResponse = new Response<CustomerSettingsModel>() { Result = new CustomerSettingsModel() };
            customerSettingsBusinessMock.Setup(a => a.GetCustomerSettings(It.IsAny<string>())).Returns(Task.FromResult(customerSettingsMockResponse));

            var response = await customerSettingsController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
        }


        [Test]
        public async Task UpdateCustomerSettings_NullCustomerSettingsViewModel_ReturnsValidationResponse()
        {
            var response = await customerSettingsController.Put(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CustomerSettingsMissing));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
        }


        [Test()]
        public async Task UpdateCustomerSettings_EmptyCustomerSettingsViewModel_ReturnsValidationResponse()
        {
            var response = await customerSettingsController.Put(new CustomerSettingsViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.TimeZoneRequired));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
        }

        [TestCase("   ", AppBusinessMessagesConstants.TimeZoneRequired)]
        [TestCase(InValidTimeZone, AppBusinessMessagesConstants.InValidTimeZone)]
        public async Task UpdateCustomerSettings_InValidCustomerSettingsViewModel_ReturnsValidationResponse(string timeZone, string validationMessage)
        {
            var response = await customerSettingsController.Put(new CustomerSettingsViewModel() { TimeZone = timeZone });

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(validationMessage));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
        }



        [Test]
        public async Task UpdateCustomerSettings_ValidCustomerSettingsViewModel_ReturnsValidationResponse()
        {
            Response<bool> customerSettingsMockResponse = new Response<bool>() { Result = true };
            customerSettingsBusinessMock.Setup(a => a.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())).Returns(Task.FromResult(customerSettingsMockResponse));

            var response = await customerSettingsController.Put(new CustomerSettingsViewModel() { TimeZone = ValidTimeZone });

            var objectResult = response as NoContentResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status204NoContent);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Once());
        }

        private NodaTimeZoneLocationConfigurationSingleton DefaultNodaTimeLocationConfiguration()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();
            zoneWithCountryId.Add(ValidTimeZone, ValidTimeZoneCountry);
            var countries = zoneWithCountryId.Values.Distinct().ToDictionary(x=>x, x=>x);
            NodaTimeZoneLocationConfigurationSingleton.CreateInstance(zoneWithCountryId, countries);
            return NodaTimeZoneLocationConfigurationSingleton.GetInstance();
        }
    }
}
