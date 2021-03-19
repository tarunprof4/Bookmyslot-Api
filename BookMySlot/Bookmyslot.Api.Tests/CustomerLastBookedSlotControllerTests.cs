using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class CustomerLastBookedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private CustomerLastBookedSlotController customerLastBookedSlotController;
        private Mock<ICustomerLastBookedSlotBusiness> customerLastBookedSlotBusinessMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerLastBookedSlotBusinessMock = new Mock<ICustomerLastBookedSlotBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            customerLastBookedSlotController = new CustomerLastBookedSlotController(customerLastBookedSlotBusinessMock.Object, currentUserMock.Object);

            Response<string> currentUserMockResponse = new Response<string>() { Result = CustomerId };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerLastSlot_NoRecordAvailable_ReturnsEmptyResponse()
        {
            Response<CustomerLastBookedSlotModel> customerLastBookedSlotBusinessMockResponse = new Response<CustomerLastBookedSlotModel>() {  ResultType = ResultType.Empty};
            customerLastBookedSlotBusinessMock.Setup(a => a.GetCustomerLatestSlot(It.IsAny<string>())).Returns(Task.FromResult(customerLastBookedSlotBusinessMockResponse));

            var response = await customerLastBookedSlotController.GetCustomerLastSlot();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerLastBookedSlotBusinessMock.Verify((m => m.GetCustomerLatestSlot(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerLastSlot_RecordAvailable_ReturnsSuccessResponse()
        {
            Response<CustomerLastBookedSlotModel> customerLastBookedSlotBusinessMockResponse = new Response<CustomerLastBookedSlotModel>() { Result = new CustomerLastBookedSlotModel() };
            customerLastBookedSlotBusinessMock.Setup(a => a.GetCustomerLatestSlot(It.IsAny<string>())).Returns(Task.FromResult(customerLastBookedSlotBusinessMockResponse));

            var response = await customerLastBookedSlotController.GetCustomerLastSlot();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerLastBookedSlotBusinessMock.Verify((m => m.GetCustomerLatestSlot(It.IsAny<string>())), Times.Once());
        }



     

    }
}
