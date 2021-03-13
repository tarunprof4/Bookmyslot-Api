using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Location.Contracts.Configuration;
using Bookmyslot.Api.Location.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class CustomerSettingsBusinessTests
    {
        private const string CUSTOMERID = "customerid";
        private const string ValidTimeZone = "Asia/Kolkata";
        private const string ValidTimeZoneCountry = "India";
        private const string InvalidTimeZone = "InvalidTimeZone";

        private Mock<ICustomerSettingsRepository> customerSettingsRepositoryMock;
        private Mock<INodaTimeZoneLocationBusiness> nodaTimeZoneLocationBusinessMock;
        private CustomerSettingsBusiness customerSettingsBusiness;

        [SetUp]
        public void SetUp()
        {
            this.customerSettingsRepositoryMock = new Mock<ICustomerSettingsRepository>();
            this.nodaTimeZoneLocationBusinessMock = new Mock<INodaTimeZoneLocationBusiness>();
            this.customerSettingsBusiness = new CustomerSettingsBusiness(this.customerSettingsRepositoryMock.Object, this.nodaTimeZoneLocationBusinessMock.Object);
        }

        [Test]
        public async Task UpdateCustomerAdditionalInformation_InvalidTimeZone_ReturnsValidationErrorResponse()
        {
            this.nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());
            
            var customerAdditionalInformationModelResponse = await this.customerSettingsBusiness.UpdateCustomerSettings(CUSTOMERID, DefaultInValidCustomerSettingsModel());

            Assert.AreEqual(customerAdditionalInformationModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(customerAdditionalInformationModelResponse.Messages.First(), AppBusinessMessagesConstants.InValidTimeZone);
            this.customerSettingsRepositoryMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
        }




        [Test]
        public async Task UpdateCustomerAdditionalInformation_ValidTimeZone_ReturnsSuccessResponse()
        {
            this.nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());
            var updateCustomerAdditionInformationResponse = new Response<bool>() { Result = true };
            this.customerSettingsRepositoryMock.Setup(a => a.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())).Returns(Task.FromResult(updateCustomerAdditionInformationResponse));
            
            var customerAdditionalInformationModelResponse = await this.customerSettingsBusiness.UpdateCustomerSettings(CUSTOMERID, DefaultValidCustomerSettingsModel());

            Assert.AreEqual(customerAdditionalInformationModelResponse.ResultType, ResultType.Success);
            this.customerSettingsRepositoryMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Once());
        }


        private NodaTimeZoneLocationConfiguration DefaultNodaTimeLocationConfiguration()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();
            zoneWithCountryId.Add(ValidTimeZone, ValidTimeZoneCountry);
            var countries = zoneWithCountryId.Values.Distinct().ToList();
            return new NodaTimeZoneLocationConfiguration(zoneWithCountryId, countries);
        }

        private CustomerSettingsModel DefaultValidCustomerSettingsModel()
        {
            return new CustomerSettingsModel() { TimeZone = ValidTimeZone };
        }

        private CustomerSettingsModel DefaultInValidCustomerSettingsModel()
        {
            return new CustomerSettingsModel() { TimeZone = InvalidTimeZone };
        }

    }
}