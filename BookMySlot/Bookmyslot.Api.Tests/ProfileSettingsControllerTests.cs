using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
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
        private Mock<IFileBusiness> fileBusinessMock;

        [SetUp]
        public void Setup()
        {
            profileSettingsBusinessMock = new Mock<IProfileSettingsBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            fileBusinessMock = new Mock<IFileBusiness>();
            profileSettingsController = new ProfileSettingsController(profileSettingsBusinessMock.Object, currentUserMock.Object, fileBusinessMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
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
        public async Task UpdateProfilePicture_InValidProfilePicture_ReturnsValidationResponse()
        {
            Response<bool> fileBusinessMockResponse = new Response<bool>() { ResultType = ResultType.ValidationError };
            fileBusinessMock.Setup(a => a.IsImageValid(It.IsAny<IFormFile>())).Returns(fileBusinessMockResponse);

            var response = await profileSettingsController.UploadProfilePicture(null);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            fileBusinessMock.Verify((m => m.IsImageValid(It.IsAny<IFormFile>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task UpdateProfilePicture_ValidProfilePicture_ReturnsSuccessResponse()
        {
            Response<bool> fileBusinessMockResponse = new Response<bool>() { Result = true };
            fileBusinessMock.Setup(a => a.IsImageValid(It.IsAny<IFormFile>())).Returns(fileBusinessMockResponse);
            Response<string> profileSettingsBusinessMockResponse = new Response<string>() { Result = "Uri" };
            profileSettingsBusinessMock.Setup(a => a.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>())).Returns(Task.FromResult(profileSettingsBusinessMockResponse));

            var response = await profileSettingsController.UploadProfilePicture(null);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status204NoContent);
            fileBusinessMock.Verify((m => m.IsImageValid(It.IsAny<IFormFile>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>())), Times.Once());
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

    }
}
