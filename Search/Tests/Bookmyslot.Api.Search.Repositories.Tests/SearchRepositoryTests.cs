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


    public class SearchRepositoryTests
    {
        private const string SearchKey = "SearchKey";
        private const string Id = "Id";
        private const string UserName = "UserName";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string FullName = "FirstNameLastName";
        private const string PhotoUrl = "PhotoUrl";
        private SearchRepository searchCustomerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;
        private Mock<ICompression> compressionMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            compressionMock = new Mock<ICompression>();
            searchCustomerRepository = new SearchRepository(dbConnectionMock.Object, dbInterceptorMock.Object, compressionMock.Object);
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
            SearchEntity searchEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchEntity>>>())).Returns(Task.FromResult(searchEntity));

            var customerSettingsModelResponse = await searchCustomerRepository.GetPreProcessedSearchedCustomers(SearchKey);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            compressionMock.Verify(m => m.Compress(It.IsAny<List<SearchCustomerModel>>()), Times.Never);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchEntity>>>()), Times.Once);
        }


        [Test]
        public async Task GetPreProcessedSearchedCustomers_HasRecords_ReturnsSuccessResponse()
        {
            SearchEntity searchEntity = DefaultCreateSearchEntity();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchEntity>>>())).Returns(Task.FromResult(searchEntity));
            compressionMock.Setup(m => m.Decompress<List<SearchCustomerModel>>(It.IsAny<string>())).Returns(DefaultCreateSearchCustomerModels());

            var customerSettingsModelResponse = await searchCustomerRepository.GetPreProcessedSearchedCustomers(SearchKey);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            compressionMock.Verify(m => m.Decompress<List<SearchCustomerModel>>(It.IsAny<string>()), Times.Once);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SearchEntity>>>()), Times.Once);
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

     

    }
}
