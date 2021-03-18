using System.Collections.Generic;

namespace Bookmyslot.Api.NodaTime.Contracts.Configuration
{
    public class NodaTimeZoneLocationConfiguration
    {
        public Dictionary<string, string> ZoneWithCountryId { get; }
        public List<string> Countries { get; }

        public NodaTimeZoneLocationConfiguration(Dictionary<string, string> zoneWithCountryId, List<string> countries)
        {
            this.ZoneWithCountryId = zoneWithCountryId;
            this.Countries = countries;
        }
    }
}
