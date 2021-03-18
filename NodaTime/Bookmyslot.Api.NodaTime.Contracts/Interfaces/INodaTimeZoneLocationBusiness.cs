
using Bookmyslot.Api.NodaTime.Contracts.Configuration;

namespace Bookmyslot.Api.NodaTime.Interfaces
{
    public interface INodaTimeZoneLocationBusiness
    {
        NodaTimeZoneLocationConfigurationSingleton GetNodaTimeZoneLocationInformation();

        void CreateNodaTimeZoneLocationInformation();
    }
}