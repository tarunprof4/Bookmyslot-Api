using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Repositories.Tests
{
    public class SearchCustomerRepositoryTests
    {
        private const string SearchKey = "SearchKey";
        private const string SearchByUserName = "@UserName";
        private const string SearchByName = "#name";
        private const string SearchByBioHeadLine = "bioheadline";
        private const string Id = "Id";
        private const string UserName= "UserName";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string FullName = "FirstNameLastName";
        private const string PhotoUrl = "PhotoUrl";
        private SearchCustomerRepository searchCustomerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            searchCustomerRepository = new SearchCustomerRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
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
            IEnumerable<SearchCustomerEntity> searchCustomerEntities = new List<SearchCustomerEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>())).Returns(Task.FromResult(searchCustomerEntities));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByName(SearchByName);

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task SearchCustomersByName_HasRecord_ReturnsSuccessResponse()
        {
            IEnumerable<SearchCustomerEntity> searchCustomerEntities = DefaultCreateSearchCustomerEntities();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>())).Returns(Task.FromResult(searchCustomerEntities));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByName(SearchByName);

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>()), Times.Once);
        }


        [Test]
        public async Task SearchCustomersByBioHeadLine_HasNoRecord_ReturnsEmptyResponse()
        {
            IEnumerable<SearchCustomerEntity> searchCustomerEntities = new List<SearchCustomerEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>())).Returns(Task.FromResult(searchCustomerEntities));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByBioHeadLine(SearchByBioHeadLine);

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task SearchCustomersByBioHeadLine_HasRecord_ReturnsSuccessResponse()
        {
            IEnumerable<SearchCustomerEntity> searchCustomerEntities = DefaultCreateSearchCustomerEntities();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>())).Returns(Task.FromResult(searchCustomerEntities));

            var searchCustomersModelResponse = await searchCustomerRepository.SearchCustomersByBioHeadLine(SearchByBioHeadLine);

            Assert.AreEqual(searchCustomersModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>()), Times.Once);
        }




        private List<SearchCustomerModel> DefaultCreateSearchCustomerModels()
        {
            var searchCustomerModels = new List<SearchCustomerModel>();
            var searchCustomerModel = new SearchCustomerModel();
            searchCustomerModel.UserName = UserName;
            searchCustomerModel.FullName = FullName;
            searchCustomerModel.PhotoUrl = PhotoUrl;
            searchCustomerModels.Add(searchCustomerModel);

            searchCustomerModel = new SearchCustomerModel();
            searchCustomerModel.UserName = UserName;
            searchCustomerModel.FullName = FullName;
            searchCustomerModel.PhotoUrl = PhotoUrl;
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
            searchCustomerEntity.PhotoUrl = PhotoUrl;
            searchCustomerEntities.Add(searchCustomerEntity);

            searchCustomerEntity = new SearchCustomerEntity();
            searchCustomerEntity.UserName = UserName;
            searchCustomerEntity.FirstName = FullName;
            searchCustomerEntity.LastName = FullName;
            searchCustomerEntity.PhotoUrl = PhotoUrl;
            searchCustomerEntities.Add(searchCustomerEntity);

            return searchCustomerEntities;
        }

        private SearchEntity DefaultCreateSearchEntity()
        {
            var searchEntity = new SearchEntity();
            searchEntity.SearchKey = SearchKey;
            searchEntity.Value = CompressedSearchCustomerModels(DefaultCreateSearchCustomerModels());
            return searchEntity;
        }

        private string CompressedSearchCustomerModels(List<SearchCustomerModel> searchCustomerModels)
        {
            return JsonConvert.SerializeObject(searchCustomerModels);
        }

        private List<SearchCustomerModel> DeCompressedSearchCustomerModels(string compressedModel)
        {
            var searchCustomerModels =  JsonConvert.DeserializeObject<List<SearchCustomerModel>>(compressedModel);
            return searchCustomerModels;
        }

    }
}