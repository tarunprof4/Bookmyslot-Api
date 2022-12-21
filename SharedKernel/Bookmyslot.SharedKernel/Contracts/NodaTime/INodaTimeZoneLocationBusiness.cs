using Bookmyslot.SharedKernel.NodaTime.Configuration;

namespace Bookmyslot.SharedKernel.Contracts.NodaTime
{
    public interface INodaTimeZoneLocationBusiness
    {
        NodaTimeZoneLocationConfigurationSingleton GetNodaTimeZoneLocationInformation();

        void CreateNodaTimeZoneLocationInformation();
    }
}