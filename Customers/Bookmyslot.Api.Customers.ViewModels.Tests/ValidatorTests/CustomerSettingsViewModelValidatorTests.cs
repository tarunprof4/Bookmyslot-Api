using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using Bookmyslot.Api.NodaTime.Contracts.Configuration;
using Bookmyslot.Api.NodaTime.Interfaces;
using FluentValidation;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookmyslot.Api.Customers.ViewModels.Tests.ValidatorTests
{
   
    [TestFixture]
    public class CustomerSettingsViewModelValidatorTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string InValidCountry = "InValidCountry";
        private const string InValidTimeZone = "InValidTimeZone";
        private const string ValidCountry = CountryConstants.India;
        private const string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private const string ValidTimeZoneCountry = CountryConstants.India;

        private IValidator<CustomerSettingsViewModel> validator;
        private Mock<INodaTimeZoneLocationBusiness> nodaTimeZoneLocationBusinessMock;

        [SetUp]
        public void Setup()
        {
            nodaTimeZoneLocationBusinessMock = new Mock<INodaTimeZoneLocationBusiness>();
            nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());
            validator = new CustomerSettingsViewModelValidator(nodaTimeZoneLocationBusinessMock.);
        }


        [Test]
        public void ValidateCustomerSettingsViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.CustomerSettingsMissing));
        }


        [Test]
        public void ValidateCustomerSettingsViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(new CustomerSettingsViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.CountryRequired));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.TimeZoneRequired));
        }

        [Test]
        public void ValidateCustomerSettingsViewModel_InValidViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = validator.Validate(DefaultInValidCustomerSettingsModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.InValidCountry));
            Assert.IsTrue(validationErrorMessages.Contains(AppBusinessMessagesConstants.InValidTimeZone));
        }

        [Test]
        public void ValidateCustomerSettingsViewModel_ValidViewModel_ReturnSuccessResponse()
        {
            var validationResult = validator.Validate(DefaultValidCustomerSettingsModel());

            Assert.IsTrue(validationResult.IsValid);
        }

        private CustomerSettingsViewModel DefaultValidCustomerSettingsModel()
        {
            return new CustomerSettingsViewModel()
            {
                Country = ValidCountry,
                TimeZone = ValidTimeZone
            };
        }

        private CustomerSettingsViewModel DefaultInValidCustomerSettingsModel()
        {
            return new CustomerSettingsViewModel()
            {
                Country = InValidCountry,
                TimeZone = InValidTimeZone
            };
        }


        private NodaTimeZoneLocationConfigurationSingleton DefaultNodaTimeLocationConfiguration()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();
            zoneWithCountryId.Add(ValidTimeZone, ValidCountry);
            var countries = zoneWithCountryId.Values.Distinct().ToDictionary(x => x, x => x);

            NodaTimeZoneLocationConfigurationSingleton.CreateInstance(zoneWithCountryId, countries);
            return NodaTimeZoneLocationConfigurationSingleton.GetInstance();
        }

    }
}
