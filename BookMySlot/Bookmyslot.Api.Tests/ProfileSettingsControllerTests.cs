using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.File.Contracts.Configuration;
using Bookmyslot.Api.File.Contracts.Constants;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class ProfileSettingsControllerTests
    {
        private const string CustomerId = "CustomerId";

        private const string ValidFirstName = "ValidFirstName";
        private const string ValidLastName = "ValidLastName";
        private const string ValidGender = "ValidGender";

        private const string InValidFirstName = "InValidFirstName12212";
        private const string InValidLastName = "InValidLastName121212";
        private const string InValidGender = "InValidGender232323";

        private ProfileSettingsController profileSettingsController;
        private Mock<IProfileSettingsBusiness> profileSettingsBusinessMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<IFileConfigurationBusiness> fileConfigurationBusinessMock;

        [SetUp]
        public void Setup()
        {
            profileSettingsBusinessMock = new Mock<IProfileSettingsBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            fileConfigurationBusinessMock = new Mock<IFileConfigurationBusiness>();
            profileSettingsController = new ProfileSettingsController(profileSettingsBusinessMock.Object, currentUserMock.Object, fileConfigurationBusinessMock.Object);

            Response<CustomerAuthModel> currentUserMockResponse = new Response<CustomerAuthModel>() { Result = new CustomerAuthModel() { Id = CustomerId, FirstName = ValidFirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetProfileSettings_ReturnsSuccessResponse()
        {
            Response<ProfileSettingsModel> profileSettingsBusinessMockResponse = new Response<ProfileSettingsModel>() { Result = new ProfileSettingsModel() };
            profileSettingsBusinessMock.Setup(a => a.GetProfileSettingsByCustomerId(It.IsAny<string>())).Returns(Task.FromResult(profileSettingsBusinessMockResponse));

            var response = await profileSettingsController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            profileSettingsBusinessMock.Verify((m => m.GetProfileSettingsByCustomerId(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task UpdateProfileSettings_NullProfileSettings_ReturnsValidationResponse()
        {
            var response = await profileSettingsController.Put(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.ProfileSettingDetailsMissing));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task UpdateProfileSettings_EmptyProfileSettings_ReturnsValidationResponse()
        {
            var response = await profileSettingsController.Put(new ProfileSettingsViewModel()); ;

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.FirstNameRequired));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.LastNameRequired));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.GenderRequired));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Never());
        }





        [Test]
        public async Task UpdateProfileSettings_InValidProfileSettings_ReturnsValidationResponse()
        {
            var response = await profileSettingsController.Put(DefaultInValidProfileSettingViewModel()); ;

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.FirstNameInValid));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.LastNameInValid));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.GenderNotValid));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task UpdateProfileSettings_ValidProfileSettings_ReturnsSuccessResponse()
        {
            Response<bool> profileSettingsBusinessMockResponse = new Response<bool>() { Result = true };
            profileSettingsBusinessMock.Setup(a => a.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())).Returns(Task.FromResult(profileSettingsBusinessMockResponse));

            var response = await profileSettingsController.Put(DefaultValidProfileSettingViewModel()); ;

            var objectResult = response as NoContentResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status204NoContent);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task UpdateProfilePicture_EmptyFile_ReturnsValidationResponse()
        {
            fileConfigurationBusinessMock.Setup(a => a.GetImageConfigurationInformation()).Returns(DefaultImageConfigurationSingleton());

            var response = await profileSettingsController.UpdateProfilePicture(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.FileMissing));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task UpdateProfilePicture_InValidProfilePicture_ReturnsValidationResponse()
        {
            fileConfigurationBusinessMock.Setup(a => a.GetImageConfigurationInformation()).Returns(DefaultImageConfigurationSingleton());
            Response<string> profileSettingsBusinessMockResponse = new Response<string>() { Result = "Uri" };
            profileSettingsBusinessMock.Setup(a => a.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(profileSettingsBusinessMockResponse));

            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            var response = await profileSettingsController.UpdateProfilePicture(file);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
        }




        private ProfileSettingsViewModel DefaultValidProfileSettingViewModel()
        {
            return new ProfileSettingsViewModel()
            {
                FirstName = ValidFirstName,
                LastName = ValidLastName,
                Gender = ValidGender
            };
        }

        private ProfileSettingsViewModel DefaultInValidProfileSettingViewModel()
        {
            return new ProfileSettingsViewModel()
            {
                FirstName = InValidFirstName,
                LastName = InValidLastName,
                Gender = InValidGender
            };
        }


        private ImageConfigurationSingleton DefaultImageConfigurationSingleton()
        {
            Dictionary<string, string> imageExtensions = CreateImageExtensions();
            Dictionary<string, List<byte[]>> imageExtensionsSignatures = CreateImageExtensionSignatures();

            ImageConfigurationSingleton.CreateInstance(imageExtensions, imageExtensionsSignatures);
            return ImageConfigurationSingleton.GetInstance();
        }



        private Dictionary<string, string> CreateImageExtensions()
        {
            var extensions = new Dictionary<string, string>();
            extensions.Add(ImageConstants.Jpeg, ImageConstants.Jpeg);
            extensions.Add(ImageConstants.Jpg, ImageConstants.Jpg);
            extensions.Add(ImageConstants.Png, ImageConstants.Png);
            extensions.Add(ImageConstants.Gif, ImageConstants.Gif);
            return extensions;
        }

        private Dictionary<string, List<byte[]>> CreateImageExtensionSignatures()
        {
            Dictionary<string, List<byte[]>> fileExtensionSignatures = new Dictionary<string, List<byte[]>>
{
    { ImageConstants.Jpeg, new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
        }
    },
    { ImageConstants.Jpg, new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
        }
    },
      { ImageConstants.Gif, new List<byte[]>
        {
            new byte[] { 0x47, 0x49, 0x46, 0x38 },
        }
    },

      { ImageConstants.Png, new List<byte[]>
        {
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
        }
    },
};

            return fileExtensionSignatures;
        }

    }
}
