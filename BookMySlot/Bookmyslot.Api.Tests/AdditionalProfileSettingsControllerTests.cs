using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
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
    public class AdditionalProfileSettingsControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string ValidFirstName = "ValidFirstName";
        private const string ValidBioHeadLine = "ValidBioHeadLine";
        private AdditionalProfileSettingsController additionalProfileSettingsController;
        private Mock<IAdditionalProfileSettingsBusiness> additionalProfileSettingsBusinessMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<IValidator<AdditionalProfileSettingsViewModel>> additionalProfileSettingsViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            additionalProfileSettingsBusinessMock = new Mock<IAdditionalProfileSettingsBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            additionalProfileSettingsViewModelValidatorMock = new Mock<IValidator<AdditionalProfileSettingsViewModel>>();
            additionalProfileSettingsController = new AdditionalProfileSettingsController(additionalProfileSettingsBusinessMock.Object,
                currentUserMock.Object, additionalProfileSettingsViewModelValidatorMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>()
            {
                Result = new CurrentUserModel()
                {
                    Id = CustomerId,
                    FirstName = ValidFirstName,
                    BioHeadLine = ValidBioHeadLine
                }
            };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetAdditionalProfileSettings_ReturnsSuccessResponse()
        {
            var response = await additionalProfileSettingsController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var additionalProfileSettingsViewModel = objectResult.Value as AdditionalProfileSettingsViewModel;
            Assert.AreEqual(additionalProfileSettingsViewModel.BioHeadLine, ValidBioHeadLine);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
        }


        [Test]
        public async Task UpdateAdditionalProfileSettings_InValidAdditionalProfileSettings_ReturnsValidationResponse()
        {
            ValidationFailure validationFailure = new ValidationFailure("", AppBusinessMessagesConstants.AdditionalProfileSettingDetailsMissing);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            additionalProfileSettingsViewModelValidatorMock.Setup(a => a.Validate(null)).Returns(new ValidationResult(validationFailures));

            var response = await additionalProfileSettingsController.Put(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.AdditionalProfileSettingDetailsMissing));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            additionalProfileSettingsBusinessMock.Verify((m => m.UpdateAdditionalProfileSettings(It.IsAny<string>(), It.IsAny<AdditionalProfileSettingsModel>())), Times.Never());
            additionalProfileSettingsViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<AdditionalProfileSettingsViewModel>())), Times.Once());
        }



        [Test]
        public async Task UpdateAdditionalProfileSettings_ValidAdditionalProfileSettings_ReturnsSuccessResponse()
        {
            Response<bool> additionalProfileSettingsBusinessMockResponse = new Response<bool>() { Result = true };
            additionalProfileSettingsBusinessMock.Setup(a => a.UpdateAdditionalProfileSettings(It.IsAny<string>(), It.IsAny<AdditionalProfileSettingsModel>())).Returns(Task.FromResult(additionalProfileSettingsBusinessMockResponse));
            additionalProfileSettingsViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<AdditionalProfileSettingsViewModel>())).Returns(new ValidationResult());

            var response = await additionalProfileSettingsController.Put(DefaultValidAdditionalProfileSettingViewModel()); ;

            var objectResult = response as NoContentResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status204NoContent);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            additionalProfileSettingsBusinessMock.Verify((m => m.UpdateAdditionalProfileSettings(It.IsAny<string>(), It.IsAny<AdditionalProfileSettingsModel>())), Times.Once());
            additionalProfileSettingsViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<AdditionalProfileSettingsViewModel>())), Times.Once());
        }






        private AdditionalProfileSettingsViewModel DefaultValidAdditionalProfileSettingViewModel()
        {
            return new AdditionalProfileSettingsViewModel(ValidBioHeadLine)
            {
            };
        }
    }
}
