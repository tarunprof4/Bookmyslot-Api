using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class AdditionalProfileSettingsRepositoryTests
    {
        private const string CustomerId = "CustomerId";
        private const string BioHeadLine = "BioHeadLine";
        private AdditionalProfileSettingsRepository additionalProfileSettingsRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            additionalProfileSettingsRepository = new AdditionalProfileSettingsRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetAdditionalProfileSettingsByCustomerId_NoRecordsFound_ReturnsEmptyResponse()
        {
            RegisterCustomerEntity registerCustomerEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(registerCustomerEntity));

            var response = await additionalProfileSettingsRepository.GetAdditionalProfileSettingsByCustomerId(CustomerId);

            Assert.AreEqual(response.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }


        [Test]
        public async Task GetAdditionalProfileSettingsByCustomerId_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(DefaultCreateRegisterCustomerEntity()));

            var response = await additionalProfileSettingsRepository.GetAdditionalProfileSettingsByCustomerId(CustomerId);

            Assert.AreEqual(response.ResultType, ResultType.Success);
            Assert.AreEqual(response.Result.BioHeadLine, BioHeadLine);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }


        [Test]
        public async Task UpdateAdditionalProfileSettings_ReturnsSuccessResponse()
        {
            var additionalProfileSettingsModel = DefaultCreateAdditionalProfileSettingsModel();
            IEnumerable<RegisterCustomerEntity> customerSettingsEntities = new List<RegisterCustomerEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var response = await additionalProfileSettingsRepository.UpdateAdditionalProfileSettings(CustomerId, additionalProfileSettingsModel);

            Assert.AreEqual(response.ResultType, ResultType.Success);
            Assert.AreEqual(true, response.Result);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }


      



        private AdditionalProfileSettingsModel DefaultCreateAdditionalProfileSettingsModel()
        {
            var additionalProfileSettingsModel= new AdditionalProfileSettingsModel();
            additionalProfileSettingsModel.BioHeadLine = BioHeadLine;
            return additionalProfileSettingsModel;
        }


        private RegisterCustomerEntity DefaultCreateRegisterCustomerEntity()
        {
            var registerCustomerEntity = new RegisterCustomerEntity();
            registerCustomerEntity.BioHeadLine = BioHeadLine;
            return registerCustomerEntity;
        }
    }
}