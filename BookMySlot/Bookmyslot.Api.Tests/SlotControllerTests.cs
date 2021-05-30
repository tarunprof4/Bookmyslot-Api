using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class SlotControllerTests
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
        private const string InValidSlot = "InValidSlot";

        private readonly string ValidSlotDate = DateTime.UtcNow.AddDays(2).ToString(DateTimeConstants.ApplicationDatePattern);
        private const string ValidSlotKey= "ValidSlotKey";

        private SlotController slotController;
        private Mock<ISlotBusiness> slotBusinessMock;
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<ISlotRequestAdaptor> slotRequestAdaptorMock;
        private Mock<IValidator<SlotViewModel>> slotViewModelValidatorMock;
        private Mock<IValidator<CancelSlotViewModel>> cancelSlotViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            slotBusinessMock = new Mock<ISlotBusiness>();
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            currentUserMock = new Mock<ICurrentUser>();
            slotRequestAdaptorMock = new Mock<ISlotRequestAdaptor>();

            slotViewModelValidatorMock = new Mock<IValidator<SlotViewModel>>();
            cancelSlotViewModelValidatorMock = new Mock<IValidator<CancelSlotViewModel>>();
            slotController = new SlotController(slotBusinessMock.Object, symmetryEncryptionMock.Object, currentUserMock.Object,
                slotRequestAdaptorMock.Object, slotViewModelValidatorMock.Object, cancelSlotViewModelValidatorMock.Object);

            
            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

      
        [Test]
        public async Task CreateSlot_InValidSlotViewModel_ReturnsValidationResponse()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InValidSlot);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            slotViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotViewModel>())).Returns(new ValidationResult(validationFailures));

            var postResponse = await slotController.Post(DefaultInValidSlotViewModel());

            var objectResult = postResponse as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(InValidSlot));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotBusinessMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
            slotViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotViewModel>())), Times.Once());
        }



        [Test]
        public async Task CreateSlot_ValidSlotViewModel_ReturnsSuccessResponse()
        {
            slotViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotViewModel>())).Returns(new ValidationResult());
            var guid = Guid.NewGuid().ToString();
            Response<string> slotBusinessMockResponse = new Response<string>() { Result = guid };
            slotBusinessMock.Setup(a => a.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotBusinessMockResponse));

            var postResponse = await slotController.Post(DefaultValidSlotViewModel());
            
            var objectResult = postResponse as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            Assert.AreEqual(objectResult.Value, guid);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotBusinessMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Once());
            slotViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotViewModel>())), Times.Once());
        }

        [Test]
        public async Task CancelSlot_InValidCancelSlotViewModel_ReturnsValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            cancelSlotViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<CancelSlotViewModel>())).Returns(new ValidationResult(validationFailures));
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await slotController.CancelSlot(new CancelSlotViewModel() { SlotKey = InValidSlotKey });

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(InValidSlot));
            symmetryEncryptionMock.Verify(a => a.Decrypt(It.IsAny<string>()), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotBusinessMock.Verify((m => m.CancelSlot(It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            cancelSlotViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<CancelSlotViewModel>())), Times.Once());
        }

        [Test]
        public async Task CancelSlot_CorruptCancelSlotViewModel_ReturnsValidationResponse()
        {
            cancelSlotViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<CancelSlotViewModel>())).Returns(new ValidationResult());
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await slotController.CancelSlot(new CancelSlotViewModel() { SlotKey = InValidSlotKey });

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            symmetryEncryptionMock.Verify(a => a.Decrypt(It.IsAny<string>()), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotBusinessMock.Verify((m => m.CancelSlot(It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            cancelSlotViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<CancelSlotViewModel>())), Times.Once());
        }

       
        [Test]
        public async Task CancelSlot_ValidCancelSlotViewModel_ReturnsValidationResponse()
        {
            cancelSlotViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<CancelSlotViewModel>())).Returns(new ValidationResult());
            var cancelSlotViewModel = new CancelSlotViewModel() { SlotKey = ValidSlotKey };
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(cancelSlotViewModel));
            Response<bool> slotBusinessMockResponse = new Response<bool>() { Result = true };
            slotBusinessMock.Setup(a => a.CancelSlot(It.IsAny<string>(),It.IsAny<string>())).Returns(Task.FromResult(slotBusinessMockResponse));

            var response = await slotController.CancelSlot(cancelSlotViewModel);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            symmetryEncryptionMock.Verify(a => a.Decrypt(It.IsAny<string>()), Times.Once());
            slotBusinessMock.Verify((m => m.CancelSlot(It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            cancelSlotViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<CancelSlotViewModel>())), Times.Once());
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

     


        private static List<ValidationFailure> CreateDefaultValidationFailure()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InValidSlot);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }

    }
}