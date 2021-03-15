using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
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

    public class SearchCustomerControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string ValidSearchKey = "ValidSearchKey";

        private SearchCustomerController searchCustomerController;
        private Mock<ISearchCustomerBusiness> searchCustomerBusinessMock;

        [SetUp]
        public void Setup()
        {
            searchCustomerBusinessMock = new Mock<ISearchCustomerBusiness>();
            searchCustomerController = new SearchCustomerController(searchCustomerBusinessMock.Object);
        }

        [TestCase("")]
        [TestCase("   ")]
        public async Task SearchCustomer_InValidSearchKey_ReturnsValidationResponse(string searchKey)
        {
            var response = await searchCustomerController.Get(searchKey);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidSearchKey));
            searchCustomerBusinessMock.Verify((m => m.SearchCustomers(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task SearchCustomer_ValidSearchKeyHasNoRecords_ReturnsEmptyResponse()
        {
            Response<List<SearchCustomerModel>> searchCustomerBusinessMockResponse = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Empty };
            searchCustomerBusinessMock.Setup(a => a.SearchCustomers(It.IsAny<string>())).Returns(Task.FromResult(searchCustomerBusinessMockResponse));

            var response = await searchCustomerController.Get(ValidSearchKey);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            searchCustomerBusinessMock.Verify((m => m.SearchCustomers(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task SearchCustomer_ValidSearchKeyHasRecords_ReturnsSuccessResponse()
        {
            Response<List<SearchCustomerModel>> searchCustomerBusinessMockResponse = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Success };
            searchCustomerBusinessMock.Setup(a => a.SearchCustomers(It.IsAny<string>())).Returns(Task.FromResult(searchCustomerBusinessMockResponse));

            var response = await searchCustomerController.Get(ValidSearchKey);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            searchCustomerBusinessMock.Verify((m => m.SearchCustomers(It.IsAny<string>())), Times.Once());
        }
     
    }
}
