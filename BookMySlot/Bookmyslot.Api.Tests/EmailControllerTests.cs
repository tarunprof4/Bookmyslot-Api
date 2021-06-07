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
    public class EmailControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string InValidResendSlotModel = "InValidResendSlotModel";
        private const string ValidResendSlotModel = "ValidResendSlotModel";

        private EmailController emailController;
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<IResendSlotInformationBusiness> resendSlotInformationBusinessMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<IValidator<ResendSlotInformationViewModel>> resendSlotInformationViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            resendSlotInformationBusinessMock = new Mock<IResendSlotInformationBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            resendSlotInformationViewModelValidatorMock = new Mock<IValidator<ResendSlotInformationViewModel>>();
            emailController = new EmailController(symmetryEncryptionMock.Object, resendSlotInformationBusinessMock.Object,
                currentUserMock.Object, resendSlotInformationViewModelValidatorMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task ResendSlotMeetingInformation_InValidResendSlotInformationModel_ReturnsValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            resendSlotInformationViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<ResendSlotInformationViewModel>())).Returns(new ValidationResult(validationFailures));
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await emailController.ResendSlotMeetingInformation(new ResendSlotInformationViewModel() { ResendSlotModel = InValidResendSlotModel }); ;

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<CustomerSummaryModel>())), Times.Never());
            resendSlotInformationViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<ResendSlotInformationViewModel>())), Times.Once());
        }




        [Test]
        public async Task ResendSlotMeetingInformation_CorruptResendSlotInformationModel_ReturnsValidationResponse()
        {
            resendSlotInformationViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<ResendSlotInformationViewModel>())).Returns(new ValidationResult());
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await emailController.ResendSlotMeetingInformation(new ResendSlotInformationViewModel() { ResendSlotModel = InValidResendSlotModel }); ;

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<CustomerSummaryModel>())), Times.Never());
            resendSlotInformationViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<ResendSlotInformationViewModel>())), Times.Once());
        }

      

        [Test]
        public async Task ResendSlotMeetingInformation_ValidResendSlotInformationModel_ReturnsSuccessResponse()
        {
            resendSlotInformationViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<ResendSlotInformationViewModel>())).Returns(new ValidationResult());
            var resendSlotInformationViewModel = new ResendSlotInformationViewModel() { ResendSlotModel = ValidResendSlotModel };
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(resendSlotInformationViewModel));
            Response<bool> resendSlotInformationBusinessMockResponse = new Response<bool>() { Result = true };
            resendSlotInformationBusinessMock.Setup(a => a.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<CustomerSummaryModel>())).Returns(Task.FromResult(resendSlotInformationBusinessMockResponse));

            var response = await emailController.ResendSlotMeetingInformation(resendSlotInformationViewModel);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<CustomerSummaryModel>())), Times.Once());
            resendSlotInformationViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<ResendSlotInformationViewModel>())), Times.Once());
        }


        private static List<ValidationFailure> CreateDefaultValidationFailure()
        {
            ValidationFailure validationFailure = new ValidationFailure("", AppBusinessMessagesConstants.CorruptData);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }

    }
}
