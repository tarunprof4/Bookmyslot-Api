using Bookmyslot.Api.File.Business;
using Bookmyslot.Api.File.Contracts.Configuration;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.File.BusinessTests
{
    public class FileBusinessTests
    {

        private IFileConfigurationBusiness fileConfigurationBusiness;
        private FileBusiness fileBusiness;

        [SetUp]
        public void Setup()
        {
            fileConfigurationBusiness = new FileConfigurationBusiness();
            fileConfigurationBusiness.CreateImageConfigurationInformation();
            fileBusiness = new FileBusiness(fileConfigurationBusiness);
        }

        [Test]
        public void IsImageValid_ReturnsImageConfigurationSingleton()
        {
            fileBusiness.CreateImageConfigurationInformation();
            var instance = fileBusiness.GetImageConfigurationInformation();

            Assert.IsNotNull(instance);
        }

        private ImageConfigurationSingleton DefaultImageConfigurationSingleton()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();
            zoneWithCountryId.Add(ValidTimeZone, ValidCountry);
            var countries = zoneWithCountryId.Values.Distinct().ToDictionary(x => x, x => x);

            ImageConfigurationSingleton.CreateInstance(zoneWithCountryId, countries);
            return ImageConfigurationSingleton.GetInstance();
        }

    }
}
