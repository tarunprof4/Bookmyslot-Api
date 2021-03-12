using Bookmyslot.Api.Location.Contracts.Configuration;
using Bookmyslot.Api.Location.Interfaces;
using NodaTime.TimeZones;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.Location.Business
{
    public class TzdbNodaTimeZoneLocationBusiness : INodaTimeZoneLocationBusiness
    {
        public NodaTimeZoneLocationConfiguration GetNodaTimeZoneLocationInformation()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();

            foreach (var zoneLocation in TzdbDateTimeZoneSource.Default.ZoneLocations)
            {
                zoneWithCountryId.Add(zoneLocation.ZoneId, zoneLocation.CountryName);
            }
            var countries = zoneWithCountryId.Values.Distinct().ToList();


            return new NodaTimeZoneLocationConfiguration(zoneWithCountryId, countries);
        }
    }
}
