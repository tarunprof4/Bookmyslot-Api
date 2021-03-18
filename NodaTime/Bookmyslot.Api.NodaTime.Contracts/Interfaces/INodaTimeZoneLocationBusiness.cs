
using Bookmyslot.Api.NodaTime.Contracts.Configuration;

namespace Bookmyslot.Api.NodaTime.Interfaces
{
    public interface INodaTimeZoneLocationBusiness
    {
        NodaTimeZoneLocationConfiguration GetNodaTimeZoneLocationInformation();
    }
}