using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Moq;
using NUnit.Framework;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class CustomerSettingsBusinessTests
    {
        private const string CUSTOMERID = "customerid";
        private const string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private const string ValidTimeZoneCountry = CountryConstants.India;
        private const string InvalidTimeZone = "InvalidTimeZone";

        private Mock<ICustomerSettingsRepository> customerSettingsRepositoryMock;
        private CustomerSettingsBusiness customerSettingsBusiness;

        [SetUp]
        public void SetUp()
        {
            this.customerSettingsRepositoryMock = new Mock<ICustomerSettingsRepository>();
            this.customerSettingsBusiness = new CustomerSettingsBusiness(this.customerSettingsRepositoryMock.Object);
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