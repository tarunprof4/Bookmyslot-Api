using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
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
        private Mock<IKeyEncryptor> keyEncryptorMock;
        private Mock<IResendSlotInformationBusiness> resendSlotInformationBusinessMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            keyEncryptorMock = new Mock<IKeyEncryptor>();
            resendSlotInformationBusinessMock = new Mock<IResendSlotInformationBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            emailController = new EmailController(keyEncryptorMock.Object, resendSlotInformationBusinessMock.Object, currentUserMock.Object);

            Response<CustomerAuthModel> currentUserMockResponse = new Response<CustomerAuthModel>() { Result = new CustomerAuthModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task ResendSlotMeetingInformation_NullResendSlotInformationModel_ReturnsValidationResponse()
        {
            var response = await emailController.ResendSlotMeetingInformation(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoMissing));
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task ResendSlotMeetingInformation_EmptyResendSlotInformationModel_ReturnsValidationResponse()
        {
            var response = await emailController.ResendSlotMeetingInformation(new ResendSlotInformationViewModel()); ;

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.ResendSlotInfoRequired));
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task ResendSlotMeetingInformation_InValidResendSlotInformationModel_ReturnsValidationResponse()
        {
            keyEncryptorMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await emailController.ResendSlotMeetingInformation(new ResendSlotInformationViewModel() { ResendSlotModel = InValidResendSlotModel }); ;

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task ResendSlotMeetingInformation_ValidResendSlotInformationModel_ReturnsValidationResponse()
        {
            var resendSlotInformationViewModel = new ResendSlotInformationViewModel() { ResendSlotModel = ValidResendSlotModel };
            keyEncryptorMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(resendSlotInformationViewModel));
            Response<bool> resendSlotInformationBusinessMockResponse = new Response<bool>() { Result = true };
            resendSlotInformationBusinessMock.Setup(a => a.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<string>())).Returns(Task.FromResult(resendSlotInformationBusinessMockResponse));

            var response = await emailController.ResendSlotMeetingInformation(resendSlotInformationViewModel);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            resendSlotInformationBusinessMock.Verify((m => m.ResendSlotMeetingInformation(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Once());
        }




    }
}
