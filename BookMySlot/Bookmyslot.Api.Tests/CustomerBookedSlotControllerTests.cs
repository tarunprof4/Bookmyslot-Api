using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class CustomerBookedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private CustomerBookedSlotController customerBookedSlotController;
        private Mock<ICustomerBookedSlotBusiness> customerBookedSlotBusinessMock;
        private Mock<IKeyEncryptor> keyEncryptorMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerBookedSlotBusinessMock = new Mock<ICustomerBookedSlotBusiness>();
            keyEncryptorMock = new Mock<IKeyEncryptor>();
            currentUserMock = new Mock<ICurrentUser>();
            customerBookedSlotController = new CustomerBookedSlotController(customerBookedSlotBusinessMock.Object, keyEncryptorMock.Object, currentUserMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { Result = new List<BookedSlotModel>() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
        }



        [Test]
        public async Task GetCustomerCompletedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCompletedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { Result = new List<BookedSlotModel>() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
        }



        [Test]
        public async Task GetCustomerCancelledSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotInformationModel>>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerCancelledSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCancelledSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotInformationModel>>() { Result = new List<CancelledSlotInformationModel>() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerCancelledSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        }

    }



}
