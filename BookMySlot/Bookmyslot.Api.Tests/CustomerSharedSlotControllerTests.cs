using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Encryption.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
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
    public class CustomerSharedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private CustomerSharedSlotController customerSharedSlotController;
        private Mock<ICustomerSharedSlotBusiness> customerSharedSlotBusinessMock;
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerSharedSlotBusinessMock = new Mock<ICustomerSharedSlotBusiness>();
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            currentUserMock = new Mock<ICurrentUser>();
            customerSharedSlotController = new CustomerSharedSlotController(customerSharedSlotBusinessMock.Object, symmetryEncryptionMock.Object, currentUserMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Response<SharedSlotModel>() { ResultType = ResultType.Empty };
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
            Response<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Response<SharedSlotModel>() { Result = CreateDefaultSharedSlotModel() };
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
            Response<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Response<SharedSlotModel>() { ResultType = ResultType.Empty };
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
            Response<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Response<SharedSlotModel>() { Result = CreateDefaultSharedSlotModel() };
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
            Response<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Response<SharedSlotModel>() { ResultType = ResultType.Empty };
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
            Response<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Response<SharedSlotModel>() { Result = CreateDefaultSharedSlotModel() };
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

        private SharedSlotModel CreateDefaultSharedSlotModel()
        {
            var sharedSlotModel = new SharedSlotModel();
            sharedSlotModel.SharedSlotModels = new List<KeyValuePair<CustomerModel, SlotModel>>();

            return sharedSlotModel;
        }


    }
}
