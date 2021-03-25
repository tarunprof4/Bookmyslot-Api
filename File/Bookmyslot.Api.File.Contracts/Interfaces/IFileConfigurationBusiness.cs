using Bookmyslot.Api.File.Contracts.Configuration;

namespace Bookmyslot.Api.File.Contracts.Interfaces
{

    public interface IFileConfigurationBusiness
    {
        ImageConfigurationSingleton GetImageConfigurationInformation();

        void CreateImageConfigurationInformation();
    }
}
