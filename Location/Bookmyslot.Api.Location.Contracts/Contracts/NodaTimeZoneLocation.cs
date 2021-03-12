using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.Location.Contracts
{
    public class NodaTimeZoneLocation
    {
        public Dictionary<string, string> ZoneWithCountryId { get; }
        public List<string> Countries { get; }

        public NodaTimeZoneLocation(Dictionary<string, string> zoneWithCountryId, List<string> countries)
        {
            this.ZoneWithCountryId = zoneWithCountryId;
            this.Countries = countries;
        }
    }
}
