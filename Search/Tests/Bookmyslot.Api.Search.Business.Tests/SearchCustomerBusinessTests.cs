using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Business.Tests
{
    public class SearchCustomerBusinessTests
    {
        private const string SearchKey = "SearchKey";
        private const string SearchByUserName = "@UserName";
        private const string SearchByName = "#name";
        private const string SearchByBioHeadLine = "bioheadline";
        private SearchCustomerBusiness searchCustomerBusiness;
        private Mock<ISearchRepository> searchRepositoryMock;
        private Mock<ISearchCustomerRepository> searchCustomerRepositoryMock;

        [SetUp]
        public void Setup()
        {
            searchRepositoryMock = new Mock<ISearchRepository>();
            searchCustomerRepositoryMock = new Mock<ISearchCustomerRepository>();
            searchCustomerBusiness = new SearchCustomerBusiness(searchRepositoryMock.Object, searchCustomerRepositoryMock.Object);
        }


        [Test]
        public async Task SearchCustomers_GetsPreProcessedSearchedCustomers_ReturnsSuccessResponse()
        {
            string searchKey = SearchKey;
            Response<List<SearchCustomerModel>> searchCustomersResponseMock = new Response<List<SearchCustomerModel>>() { Result = new List<SearchCustomerModel>() };
            searchRepositoryMock.Setup(a => a.GetPreProcessedSearchedCustomers(It.IsAny<string>())).Returns(Task.FromResult(searchCustomersResponseMock));

            var searchedCustomersResponse = await searchCustomerBusiness.SearchCustomers(searchKey);

            Assert.AreEqual(searchedCustomersResponse.ResultType, ResultType.Success);
            searchRepositoryMock.Verify((m => m.GetPreProcessedSearchedCustomers(searchKey)), Times.Once());
        }


        [Test]
        public async Task SearchCustomers_GetCustomerByUserName_ReturnsSuccessResponse()
        {
            string searchKey = SearchByUserName;
            Response<List<SearchCustomerModel>> searchRepositoryResponseMock = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Empty };
            searchRepositoryMock.Setup(a => a.GetPreProcessedSearchedCustomers(It.IsAny<string>())).Returns(Task.FromResult(searchRepositoryResponseMock));
            Response<SearchCustomerModel> searchCustomersResponseMock = new Response<SearchCustomerModel>() { Result = new SearchCustomerModel() };
            searchCustomerRepositoryMock.Setup(a => a.SearchCustomersByUserName(It.IsAny<string>())).Returns(Task.FromResult(searchCustomersResponseMock));
            
            var searchedCustomersResponse = await searchCustomerBusiness.SearchCustomers(searchKey);

            Assert.AreEqual(searchedCustomersResponse.ResultType, ResultType.Success);
            searchRepositoryMock.Verify((m => m.GetPreProcessedSearchedCustomers(searchKey)), Times.Once());
            searchCustomerRepositoryMock.Verify((m => m.SearchCustomersByUserName(searchKey)), Times.Once());
            searchRepositoryMock.Verify((m => m.SavePreProcessedSearchedCustomers(searchKey, It.IsAny<List<SearchCustomerModel>>())), Times.Once());
        }


        [Test]
        public async Task SearchCustomers_GetCustomerByName_ReturnsSuccessResponse()
        {
            string searchKey = SearchByName;
            Response<List<SearchCustomerModel>> searchRepositoryResponseMock = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Empty };
            searchRepositoryMock.Setup(a => a.GetPreProcessedSearchedCustomers(It.IsAny<string>())).Returns(Task.FromResult(searchRepositoryResponseMock));
            Response<List<SearchCustomerModel>> searchCustomersResponseMock = new Response<List<SearchCustomerModel>>();
            searchCustomerRepositoryMock.Setup(a => a.SearchCustomersByName(It.IsAny<string>())).Returns(Task.FromResult(searchCustomersResponseMock));

            var searchedCustomersResponse = await searchCustomerBusiness.SearchCustomers(searchKey);

            Assert.AreEqual(searchedCustomersResponse.ResultType, ResultType.Success);
            searchRepositoryMock.Verify((m => m.GetPreProcessedSearchedCustomers(searchKey)), Times.Once());
            searchCustomerRepositoryMock.Verify((m => m.SearchCustomersByName(searchKey)), Times.Once());
            searchRepositoryMock.Verify((m => m.SavePreProcessedSearchedCustomers(searchKey, It.IsAny<List<SearchCustomerModel>>())), Times.Once());
        }


        [Test]
        public async Task SearchCustomers_GetCustomerByBioHeadLine_ReturnsSuccessResponse()
        {
            string searchKey = SearchByBioHeadLine;
            Response<List<SearchCustomerModel>> searchRepositoryResponseMock = new Response<List<SearchCustomerModel>>() { ResultType = ResultType.Empty };
            searchRepositoryMock.Setup(a => a.GetPreProcessedSearchedCustomers(It.IsAny<string>())).Returns(Task.FromResult(searchRepositoryResponseMock));
            Response<List<SearchCustomerModel>> searchCustomersResponseMock = new Response<List<SearchCustomerModel>>();
            searchCustomerRepositoryMock.Setup(a => a.SearchCustomersByBioHeadLine(It.IsAny<string>())).Returns(Task.FromResult(searchCustomersResponseMock));

            var searchedCustomersResponse = await searchCustomerBusiness.SearchCustomers(searchKey);

            Assert.AreEqual(searchedCustomersResponse.ResultType, ResultType.Success);
            searchRepositoryMock.Verify((m => m.GetPreProcessedSearchedCustomers(searchKey)), Times.Once());
            searchCustomerRepositoryMock.Verify((m => m.SearchCustomersByBioHeadLine(searchKey)), Times.Once());
            searchRepositoryMock.Verify((m => m.SavePreProcessedSearchedCustomers(searchKey, It.IsAny<List<SearchCustomerModel>>())), Times.Once());
        }
    }
}