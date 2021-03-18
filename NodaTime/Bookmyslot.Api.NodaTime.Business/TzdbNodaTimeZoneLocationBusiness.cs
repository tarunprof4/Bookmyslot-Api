using Bookmyslot.Api.NodaTime.Contracts.Configuration;
using Bookmyslot.Api.NodaTime.Interfaces;
using NodaTime.TimeZones;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.NodaTime.Business
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
