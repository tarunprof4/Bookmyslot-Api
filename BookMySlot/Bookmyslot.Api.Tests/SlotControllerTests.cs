using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Location.Contracts.Configuration;
using Bookmyslot.Api.Location.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class SlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string ValidSlotTitle = "SlotTitle";
        private const string InValidTimeZone = "InValidTimeZone";
        private const string InValidSlotDate = "31-31-2000";
        private const string ValidTimeZone = TimeZoneConstants.IndianTimezone;
        private const string ValidTimeZoneCountry = "India";
        private readonly string ValidSlotDate = DateTime.UtcNow.AddDays(2).ToString(DateTimeConstants.ApplicationInputDatePattern);

        private SlotController slotController;
        private Mock<ISlotBusiness> slotBusinessMock;
        private Mock<IKeyEncryptor> keyEncryptorMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<INodaTimeZoneLocationBusiness> nodaTimeZoneLocationBusinessMock;

        [SetUp]
        public void Setup()
        {
            slotBusinessMock = new Mock<ISlotBusiness>();
            keyEncryptorMock = new Mock<IKeyEncryptor>();
            currentUserMock = new Mock<ICurrentUser>();
            nodaTimeZoneLocationBusinessMock = new Mock<INodaTimeZoneLocationBusiness>();
            slotController = new SlotController(slotBusinessMock.Object, keyEncryptorMock.Object, currentUserMock.Object, nodaTimeZoneLocationBusinessMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task CreateSlot_NullSlotViewModel_ReturnsValidationResponse()
        {
            var postResponse = await slotController.Post(null);

            var objectResult = postResponse as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.SlotDetailsMissing));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotBusinessMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task CreateSlot_EmptySlotViewModel_ReturnsValidationResponse()
        {
            var postResponse = await slotController.Post(DefaultEmptySlotViewModel());

            var objectResult = postResponse as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.SlotTitleRequired));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.TimeZoneRequired));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.SlotDateRequired));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotBusinessMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task CreateSlot_InValidSlotViewModel_ReturnsValidationResponse()
        {
            this.nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());

            var postResponse = await slotController.Post(DefaultInValidSlotViewModel());

            var objectResult = postResponse as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidTimeZone));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidSlotDate));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotBusinessMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task CreateSlot_ValidSlotViewModel_ReturnsSuccessResponse()
        {
            var guid = Guid.NewGuid();
            this.nodaTimeZoneLocationBusinessMock.Setup(a => a.GetNodaTimeZoneLocationInformation()).Returns(DefaultNodaTimeLocationConfiguration());
            Response<Guid> slotBusinessMockResponse = new Response<Guid>() { Result = guid };
            slotBusinessMock.Setup(a => a.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotBusinessMockResponse));

            var postResponse = await slotController.Post(DefaultValidSlotViewModel());

            var objectResult = postResponse as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            Assert.AreEqual(objectResult.Value, guid);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            slotBusinessMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>(), It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task CancelSlot_InValidSlotKey_ReturnsValidationResponse()
        {
            keyEncryptorMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var postResponse = await slotController.CancelSlot(new CancelSlot());

            var objectResult = postResponse as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.CorruptData));
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            keyEncryptorMock.Verify(a => a.Decrypt(It.IsAny<string>()), Times.Once());
            slotBusinessMock.Verify((m => m.CancelSlot(It.IsAny<Guid>(), It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task CancelSlot_ValidSlotKey_ReturnsSuccessResponse()
        {
            var cancelSlot = new CancelSlot();
            keyEncryptorMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(cancelSlot));
            Response<bool> slotBusinessMockResponse = new Response<bool>() { Result = true };
            slotBusinessMock.Setup(a => a.CancelSlot(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(slotBusinessMockResponse));

            var postResponse = await slotController.CancelSlot(cancelSlot);

            var objectResult = postResponse as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            keyEncryptorMock.Verify(a => a.Decrypt(It.IsAny<string>()), Times.Once());
            slotBusinessMock.Verify((m => m.CancelSlot(It.IsAny<Guid>(), It.IsAny<string>())), Times.Once());
        }


        private SlotViewModel DefaultEmptySlotViewModel()
        {
            return new SlotViewModel();
        }



        private SlotViewModel DefaultInValidSlotViewModel()
        {
            var slotviewModel = new SlotViewModel();
            slotviewModel.TimeZone = InValidTimeZone;
            slotviewModel.SlotDate = InValidSlotDate;
            return slotviewModel;
        }

        private SlotViewModel DefaultValidSlotViewModel()
        {
            var slotviewModel = new SlotViewModel();
            slotviewModel.Title = ValidSlotTitle;
            slotviewModel.TimeZone = ValidTimeZone;
            slotviewModel.SlotDate = ValidSlotDate;
            return slotviewModel;
        }

        private NodaTimeZoneLocationConfiguration DefaultNodaTimeLocationConfiguration()
        {
            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();
            zoneWithCountryId.Add(ValidTimeZone, ValidTimeZoneCountry);
            var countries = zoneWithCountryId.Values.Distinct().ToList();
            return new NodaTimeZoneLocationConfiguration(zoneWithCountryId, countries);
        }

       

    }
}