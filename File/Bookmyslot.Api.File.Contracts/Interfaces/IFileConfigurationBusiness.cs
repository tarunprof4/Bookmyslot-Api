using Bookmyslot.Api.File.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.File.Contracts.Interfaces
{
   
    public interface IFileConfigurationBusiness
    {
        ImageConfigurationSingleton GetImageConfigurationInformation();

        void CreateImageConfigurationInformation();
    }
}
