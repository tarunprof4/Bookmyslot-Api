using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

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