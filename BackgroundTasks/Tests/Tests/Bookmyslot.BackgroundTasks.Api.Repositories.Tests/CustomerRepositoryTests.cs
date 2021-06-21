using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Search.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Constants;
using Moq;
using Nest;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Repositories.Tests
{
    public class CustomerRepositoryTests
    {
        private const string ID = "Id";
        private const string USERNAME = "UserName";
        private const string BIOHEADLINE = "BioHeadline";
        private const string EMAIL = "a@gmail.com";
        private const string FIRSTNAME = "fisrtname";
        private const string LASTNAME = "lastname";

        private CustomerRepository customerRepository;
        private Mock<ElasticClient> elasticClientMock;
        private Mock<IDbInterceptor> dbInterceptorMock;
        private Mock<ILoggerService> loggerServiceMock;

        [SetUp]
        public void SetUp()
        {
            elasticClientMock = new Mock<ElasticClient>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            loggerServiceMock = new Mock<ILoggerService>();
            customerRepository = new CustomerRepository(elasticClientMock.Object, dbInterceptorMock.Object, loggerServiceMock.Object);
        }

        [Test]
        public async Task CreateValidCustomer_ReturnsSuccessResponse()
        {
            var customerModel = DefaultCreateCustomerModel();
            var mockSearchResponse = new Mock<IndexResponse>();
            mockSearchResponse.Setup(x => x.IsValid).Returns(true);
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>())).Returns(Task.FromResult(mockSearchResponse.Object));

            var createCustomerModelResponse = await customerRepository.CreateCustomer(customerModel);

            Assert.AreEqual(createCustomerModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(createCustomerModelResponse.Result, true);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>()), Times.Once);
            loggerServiceMock.Verify(m => m.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CreateInValidCustomer_ReturnsErrorResponse()
        {
            var customerModel = DefaultCreateCustomerModel();
            var indexResponse = new IndexResponse();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>())).Returns(Task.FromResult(indexResponse));

            var createCustomerModelResponse = await customerRepository.CreateCustomer(customerModel);

            Assert.AreEqual(createCustomerModelResponse.ResultType, ResultType.Error);
            Assert.AreEqual(createCustomerModelResponse.Result, false);
            Assert.IsTrue(createCustomerModelResponse.Messages.Contains(AppBusinessMessagesConstants.CreateCustomerFailed));
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>()), Times.Once);
            loggerServiceMock.Verify(m => m.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UpdateValidCustomer_ReturnsSuccessResponse()
        {
            var customerModel = DefaultCreateCustomerModel();
            var mockSearchResponse = new Mock<IndexResponse>();
            mockSearchResponse.Setup(x => x.IsValid).Returns(true);
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>())).Returns(Task.FromResult(mockSearchResponse.Object));

            var createCustomerModelResponse = await customerRepository.UpdateCustomer(customerModel);

            Assert.AreEqual(createCustomerModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(createCustomerModelResponse.Result, true);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>()), Times.Once);
            loggerServiceMock.Verify(m => m.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateInValidCustomer_ReturnsErrorResponse()
        {
            var customerModel = DefaultCreateCustomerModel();
            var indexResponse = new IndexResponse();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>())).Returns(Task.FromResult(indexResponse));

            var createCustomerModelResponse = await customerRepository.UpdateCustomer(customerModel);

            Assert.AreEqual(createCustomerModelResponse.ResultType, ResultType.Error);
            Assert.AreEqual(createCustomerModelResponse.Result, false);
            Assert.IsTrue(createCustomerModelResponse.Messages.Contains(AppBusinessMessagesConstants.UpdateCustomerFailed));
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>()), Times.Once);
            loggerServiceMock.Verify(m => m.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }


      

        private SearchCustomerModel DefaultCreateCustomerModel()
        {
            var customerModel = new SearchCustomerModel();
            customerModel.Id = ID;
            customerModel.Email = EMAIL;
            customerModel.UserName = USERNAME;
            
            customerModel.FirstName = FIRSTNAME;
            customerModel.LastName = LASTNAME;

            customerModel.BioHeadLine = BIOHEADLINE;
            return customerModel;
        }

    }
}