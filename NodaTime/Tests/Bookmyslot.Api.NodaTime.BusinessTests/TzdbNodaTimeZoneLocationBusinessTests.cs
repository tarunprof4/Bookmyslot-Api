using Bookmyslot.Api.NodaTime.Business;
using Bookmyslot.Api.NodaTime.Contracts.Configuration;
using Bookmyslot.SharedKernel.Constants;
using NUnit.Framework;

namespace Bookmyslot.Api.NodaTime.BusinessTests
{
    public class TzdbNodaTimeZoneLocationBusinessTests
    {
        private readonly string ValidCountry = CountryConstants.India;
        private readonly string InValidCountry = "InValidCountry";
        private readonly string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private readonly string InValidTimeZone = "InValidTimeZone";
        private TzdbNodaTimeZoneLocationBusiness tzdbNodaTimeZoneLocationBusiness;
        private NodaTimeZoneLocationConfigurationSingleton instance;

        [SetUp]
        public void Setup()
        {
            tzdbNodaTimeZoneLocationBusiness = new TzdbNodaTimeZoneLocationBusiness();
            tzdbNodaTimeZoneLocationBusiness.CreateNodaTimeZoneLocationInformation();
            this.instance = tzdbNodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();
        }

        [Test]
        public void CreateNodaTimeZoneLocationInformation_ReturnsSingletonNodaTimeZoneConfiguration()
        {
            tzdbNodaTimeZoneLocationBusiness.CreateNodaTimeZoneLocationInformation();
            var instance = tzdbNodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();

            Assert.IsNotNull(instance);
        }


        [Test]
        public void IsCountryValid_ValidCountry_ReturnsTrueResponse()
        {
            var isValid = this.instance.IsCountryValid(ValidCountry);
            Assert.AreEqual(isValid, true);
        }

        [Test]
        public void IsCountryValid_InValidCountry_ReturnsFalseResponse()
        {
            var isValid = this.instance.IsCountryValid(InValidCountry);
            Assert.AreEqual(isValid, false);
        }


        [Test]
        public void IsTimeZoneValid_ValidTimeZone_ReturnsTrueResponse()
        {
            var isValid = this.instance.IsTimeZoneValid(ValidTimeZone);
            Assert.AreEqual(isValid, true);
        }

        [Test]
        public void IsTimeZoneValid_InValidTimeZone_ReturnsFalseResponse()
        {
            var isValid = this.instance.IsTimeZoneValid(InValidTimeZone);
            Assert.AreEqual(isValid, false);
        }

    }
}