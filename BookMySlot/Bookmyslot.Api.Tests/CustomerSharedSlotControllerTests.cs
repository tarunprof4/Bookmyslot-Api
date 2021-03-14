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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class CustomerSharedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private CustomerSharedSlotController customerSharedSlotController;
        private Mock<ICustomerSharedSlotBusiness> customerSharedSlotBusinessMock;
        private Mock<IKeyEncryptor> keyEncryptorMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerSharedSlotBusinessMock = new Mock<ICustomerSharedSlotBusiness>();
            keyEncryptorMock = new Mock<IKeyEncryptor>();
            currentUserMock = new Mock<ICurrentUser>();
            customerSharedSlotController = new CustomerSharedSlotController(customerSharedSlotBusinessMock.Object, keyEncryptorMock.Object, currentUserMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<SharedSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<SharedSlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerYetToBeBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerYetToBeBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerYetToBeBookedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<SharedSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<SharedSlotModel>>() { Result = new List<SharedSlotModel>() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerYetToBeBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerYetToBeBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerYetToBeBookedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<SharedSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<SharedSlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<SharedSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<SharedSlotModel>>() { Result = new List<SharedSlotModel>() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerCompletedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<SharedSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<SharedSlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCompletedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<SharedSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<SharedSlotModel>>() { Result = new List<SharedSlotModel>() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerCancelledSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<CancelledSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerCancelledSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCancelledSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<IEnumerable<CancelledSlotModel>> customerSharedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotModel>>() { Result = new List<CancelledSlotModel>() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));

            var response = await customerSharedSlotController.GetCustomerCancelledSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        }


    }
}
