﻿using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class CustomerControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private CustomerController customerController;
        private Mock<ICustomerBusiness> customerBusinessMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerBusinessMock = new Mock<ICustomerBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            customerController = new CustomerController(customerBusinessMock.Object, currentUserMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetProfileSettings_ReturnsSuccessResponse()
        {
            var response = await customerController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
        }
    }
}
