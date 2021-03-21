using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
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
        private const string Id = "Id";
        private const string UserName= "UserName";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string FullName = "FirstNameLastName";
        private const string PhotoUrl = "PhotoUrl";
        private SearchCustomerRepository searchCustomerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;
        private Mock<ICompression> compressionMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            compressionMock = new Mock<ICompression>();
            searchCustomerRepository = new SearchCustomerRepository(dbConnectionMock.Object, dbInterceptorMock.Object, compressionMock.Object);
        }

        [Test]
        public async Task SavePreProcessedSearchedCustomers_ReturnsSuccessResponse()
        {
            compressionMock.Setup(m => m.Compress(It.IsAny<List<SearchCustomerModel>>())).Returns(CompressedSearchCustomerModels(DefaultCreateSearchCustomerModels()));
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var customerSettingsModelResponse = await searchCustomerRepository.SavePreProcessedSearchedCustomers(SearchKey, DefaultCreateSearchCustomerModels());

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            compressionMock.Verify(m => m.Compress(It.IsAny<List<SearchCustomerModel>>()), Times.Once);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        [Test]
        public async Task GetPreProcessedSearchedCustomers_HasNoRecords_ReturnsEmptyResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<List<SearchCustomerModel>>>>())).Returns(Task.FromResult(new List<SearchCustomerModel>()));
            compressionMock.Setup(m => m.Decompress<List<SearchCustomerModel>>(It.IsAny<string>())).Returns(new List<SearchCustomerModel>());

            var customerSettingsModelResponse = await searchCustomerRepository.GetPreProcessedSearchedCustomers(SearchKey);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            compressionMock.Verify(m => m.Compress(It.IsAny<List<SearchCustomerModel>>()), Times.Once);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }


        [Test]
        public async Task GetPreProcessedSearchedCustomers_HasRecords_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<List<SearchCustomerModel>>>>())).Returns(Task.FromResult(DefaultCreateSearchCustomerModels()));
            compressionMock.Setup(m => m.Decompress<List<SearchCustomerModel>>(It.IsAny<string>())).Returns(DefaultCreateSearchCustomerModels());

            var customerSettingsModelResponse = await searchCustomerRepository.GetPreProcessedSearchedCustomers(SearchKey);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            compressionMock.Verify(m => m.Compress(It.IsAny<List<SearchCustomerModel>>()), Times.Once);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }


        [Test]
        public async Task SearchCustomers_HasNoRecords_ReturnsEmptyResponse()
        {
            IEnumerable<SearchCustomerEntity> searchCustomerEntities = new List<SearchCustomerEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>())).Returns(Task.FromResult(searchCustomerEntities));

            var customerSettingsModelResponse = await searchCustomerRepository.SearchCustomers(SearchKey);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task SearchCustomers_HasRecords_ReturnsSuccessResponse()
        {
            IEnumerable<SearchCustomerEntity> searchCustomerEntities =  DefaultCreateSearchEntities();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>())).Returns(Task.FromResult(searchCustomerEntities)));

            var customerSettingsModelResponse = await searchCustomerRepository.SearchCustomers(SearchKey);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SearchCustomerEntity>>>>()), Times.Once);
        }




        private List<SearchCustomerModel> DefaultCreateSearchCustomerModels()
        {
            var searchCustomerModels = new List<SearchCustomerModel>();
            var searchCustomerModel = new SearchCustomerModel();
            searchCustomerModel.Id = Id;
            searchCustomerModel.UserName = UserName;
            searchCustomerModel.FullName = FullName;
            searchCustomerModel.PhotoUrl = PhotoUrl;
            searchCustomerModels.Add(searchCustomerModel);

            searchCustomerModel = new SearchCustomerModel();
            searchCustomerModel.Id = Id;
            searchCustomerModel.UserName = UserName;
            searchCustomerModel.FullName = FullName;
            searchCustomerModel.PhotoUrl = PhotoUrl;
            searchCustomerModels.Add(searchCustomerModel);

            return searchCustomerModels;
        }


        private IEnumerable<SearchCustomerEntity> DefaultCreateSearchEntities()
        {
            var searchCustomerEntities = new List<SearchCustomerEntity>();
            var searchCustomerEntity = new SearchCustomerEntity();
            searchCustomerEntity.Id = Id;
            searchCustomerEntity.UserName = UserName;
            searchCustomerEntity.FirstName = FullName;
            searchCustomerEntity.LastName = FullName;
            searchCustomerEntity.PhotoUrl = PhotoUrl;
            searchCustomerEntities.Add(searchCustomerEntity);

            searchCustomerEntity = new SearchCustomerEntity();
            searchCustomerEntity.Id = Id;
            searchCustomerEntity.UserName = UserName;
            searchCustomerEntity.FirstName = FullName;
            searchCustomerEntity.LastName = FullName;
            searchCustomerEntity.PhotoUrl = PhotoUrl;
            searchCustomerEntities.Add(searchCustomerEntity);

            return searchCustomerEntities;
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