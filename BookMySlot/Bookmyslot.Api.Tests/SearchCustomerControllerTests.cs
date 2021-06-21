using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Configuration;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Search.Contracts;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class SearchCustomerControllerTests
    {
        private const string ValidSearchKey = "ValidSearchKey";
        private const string InValidSearchKey = "in";

        private SearchCustomerController searchCustomerController;
        private Mock<ISearchCustomerBusiness> searchCustomerBusinessMock;
        private Mock<IDistributedInMemoryCacheBuisness> distributedInMemoryCacheBuisnessMock;
        private CacheConfiguration cacheConfiguration;


        [SetUp]
        public void Setup()
        {
            searchCustomerBusinessMock = new Mock<ISearchCustomerBusiness>();
            distributedInMemoryCacheBuisnessMock = new Mock<IDistributedInMemoryCacheBuisness>();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            cacheConfiguration = new CacheConfiguration(configuration);
            searchCustomerController = new SearchCustomerController(searchCustomerBusinessMock.Object, distributedInMemoryCacheBuisnessMock.Object, cacheConfiguration);
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
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<SearchCustomerModel>>>>>(), It.IsAny<bool>())), Times.Never());
        }


        [TestCase(InValidSearchKey)]
        public async Task SearchCustomer_InValidSearchKeyMinLength_ReturnsValidationResponse(string searchKey)
        {
            var response = await searchCustomerController.Get(searchKey);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidCustomerSearchKeyMinLength));
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<SearchCustomerModel>>>>>(), It.IsAny<bool>())), Times.Never());
        }

        [Test]
        public async Task SearchCustomer_ValidSearchKeyHasNoRecords_ReturnsEmptyResponse()
        {
            Response<List<SearchCustomerModel>> distributedInMemoryCacheBuisnessMockResponse = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Empty };
            distributedInMemoryCacheBuisnessMock.Setup(a => a.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<SearchCustomerModel>>>>>(), It.IsAny<bool>())).Returns(Task.FromResult(distributedInMemoryCacheBuisnessMockResponse));

            var response = await searchCustomerController.Get(ValidSearchKey);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<SearchCustomerModel>>>>>(), It.IsAny<bool>())), Times.Once());
        }

        [Test]
        public async Task SearchCustomer_ValidSearchKeyHasRecords_ReturnsSuccessResponse()
        {
            Response<List<SearchCustomerModel>> distributedInMemoryCacheBuisnessMockResponse = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Success };
            distributedInMemoryCacheBuisnessMock.Setup(a => a.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<SearchCustomerModel>>>>>(), It.IsAny<bool>())).Returns(Task.FromResult(distributedInMemoryCacheBuisnessMockResponse));

            var response = await searchCustomerController.Get(ValidSearchKey);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<SearchCustomerModel>>>>>(), It.IsAny<bool>())), Times.Once());
        }
     
    }
}
