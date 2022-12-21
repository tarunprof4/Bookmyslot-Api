using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Database;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class ProfileSettingsRepositoryTests
    {
        private const string CustomerId = "CustomerId";

        private const string FIRSTNAME = "firstname";
        private const string LASTNAME = "lastname";
        private const string GENDER = "GENDER";
        private const string EMAIL = "email";
        private const string PROVIDER = "provider";

        private ProfileSettingsRepository profileSettingsRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            profileSettingsRepository = new ProfileSettingsRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetCustomerSettings_NoRecordsFound_ReturnsEmptyResponse()
        {
            RegisterCustomerEntity registerCustomerEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(registerCustomerEntity));

            var profileSettingsModelResponse = await profileSettingsRepository.GetProfileSettingsByCustomerId(CustomerId);

            Assert.AreEqual(profileSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }


        [Test]
        public async Task GetCustomerSettings_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(DefaultCreateRegisterCustomerEntity()));

            var profileSettingsModelResponse = await profileSettingsRepository.GetProfileSettingsByCustomerId(CustomerId);

            Assert.AreEqual(profileSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(profileSettingsModelResponse.Value.FirstName, FIRSTNAME);
            Assert.AreEqual(profileSettingsModelResponse.Value.LastName, LASTNAME);
            Assert.AreEqual(profileSettingsModelResponse.Value.Email, EMAIL);
            Assert.AreEqual(profileSettingsModelResponse.Value.Gender, GENDER);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }


        [Test]
        public async Task UpdateProfileSettings_ReturnsSuccessResponse()
        {
            var profileSettingsModel = DefaultCreateProfileSettingsModel();
            IEnumerable<RegisterCustomerEntity> customerSettingsEntities = new List<RegisterCustomerEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var customerSettingsModelResponse = await profileSettingsRepository.UpdateProfileSettings(profileSettingsModel, CustomerId);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(true, customerSettingsModelResponse.Value);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }


        [Test]
        public async Task UpdateProfilePicture_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var customerSettingsModelResponse = await profileSettingsRepository.UpdateProfilePicture(CustomerId, "");

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(true, customerSettingsModelResponse.Value);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }



        private ProfileSettingsModel DefaultCreateProfileSettingsModel()
        {
            var profileSettingsModel = new ProfileSettingsModel();
            profileSettingsModel.FirstName = FIRSTNAME;
            profileSettingsModel.LastName = LASTNAME;
            profileSettingsModel.Email = EMAIL;
            profileSettingsModel.Gender = GENDER;
            return profileSettingsModel;
        }


        private RegisterCustomerEntity DefaultCreateRegisterCustomerEntity()
        {
            var registerCustomerEntity = new RegisterCustomerEntity();
            registerCustomerEntity.FirstName = FIRSTNAME;
            registerCustomerEntity.LastName = LASTNAME;
            registerCustomerEntity.Email = EMAIL;
            registerCustomerEntity.Gender = GENDER;
            return registerCustomerEntity;
        }
    }
}