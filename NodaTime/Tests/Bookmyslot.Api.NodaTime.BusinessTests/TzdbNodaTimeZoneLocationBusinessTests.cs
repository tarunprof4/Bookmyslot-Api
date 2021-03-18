using Bookmyslot.Api.NodaTime.Business;
using NUnit.Framework;

namespace Bookmyslot.Api.NodaTime.BusinessTests
{
    public class TzdbNodaTimeZoneLocationBusinessTests
    {
        private TzdbNodaTimeZoneLocationBusiness tzdbNodaTimeZoneLocationBusiness;

        [SetUp]
        public void Setup()
        {
            tzdbNodaTimeZoneLocationBusiness = new TzdbNodaTimeZoneLocationBusiness();
        }

        [Test]
        public void CreateNodaTimeZoneLocationInformation_ReturnsSingletonNodaTimeZoneConfiguration()
        {
            tzdbNodaTimeZoneLocationBusiness.CreateNodaTimeZoneLocationInformation();
            var instance = tzdbNodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();

            Assert.IsNotNull(instance);
        }

    }
}