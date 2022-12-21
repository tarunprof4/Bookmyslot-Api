using Bookmyslot.Api.NodaTime.Contracts.Configuration;
using Bookmyslot.Api.NodaTime.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using Bookmyslot.SharedKernel.Constants;
using FluentValidation;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.ValidatorTests
{

    [TestFixture]
    public class SlotViewModelValidatorTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string InValidSlotKey = "InValidSlotKey";
        private const string ValidSlotTitle = "SlotTitle";
        private const string InValidCountry = "Incountry";
        private const string InValidTimeZone = "InValidTimeZone";
        private const string InValidSlotDate = "31-31-2000";
        private const string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private const string ValidCountry = CountryConstants.India;
        private readonly string ValidSlotDate = DateTime.UtcNow.AddDays(2).ToString(DateTimeConstants.ApplicationDatePattern);


        private IValidator<SlotViewModel> slotViewModelValidator;
        private Mock<INodaTimeZoneLocationBusiness> nodaTimeZoneLocationBusinessMock;

        [SetUp]
        public void Setup()
        {
            nodaTimeZoneLocationBusinessMock = new Mock<INodaTimeZoneLocationBusiness>();
            nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());
            slotViewModelValidator = new SlotViewModelValidator(nodaTimeZoneLocationBusinessMock.Object);
        }


        [Test]
        public void ValidateSlotViewModel_NullViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = slotViewModelValidator.Validate(null);
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotDetailsMissing));
        }


        [Test]
        public void ValidateSlotViewModel_EmptyViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = slotViewModelValidator.Validate(new SlotViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotTitleRequired));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.CountryRequired));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.TimeZoneRequired));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotDateRequired));
        }


        [Test]
        public void ValidateSlotViewModel_InValidViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = slotViewModelValidator.Validate(DefaultInValidSlotViewModel());
            var validationErrorMessages = validationResult.Errors.Select(a => a.ErrorMessage).ToList();

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidCountry));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidTimeZone));
            Assert.IsTrue(validationErrorMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidSlotDate));
        }

        [Test]
        public void ValidateSlotViewModel_ValidViewModel_ReturnValidationErrorResponse()
        {
            var validationResult = slotViewModelValidator.Validate(DefaultValidSlotViewModel());

            Assert.IsTrue(validationResult.IsValid);
        }


        private SlotViewModel DefaultInValidSlotViewModel()
        {
            var slotviewModel = new SlotViewModel();
            slotviewModel.Country = InValidCountry;
            slotviewModel.TimeZone = InValidTimeZone;
            slotviewModel.SlotDate = InValidSlotDate;
            return slotviewModel;
        }

        private SlotViewModel DefaultValidSlotViewModel()
        {
            var slotviewModel = new SlotViewModel();
            slotviewModel.Country = ValidCountry;
            slotviewModel.Title = ValidSlotTitle;
            slotviewModel.TimeZone = ValidTimeZone;
            slotviewModel.SlotDate = ValidSlotDate;
            return slotviewModel;
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
