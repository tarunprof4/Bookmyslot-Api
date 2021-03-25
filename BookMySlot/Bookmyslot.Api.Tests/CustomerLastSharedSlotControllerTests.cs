﻿using Bookmyslot.Api.Authentication.Common;
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

    public class CustomerLastSharedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private CustomerLastSharedSlotController customerLastSharedSlotController;
        private Mock<ICustomerLastSharedSlotBusiness> customerLastSharedSlotBusinessMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerLastSharedSlotBusinessMock = new Mock<ICustomerLastSharedSlotBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            customerLastSharedSlotController = new CustomerLastSharedSlotController(customerLastSharedSlotBusinessMock.Object, currentUserMock.Object);

            Response<CustomerAuthModel> currentUserMockResponse = new Response<CustomerAuthModel>() { Result = new CustomerAuthModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerLastSlot_NoRecordAvailable_ReturnsEmptyResponse()
        {
            Response<CustomerLastSharedSlotModel> customerLastBookedSlotBusinessMockResponse = new Response<CustomerLastSharedSlotModel>() {  ResultType = ResultType.Empty};
            customerLastSharedSlotBusinessMock.Setup(a => a.GetCustomerLatestSharedSlot(It.IsAny<string>())).Returns(Task.FromResult(customerLastBookedSlotBusinessMockResponse));

            var response = await customerLastSharedSlotController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerLastSharedSlotBusinessMock.Verify((m => m.GetCustomerLatestSharedSlot(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerLastSlot_RecordAvailable_ReturnsSuccessResponse()
        {
            Response<CustomerLastSharedSlotModel> customerLastBookedSlotBusinessMockResponse = new Response<CustomerLastSharedSlotModel>() { Result = new CustomerLastSharedSlotModel() };
            customerLastSharedSlotBusinessMock.Setup(a => a.GetCustomerLatestSharedSlot(It.IsAny<string>())).Returns(Task.FromResult(customerLastBookedSlotBusinessMockResponse));

            var response = await customerLastSharedSlotController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerLastSharedSlotBusinessMock.Verify((m => m.GetCustomerLatestSharedSlot(It.IsAny<string>())), Times.Once());
        }



     

    }
}
