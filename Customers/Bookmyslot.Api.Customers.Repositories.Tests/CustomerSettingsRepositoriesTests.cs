using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class CustomerSettingsRepositoriesTests
    {
        private const string CustomerId = "CustomerId";
        private const string TIMEZONE = "timezone";
        
        private const string LASTNAME = "lastname";
        private const string PROVIDER = "provider";

        private CustomerSettingsRepository customerSettingsRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerSettingsRepository = new CustomerSettingsRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetCustomerSettings_NoRecordsFound_ReturnsEmptyResponse()
        {
            CustomerSettingsEntity customerSettingsEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerSettingsEntity>>>())).Returns(Task.FromResult(customerSettingsEntity));

            var customerSettingsModelResponse = await customerSettingsRepository.GetCustomerSettings(CustomerId);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerSettingsEntity>>>()), Times.Once);
        }


        [Test]
        public async Task GetCustomerSettings_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerSettingsEntity>>>())).Returns(Task.FromResult(DefaultCustomerSettingsEntity()));

            var customerSettingsModelResponse = await customerSettingsRepository.GetCustomerSettings(CustomerId);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerSettingsModelResponse.Result.TimeZone, TIMEZONE);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerSettingsEntity>>>()), Times.Once);
        }


        [Test]
        public async Task UpdateCustomerSettings_ReturnsSuccessResponse()
        {
            var customerSettingsModel = DefaultCreateCustomerSettingsModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var customerSettingsModelResponse = await customerSettingsRepository.UpdateCustomerSettings(CustomerId, customerSettingsModel);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(true, customerSettingsModelResponse.Result);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        private CustomerSettingsModel DefaultCreateCustomerSettingsModel()
        {
            var customerSettingsModel = new CustomerSettingsModel();
            customerSettingsModel.TimeZone = TIMEZONE;
            return customerSettingsModel;
        }

        private CustomerSettingsEntity DefaultCustomerSettingsEntity()
        {
            var customerSettingsEntity = new CustomerSettingsEntity();
            customerSettingsEntity.TimeZone = TIMEZONE;
            return customerSettingsEntity;
        }

     

    }
}