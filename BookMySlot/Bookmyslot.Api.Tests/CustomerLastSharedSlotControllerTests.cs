using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class CustomerLastSharedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string Title = "Title";
        private const string Country = "Country";
        private const string TimeZone = "TimeZone";
        private TimeSpan SlotStartTime = new TimeSpan(2,0,0);
        private TimeSpan SlotEndTime = new TimeSpan(3, 0, 0);
        private CustomerLastSharedSlotController customerLastSharedSlotController;
        private Mock<ICustomerLastSharedSlotBusiness> customerLastSharedSlotBusinessMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerLastSharedSlotBusinessMock = new Mock<ICustomerLastSharedSlotBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            customerLastSharedSlotController = new CustomerLastSharedSlotController(customerLastSharedSlotBusinessMock.Object, currentUserMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
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
            Response<CustomerLastSharedSlotModel> customerLastBookedSlotBusinessMockResponse = new Response<CustomerLastSharedSlotModel>() { Result = DefaultCustomerLastSharedSlotModel() };
            customerLastSharedSlotBusinessMock.Setup(a => a.GetCustomerLatestSharedSlot(It.IsAny<string>())).Returns(Task.FromResult(customerLastBookedSlotBusinessMockResponse));

            var response = await customerLastSharedSlotController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var customerLastSharedSlotViewModel = objectResult.Value as CustomerLastSharedSlotViewModel;
            Assert.AreEqual(customerLastSharedSlotViewModel.Title, Title);
            Assert.AreEqual(customerLastSharedSlotViewModel.Country, Country);
            Assert.AreEqual(customerLastSharedSlotViewModel.TimeZone, TimeZone);
            Assert.AreEqual(customerLastSharedSlotViewModel.SlotStartTime, SlotStartTime);
            Assert.AreEqual(customerLastSharedSlotViewModel.SlotEndTime, SlotEndTime);
            Assert.AreEqual(customerLastSharedSlotViewModel.SlotDuration, SlotStartTime - SlotEndTime);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerLastSharedSlotBusinessMock.Verify((m => m.GetCustomerLatestSharedSlot(It.IsAny<string>())), Times.Once());
        }

        private CustomerLastSharedSlotModel DefaultCustomerLastSharedSlotModel()
        {
            return new CustomerLastSharedSlotModel()
            {
                Title = Title,
                Country = Country,
                TimeZone = TimeZone,
                SlotStartTime = SlotStartTime,
                SlotEndTime = SlotEndTime
            };
        }



    }
}
