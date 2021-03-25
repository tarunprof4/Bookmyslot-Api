using Bookmyslot.Api.File.Business;
using Bookmyslot.Api.File.Contracts.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace Bookmyslot.Api.File.BusinessTests
{
    public class FileConfigurationBusinessTests
    {

        private FileConfigurationBusiness fileConfigurationBusiness;
        private ImageConfigurationSingleton instance;

        [SetUp]
        public void Setup()
        {
            fileConfigurationBusiness = new FileConfigurationBusiness();
            fileConfigurationBusiness.CreateImageConfigurationInformation();
            this.instance = fileConfigurationBusiness.GetImageConfigurationInformation();
        }

        [Test]
        public void CreateImageConfigurationSingleton_ReturnsImageConfigurationSingleton()
        {
            fileConfigurationBusiness.CreateImageConfigurationInformation();
            var instance = fileConfigurationBusiness.GetImageConfigurationInformation();

            Assert.IsNotNull(instance);


        }


        [Test]
        public void isImageExtensionValid_InValidExtension_ReturnsFalseResponse()
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            var isValid = instance.isImageExtensionValid(file);
            Assert.AreEqual(isValid, false);

        }

        [Test]
        public void isImageExtensionValid_ValidExtension_ReturnsTrueResponse()
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.jpeg");

            var isValid = instance.isImageExtensionValid(file);
            Assert.AreEqual(isValid, true);

        }


        [Test]
        public void isImageExtensionVSignaturealid_InValidExtension_ReturnsFalseResponse()
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            var isValid = instance.isImageExtensionSignatureValid(file);
            Assert.AreEqual(isValid, false);

        }
    }
}