//using Bookmyslot.Api.Cache.Contracts.Interfaces;
//using Bookmyslot.Api.Common.Compression.Interfaces;
//using Bookmyslot.Api.Common.Contracts.Configuration;
//using Bookmyslot.Api.Controllers;
//using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Bookmyslot.Api.Tests
//{
  
//    public class CustomerSlotControllerTests
//    {
//        private const string CustomerId = "CustomerId";
//        private CustomerSlotController customerBookedSlotController;
//        private Mock<ICustomerSlotBusiness> customerSlotBusinessMock;
//        private Mock<IKeyEncryptor> keyEncryptorMock;
//        private Mock<IDistributedInMemoryCacheBuisness> distributedInMemoryCacheBuisnessMock;
//        private Mock<IHashing> hashingMock;
        

//        [SetUp]
//        public void Setup()
//        {
//            customerSlotBusinessMock = new Mock<ICustomerSlotBusiness>();
//            keyEncryptorMock = new Mock<IKeyEncryptor>();
//            distributedInMemoryCacheBuisnessMock = new Mock<IDistributedInMemoryCacheBuisness>();
//            hashingMock = new Mock<IHashing>();
//            customerBookedSlotController = new CustomerSlotController(customerSlotBusinessMock.Object, keyEncryptorMock.Object,
//                distributedInMemoryCacheBuisnessMock.Object, hashingMock.Object, new CacheConfiguration());
//        }

//        [Test]
//        public async Task GetCustomerBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
//        {
//            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { ResultType = ResultType.Empty };
//            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

//            var response = await customerBookedSlotController.GetCustomerBookedSlots();

//            var objectResult = response as ObjectResult;
//            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
//            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
//            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
//        }

//        [Test]
//        public async Task GetCustomerBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
//        {
//            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { Result = new List<BookedSlotModel>() };
//            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

//            var response = await customerBookedSlotController.GetCustomerBookedSlots();

//            var objectResult = response as ObjectResult;
//            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
//            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
//            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
//        }



//        [Test]
//        public async Task GetCustomerCompletedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
//        {
//            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { ResultType = ResultType.Empty };
//            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

//            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

//            var objectResult = response as ObjectResult;
//            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
//            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
//            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
//        }

//        [Test]
//        public async Task GetCustomerCompletedSlots_RecordsAvailable_ReturnsSuccessResponse()
//        {
//            Response<IEnumerable<BookedSlotModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<BookedSlotModel>>() { Result = new List<BookedSlotModel>() };
//            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

//            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

//            var objectResult = response as ObjectResult;
//            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
//            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
//            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
//        }



//        [Test]
//        public async Task GetCustomerCancelledSlots_NoRecordsAvailable_ReturnsEmptyResponse()
//        {
//            Response<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotInformationModel>>() { ResultType = ResultType.Empty };
//            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

//            var response = await customerBookedSlotController.GetCustomerCancelledSlots();

//            var objectResult = response as ObjectResult;
//            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
//            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
//            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
//        }

//        [Test]
//        public async Task GetCustomerCancelledSlots_RecordsAvailable_ReturnsSuccessResponse()
//        {
//            Response<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotInformationModel>>() { Result = new List<CancelledSlotInformationModel>() };
//            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

//            var response = await customerBookedSlotController.GetCustomerCancelledSlots();

//            var objectResult = response as ObjectResult;
//            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
//            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
//            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
//        }

//    }
//}
