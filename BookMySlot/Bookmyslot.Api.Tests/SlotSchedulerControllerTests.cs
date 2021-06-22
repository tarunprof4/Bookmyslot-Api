using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class SlotSchedulerControllerTests
    {
        private const string InValidSlot = "InValidSlot";
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string InValidSlotSchedulerSlotModelKey = "InValidSlotSchedulerSlotModelKey";
        private const string ValidSlotSchedulerSlotModelKey = "ValidSlotSchedulerSlotModelKey";

        private SlotSchedulerController slotSchedulerController;
        private Mock<ISlotSchedulerBusiness> slotSchedulerBusinessMock;
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<IValidator<SlotSchedulerViewModel>> slotSchedulerViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            slotSchedulerBusinessMock = new Mock<ISlotSchedulerBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            slotSchedulerViewModelValidatorMock = new Mock<IValidator<SlotSchedulerViewModel>>();
            slotSchedulerController = new SlotSchedulerController(slotSchedulerBusinessMock.Object, symmetryEncryptionMock.Object,
                currentUserMock.Object, slotSchedulerViewModelValidatorMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task ScheduleSlot_InValidSlotSchedulerInformationModel_ReturnsValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            slotSchedulerViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotSchedulerViewModel>())).Returns(new ValidationResult(validationFailures));
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await slotSchedulerController.Post(new SlotSchedulerViewModel() { SlotModelKey = InValidSlotSchedulerSlotModelKey });

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(InValidSlot));
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
            slotSchedulerViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotSchedulerViewModel>())), Times.Once());
        }


        [Test]
        public async Task ScheduleSlot_CorruptSlotSchedulerInformationModel_ReturnsValidationResponse()
        {
            slotSchedulerViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotSchedulerViewModel>())).Returns(new ValidationResult());
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await slotSchedulerController.Post(new SlotSchedulerViewModel() { SlotModelKey = InValidSlotSchedulerSlotModelKey });

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
            slotSchedulerViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotSchedulerViewModel>())), Times.Once());
        }



        [Test]
        public async Task ScheduleSlot_ValidSlotSchedulerInformationModel_ReturnsSuccessResponse()
        {
            slotSchedulerViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotSchedulerViewModel>())).Returns(new ValidationResult());
            var slotSchedulerViewModel = new SlotSchedulerViewModel() { SlotModelKey = ValidSlotSchedulerSlotModelKey };
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(slotSchedulerViewModel));
            Response<bool> slotSchedulerBusinessMockResponse = new Response<bool>() { Result = true };
            slotSchedulerBusinessMock.Setup(a => a.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotSchedulerBusinessMockResponse));

            var response = await slotSchedulerController.Post(new SlotSchedulerViewModel() { SlotModelKey = InValidSlotSchedulerSlotModelKey });

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Once());
            slotSchedulerViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotSchedulerViewModel>())), Times.Once());
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
