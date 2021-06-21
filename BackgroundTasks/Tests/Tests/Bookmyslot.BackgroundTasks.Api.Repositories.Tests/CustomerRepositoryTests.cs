using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.BackgroundTasks.Api.Contracts;
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

        [SetUp]
        public void SetUp()
        {
            elasticClientMock = new Mock<ElasticClient>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerRepository = new CustomerRepository(elasticClientMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task CreateValidCustomer_ReturnsSuccessResponse()
        {
            var customerModel = DefaultCreateCustomerModel();

            var indexResponse = new IndexResponse();


            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IndexResponse>>>())).Returns(Task.FromResult(indexResponse));

            var createCustomerModelResponse = await customerRepository.CreateCustomer(customerModel);

            Assert.AreEqual(createCustomerModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(createCustomerModelResponse.Result, true);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
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
        }

        private CustomerModel DefaultCreateCustomerModel()
        {
            var customerModel = new CustomerModel();
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