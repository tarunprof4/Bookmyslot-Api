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
    public class CustomerSettingsControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string InValidCountry = "InValidCountry";
        private const string InValidTimeZone = "InValidTimeZone";
        private const string ValidCountry = CountryConstants.India;
        private const string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private const string ValidTimeZoneCountry = CountryConstants.India;

        private const string InValidCustomerSettings = "InValidCustomerSettings";

        private CustomerSettingsController customerSettingsController;
        private Mock<ICustomerSettingsBusiness> customerSettingsBusinessMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<IValidator<CustomerSettingsViewModel>> customerSettingsViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            customerSettingsBusinessMock = new Mock<ICustomerSettingsBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            customerSettingsViewModelValidatorMock = new Mock<IValidator<CustomerSettingsViewModel>>();
            customerSettingsController = new CustomerSettingsController(customerSettingsBusinessMock.Object, currentUserMock.Object,
                customerSettingsViewModelValidatorMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }


        [Test]
        public async Task GetCustomerSettings_ReturnsSuccessResponse()
        {
            Response<CustomerSettingsModel> customerSettingsMockResponse = new Response<CustomerSettingsModel>() { Result = DefaultCustomerSettingsModel() };
            customerSettingsBusinessMock.Setup(a => a.GetCustomerSettings(It.IsAny<string>())).Returns(Task.FromResult(customerSettingsMockResponse));

            var response = await customerSettingsController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var customerSettingsViewModel = objectResult.Value as CustomerSettingsViewModel;
            Assert.AreEqual(customerSettingsViewModel.Country, ValidCountry);
            Assert.AreEqual(customerSettingsViewModel.TimeZone, ValidTimeZone);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
        }


        [Test]
        public async Task UpdateCustomerSettings_InValidCustomerSettingsViewModel_ReturnsValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            customerSettingsViewModelValidatorMock.Setup(a => a.Validate(null)).Returns(new ValidationResult(validationFailures));

            var response = await customerSettingsController.Put(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(InValidCustomerSettings));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Never());
            customerSettingsViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<CustomerSettingsViewModel>())), Times.Once());
        }

    



        [Test]
        public async Task UpdateCustomerSettings_ValidCustomerSettingsViewModel_ReturnsValidationResponse()
        {
            customerSettingsViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<CustomerSettingsViewModel>())).Returns(new ValidationResult());
            Response<bool> customerSettingsMockResponse = new Response<bool>() { Result = true };
            customerSettingsBusinessMock.Setup(a => a.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())).Returns(Task.FromResult(customerSettingsMockResponse));
            
            var response = await customerSettingsController.Put(new CustomerSettingsViewModel() { Country = ValidCountry, TimeZone = ValidTimeZone });

            var objectResult = response as NoContentResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status204NoContent);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSettingsBusinessMock.Verify((m => m.UpdateCustomerSettings(It.IsAny<string>(), It.IsAny<CustomerSettingsModel>())), Times.Once());
            customerSettingsViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<CustomerSettingsViewModel>())), Times.Once());
        }

     

        private CustomerSettingsModel DefaultCustomerSettingsModel()
        {
            return new CustomerSettingsModel()
            {
                Country = ValidCountry,
                TimeZone = ValidTimeZone
            };
        }

        private static List<ValidationFailure> CreateDefaultValidationFailure()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InValidCustomerSettings);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }


    }
}
