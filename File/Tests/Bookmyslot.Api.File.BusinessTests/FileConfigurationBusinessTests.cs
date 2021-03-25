using Bookmyslot.Api.File.Business;
using NUnit.Framework;

namespace Bookmyslot.Api.File.BusinessTests
{
    public class FileConfigurationBusinessTests
    {

        private FileConfigurationBusiness fileConfigurationBusiness;

        [SetUp]
        public void Setup()
        {
            fileConfigurationBusiness = new FileConfigurationBusiness();
        }

        [Test]
        public void CreateImageConfigurationSingleton_ReturnsImageConfigurationSingleton()
        {
            fileConfigurationBusiness.CreateImageConfigurationInformation();
            var instance = fileConfigurationBusiness.GetImageConfigurationInformation();

            Assert.IsNotNull(instance);
        }
    }
}