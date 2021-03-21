using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Business.Tests
{
    public class SearchCustomerTests
    {
        private const string SearchKey = "SearchKey";
        private SearchCustomerBusiness searchCustomerBusiness;
        private Mock<ISearchCustomerRepository> searchCustomerRepositoryMock;

        [SetUp]
        public void Setup()
        {
            searchCustomerRepositoryMock = new Mock<ISearchCustomerRepository>();
            searchCustomerBusiness = new SearchCustomerBusiness(searchCustomerRepositoryMock.Object);
        }


        [Test]
        public async Task SearchCustomers_ValidSearchKey_ReturnsSuccessResponse()
        {
            string searchKey = SearchKey;
            Response<List<SearchCustomerModel>> slotModelResponseMock = new Response<List<SearchCustomerModel>>() { Result = new List<SearchCustomerModel>() };
            searchCustomerRepositoryMock.Setup(a => a.GetPreProcessedSearchedCustomers(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var searchedCustomersResponse = await searchCustomerBusiness.SearchCustomers(searchKey);

            Assert.AreEqual(searchedCustomersResponse.ResultType, ResultType.Success);
            searchCustomerRepositoryMock.Verify((m => m.GetPreProcessedSearchedCustomers(searchKey)), Times.Once());
        }
    }
}