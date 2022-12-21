using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.ValueObject;
using Moq;
using Nest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Repositories.Tests
{
    public class SearchCustomerRepositoryTests
    {
        private const string SearchByUserName = "@UserName";
        private const string SearchByName = "#name";
        private const string SearchByBioHeadLine = "bioheadline";
        private const string UserName = "UserName";
        private const string FullName = "FirstNameLastName";
        private const string PhotoUrl = "PhotoUrl";
        private SearchCustomerRepository searchCustomerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;
        private Mock<ElasticClient> elasticClientMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            elasticClientMock = new Mock<ElasticClient>();
            searchCustomerRepository = new SearchCustomerRepository(dbConnectionMock.Object, dbInterceptorMock.Object, elasticClientMock.Object);
        }


        [Test]
        public async Task SearchCustomersByUserName_HasNoRecord_ReturnsEmptyResponse()
        {
            SearchCustomerEntity searchCustomerEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchCustomerEntity>>>())).Returns(Task.FromResult(searchCustomerEntity));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByUserName(SearchByUserName);

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchCustomerEntity>>>()), Times.Once);
        }

        [Test]
        public async Task SearchCustomersByUserName_HasRecord_ReturnsSuccessResponse()
        {
            SearchCustomerEntity searchCustomerEntity = new SearchCustomerEntity();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchCustomerEntity>>>())).Returns(Task.FromResult(searchCustomerEntity));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByUserName(SearchByUserName);

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchCustomerEntity>>>()), Times.Once);
        }



        [Test]
        public async Task SearchCustomersByName_HasNoRecord_ReturnsEmptyResponse()
        {
            IReadOnlyCollection<SearchCustomerModel> searchCustomerModels = new List<SearchCustomerModel>();
            var mockSearchResponse = new Mock<ISearchResponse<SearchCustomerModel>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(searchCustomerModels);
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>())).Returns(Task.FromResult(mockSearchResponse.Object));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByName(SearchByName, new PageParameterModel(0, 0));

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>()), Times.Once);
        }

        [Test]
        public async Task SearchCustomersByName_HasRecord_ReturnsSuccessResponse()
        {
            IReadOnlyCollection<SearchCustomerModel> searchCustomerModels = new List<SearchCustomerModel>() { new SearchCustomerModel() };
            var mockSearchResponse = new Mock<ISearchResponse<SearchCustomerModel>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(searchCustomerModels);
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>())).Returns(Task.FromResult(mockSearchResponse.Object));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByName(SearchByName, new PageParameterModel(0, 0));

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>()), Times.Once);
        }


        [Test]
        public async Task SearchCustomersByBioHeadLine_HasNoRecord_ReturnsEmptyResponse()
        {
            IReadOnlyCollection<SearchCustomerModel> searchCustomerModels = new List<SearchCustomerModel>() { };
            var mockSearchResponse = new Mock<ISearchResponse<SearchCustomerModel>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(searchCustomerModels);
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>())).Returns(Task.FromResult(mockSearchResponse.Object));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByBioHeadLine(SearchByBioHeadLine, new PageParameterModel(0, 0));

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>()), Times.Once);
        }

        [Test]
        public async Task SearchCustomersByBioHeadLine_HasRecord_ReturnsSuccessResponse()
        {
            IReadOnlyCollection<SearchCustomerModel> searchCustomerModels = new List<SearchCustomerModel>() { new SearchCustomerModel() };
            var mockSearchResponse = new Mock<ISearchResponse<SearchCustomerModel>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(searchCustomerModels);
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>())).Returns(Task.FromResult(mockSearchResponse.Object));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByBioHeadLine(SearchByBioHeadLine, new PageParameterModel(0, 0));

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<ISearchResponse<SearchCustomerModel>>>>()), Times.Once);
        }




        private List<SearchCustomerModel> DefaultCreateSearchCustomerModels()
        {
            var searchCustomerModels = new List<SearchCustomerModel>();
            var searchCustomerModel = new SearchCustomerModel();
            searchCustomerModel.UserName = UserName;
            searchCustomerModels.Add(searchCustomerModel);

            searchCustomerModel = new SearchCustomerModel();
            searchCustomerModel.UserName = UserName;
            searchCustomerModels.Add(searchCustomerModel);

            return searchCustomerModels;
        }


        private IEnumerable<SearchCustomerEntity> DefaultCreateSearchCustomerEntities()
        {
            var searchCustomerEntities = new List<SearchCustomerEntity>();
            var searchCustomerEntity = new SearchCustomerEntity();
            searchCustomerEntity.UserName = UserName;
            searchCustomerEntity.FirstName = FullName;
            searchCustomerEntity.LastName = FullName;
            searchCustomerEntity.ProfilePictureUrl = PhotoUrl;
            searchCustomerEntities.Add(searchCustomerEntity);

            searchCustomerEntity = new SearchCustomerEntity();
            searchCustomerEntity.UserName = UserName;
            searchCustomerEntity.FirstName = FullName;
            searchCustomerEntity.LastName = FullName;
            searchCustomerEntity.ProfilePictureUrl = PhotoUrl;
            searchCustomerEntities.Add(searchCustomerEntity);

            return searchCustomerEntities;
        }



    }
}