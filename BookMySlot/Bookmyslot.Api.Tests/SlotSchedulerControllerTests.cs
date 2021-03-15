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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class SlotSchedulerControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string InValidSlotSchedulerSlotModelKey = "InValidSlotSchedulerSlotModelKey";
        private const string ValidSlotSchedulerSlotModelKey = "ValidSlotSchedulerSlotModelKey";

        private SlotSchedulerController slotSchedulerController;
        private Mock<ISlotSchedulerBusiness> slotSchedulerBusinessMock;
        private Mock<IKeyEncryptor> keyEncryptorMock;

        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            keyEncryptorMock = new Mock<IKeyEncryptor>();
            slotSchedulerBusinessMock = new Mock<ISlotSchedulerBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            slotSchedulerController = new SlotSchedulerController(slotSchedulerBusinessMock.Object, keyEncryptorMock.Object, currentUserMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task ScheduleSlot_NullSlotSchedulerInformationModel_ReturnsValidationResponse()
        {
            var response = await slotSchedulerController.Post(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.SlotScheduleInfoMissing));
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task ScheduleSlot_EmptySlotSchedulerInformationModel_ReturnsValidationResponse()
        {
            var response = await slotSchedulerController.Post(new SlotSchedulerViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.SlotScheduleInfoRequired));
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task ScheduleSlot_InValidSlotSchedulerInformationModel_ReturnsValidationResponse()
        {
            keyEncryptorMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await slotSchedulerController.Post(new SlotSchedulerViewModel() { SlotModelKey = InValidSlotSchedulerSlotModelKey });

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Never());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task ScheduleSlot_ValidSlotSchedulerInformationModel_ReturnsSuccessResponse()
        {
            var slotSchedulerViewModel = new SlotSchedulerViewModel() { SlotModelKey = ValidSlotSchedulerSlotModelKey };
            keyEncryptorMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(slotSchedulerViewModel));
            Response<bool> slotSchedulerBusinessMockResponse = new Response<bool>() { Result = true };
            slotSchedulerBusinessMock.Setup(a => a.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotSchedulerBusinessMockResponse));

            var response = await slotSchedulerController.Post(new SlotSchedulerViewModel() { SlotModelKey = InValidSlotSchedulerSlotModelKey });

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            keyEncryptorMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotSchedulerBusinessMock.Verify((m => m.ScheduleSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Once());
        }
    }
}
