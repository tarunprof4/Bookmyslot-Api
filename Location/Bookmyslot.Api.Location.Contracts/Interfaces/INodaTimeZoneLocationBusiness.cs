using Bookmyslot.Api.Location.Contracts.Configuration;

namespace Bookmyslot.Api.Location.Interfaces
{
    public interface INodaTimeZoneLocationBusiness
    {
        NodaTimeZoneLocationConfiguration GetNodaTimeZoneLocationInformation();
    }
}
