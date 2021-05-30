using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.ViewModels;
using FluentValidation;
using FluentValidation.Results;
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
        private const string ValidEmail = "ValidEmail";

        private const string InValidFirstName = "InValidFirstName12212";
        private const string InValidLastName = "InValidLastName121212";
        private const string InValidGender = "InValidGender232323";
        private const string InValidProfileSettings = "InValidProfileSettings";

        private ProfileSettingsController profileSettingsController;
        private Mock<IProfileSettingsBusiness> profileSettingsBusinessMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<IValidator<ProfileSettingsViewModel>> profileSettingsViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            profileSettingsBusinessMock = new Mock<IProfileSettingsBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            profileSettingsViewModelValidatorMock = new Mock<IValidator<ProfileSettingsViewModel>>();
            profileSettingsController = new ProfileSettingsController(profileSettingsBusinessMock.Object, currentUserMock.Object,
                profileSettingsViewModelValidatorMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = ValidFirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetProfileSettings_ReturnsSuccessResponse()
        {
            Response<ProfileSettingsModel> profileSettingsBusinessMockResponse = new Response<ProfileSettingsModel>() { Result = DefaultValidProfileSettingModel() };
            profileSettingsBusinessMock.Setup(a => a.GetProfileSettingsByCustomerId(It.IsAny<string>())).Returns(Task.FromResult(profileSettingsBusinessMockResponse));

            var response = await profileSettingsController.Get();
            
            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var profileSettingsViewModel = objectResult.Value as ProfileSettingsViewModel;
            Assert.AreEqual(profileSettingsViewModel.FirstName, ValidFirstName);
            Assert.AreEqual(profileSettingsViewModel.LastName, ValidLastName);
            Assert.AreEqual(profileSettingsViewModel.Gender, ValidGender);
            Assert.AreEqual(profileSettingsViewModel.Email, ValidEmail);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            profileSettingsBusinessMock.Verify((m => m.GetProfileSettingsByCustomerId(It.IsAny<string>())), Times.Once());
        }



        [Test]
        public async Task UpdateProfileSettings_InValidProfileSettings_ReturnsValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            profileSettingsViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<ProfileSettingsViewModel>())).Returns(new ValidationResult(validationFailures));

            var response = await profileSettingsController.Put(DefaultInValidProfileSettingViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(InValidProfileSettings));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Never());
            profileSettingsViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<ProfileSettingsViewModel>())), Times.Once());
        }

    

        [Test]
        public async Task UpdateProfileSettings_ValidProfileSettings_ReturnsSuccessResponse()
        {
            profileSettingsViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<ProfileSettingsViewModel>())).Returns(new ValidationResult());
            Response<bool> profileSettingsBusinessMockResponse = new Response<bool>() { Result = true };
            profileSettingsBusinessMock.Setup(a => a.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())).Returns(Task.FromResult(profileSettingsBusinessMockResponse));
            
            var response = await profileSettingsController.Put(DefaultValidProfileSettingViewModel()); ;

            var objectResult = response as NoContentResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status204NoContent);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            profileSettingsBusinessMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Once());
            profileSettingsViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<ProfileSettingsViewModel>())), Times.Once());
        }

        private ProfileSettingsModel DefaultValidProfileSettingModel()
        {
            return new ProfileSettingsModel()
            {
                FirstName = ValidFirstName,
                LastName = ValidLastName,
                Gender = ValidGender,
                Email = ValidEmail
            };
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

        private static List<ValidationFailure> CreateDefaultValidationFailure()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InValidProfileSettings);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }



    }
}
