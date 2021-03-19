using Bookmyslot.Api.NodaTime.Contracts.Configuration;
using Bookmyslot.Api.NodaTime.Interfaces;
using NodaTime.TimeZones;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.NodaTime.Business
{
    public class TzdbNodaTimeZoneLocationBusiness : INodaTimeZoneLocationBusiness
    {
        public void CreateNodaTimeZoneLocationInformation()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();

            foreach (var zoneLocation in TzdbDateTimeZoneSource.Default.ZoneLocations)
            {
                zoneWithCountryId.Add(zoneLocation.ZoneId, zoneLocation.CountryName);
            }
            var countries = zoneWithCountryId.Values.Distinct().ToDictionary(x => x, x => x);

            NodaTimeZoneLocationConfigurationSingleton.CreateInstance(zoneWithCountryId, countries);
        }

        public NodaTimeZoneLocationConfigurationSingleton GetNodaTimeZoneLocationInformation()
        {
            return NodaTimeZoneLocationConfigurationSingleton.GetInstance();
        }
    }
}
