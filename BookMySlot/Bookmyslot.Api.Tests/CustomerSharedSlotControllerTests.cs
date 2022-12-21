using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
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
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private Mock<ICancelledSlotResponseAdaptor> cancelledSlotResponseAdaptorMock;
        private Mock<ISharedSlotResponseAdaptor> sharedSlotResponseAdaptorMock;

        [SetUp]
        public void Setup()
        {
            customerSharedSlotBusinessMock = new Mock<ICustomerSharedSlotBusiness>();
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            currentUserMock = new Mock<ICurrentUser>();
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            cancelledSlotResponseAdaptorMock = new Mock<ICancelledSlotResponseAdaptor>();
            sharedSlotResponseAdaptorMock = new Mock<ISharedSlotResponseAdaptor>();

            customerSharedSlotController = new CustomerSharedSlotController(customerSharedSlotBusinessMock.Object,
                symmetryEncryptionMock.Object, currentUserMock.Object, customerResponseAdaptorMock.Object,
                cancelledSlotResponseAdaptorMock.Object, sharedSlotResponseAdaptorMock.Object);

            Result<CurrentUserModel> currentUserMockResponse = new Result<CurrentUserModel>() { Value = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Result<SharedSlotModel>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerYetToBeBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));
            Result<SharedSlotViewModel> sharedSlotResponseAdaptorMockResponse = new Result<SharedSlotViewModel>() { ResultType = ResultType.Empty };
            sharedSlotResponseAdaptorMock.Setup(a => a.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())).Returns(sharedSlotResponseAdaptorMockResponse);

            var response = await customerSharedSlotController.GetCustomerYetToBeBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerYetToBeBookedSlots(It.IsAny<string>())), Times.Once());
            sharedSlotResponseAdaptorMock.Verify((m => m.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Result<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Result<SharedSlotModel>() { Value = CreateDefaultSharedSlotModel() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerYetToBeBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));
            Result<SharedSlotViewModel> sharedSlotResponseAdaptorMockResponse = new Result<SharedSlotViewModel>() { Value = CreateDefaultSharedSlotViewModel() };
            sharedSlotResponseAdaptorMock.Setup(a => a.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())).Returns(sharedSlotResponseAdaptorMockResponse);

            var response = await customerSharedSlotController.GetCustomerYetToBeBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerYetToBeBookedSlots(It.IsAny<string>())), Times.Once());
            sharedSlotResponseAdaptorMock.Verify((m => m.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Result<SharedSlotModel>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));
            Result<SharedSlotViewModel> sharedSlotResponseAdaptorMockResponse = new Result<SharedSlotViewModel>() { ResultType = ResultType.Empty };
            sharedSlotResponseAdaptorMock.Setup(a => a.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())).Returns(sharedSlotResponseAdaptorMockResponse);

            var response = await customerSharedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            sharedSlotResponseAdaptorMock.Verify((m => m.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Result<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Result<SharedSlotModel>() { Value = CreateDefaultSharedSlotModel() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));
            Result<SharedSlotViewModel> sharedSlotResponseAdaptorMockResponse = new Result<SharedSlotViewModel>() { Value = CreateDefaultSharedSlotViewModel() };
            sharedSlotResponseAdaptorMock.Setup(a => a.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())).Returns(sharedSlotResponseAdaptorMockResponse);

            var response = await customerSharedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            sharedSlotResponseAdaptorMock.Verify((m => m.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerCompletedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Result<SharedSlotModel>() { ResultType = ResultType.Empty };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));
            Result<SharedSlotViewModel> sharedSlotResponseAdaptorMockResponse = new Result<SharedSlotViewModel>() { ResultType = ResultType.Empty };
            sharedSlotResponseAdaptorMock.Setup(a => a.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())).Returns(sharedSlotResponseAdaptorMockResponse);

            var response = await customerSharedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            sharedSlotResponseAdaptorMock.Verify((m => m.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCompletedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Result<SharedSlotModel> customerSharedSlotBusinessMockResponse = new Result<SharedSlotModel>() { Value = CreateDefaultSharedSlotModel() };
            customerSharedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerSharedSlotBusinessMockResponse));
            Result<SharedSlotViewModel> sharedSlotResponseAdaptorMockResponse = new Result<SharedSlotViewModel>() { Value = CreateDefaultSharedSlotViewModel() };
            sharedSlotResponseAdaptorMock.Setup(a => a.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())).Returns(sharedSlotResponseAdaptorMockResponse);

            var response = await customerSharedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerSharedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            sharedSlotResponseAdaptorMock.Verify((m => m.CreateSharedSlotViewModel(It.IsAny<Result<SharedSlotModel>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerCancelledSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<IEnumerable<CancelledSlotModel>> customerSharedSlotBusinessMockResponse = new Result<IEnumerable<CancelledSlotModel>>() { ResultType = ResultType.Empty };
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
            Result<IEnumerable<CancelledSlotModel>> customerSharedSlotBusinessMockResponse = new Result<IEnumerable<CancelledSlotModel>>() { Value = new List<CancelledSlotModel>() };
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

        private SharedSlotViewModel CreateDefaultSharedSlotViewModel()
        {
            return new SharedSlotViewModel();
        }


    }
}
