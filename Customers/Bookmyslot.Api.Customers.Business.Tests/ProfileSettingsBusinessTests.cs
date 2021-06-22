using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.Tests
{
    [TestFixture]
    public class ProfileSettingsBusinessTests
    {
        private const string CUSTOMERID = "customerid";
        private const string EMAIL = "a@gmail.com";
        private const string FIRSTNAME = "fisrtname";
        private const string LASTNAME = "lastname";
        private const string GENDER = "gender";
        private const string BIOHEADLINE = "bioheadline";
        private ProfileSettingsBusiness profileSettingsBusiness;
        private Mock<IProfileSettingsRepository> profileSettingRepositoryMock;
        private Mock<IBlobRepository> blobRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            profileSettingRepositoryMock = new Mock<IProfileSettingsRepository>();
            blobRepositoryMock = new Mock<IBlobRepository>();
            profileSettingsBusiness = new ProfileSettingsBusiness(profileSettingRepositoryMock.Object, blobRepositoryMock.Object);
        }

      

        [Test]
        public async Task GetProfileSettingsByEmail_ValidEmailId_CallsGetProfileSettingsByEmailIdRepository()
        {
            var profileSettingsResponse = await profileSettingsBusiness.GetProfileSettingsByCustomerId(EMAIL);

            profileSettingRepositoryMock.Verify((m => m.GetProfileSettingsByCustomerId(It.IsAny<string>())), Times.Once());
        }



        [Test]
        public async Task UpdateProfileSettings_ValidProfileSettingsDetails_ReturnsSuccess()
        {
            var profileSettingsResponse = await profileSettingsBusiness.UpdateProfileSettings(DefaultCreateProfileSettingsModel(), CUSTOMERID);

            profileSettingRepositoryMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Once());
        }



        [Test]
        public async Task UpdateProfilePicture_InValidFile_ReturnsSuccessResponse()
        {
            Response<string> blobRepositoryResponseMock = new Response<string>() { ResultType = ResultType.ValidationError };
            blobRepositoryMock.Setup(a => a.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(blobRepositoryResponseMock));

            var profilePictureResponse = await profileSettingsBusiness.UpdateProfilePicture(null, CUSTOMERID, FIRSTNAME);

            blobRepositoryMock.Verify((m => m.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            profileSettingRepositoryMock.Verify((m => m.UpdateProfilePicture(It.IsAny<string>(), It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task UpdateProfilePicture_ValidFile_ReturnsSuccessResponse()
        {
            Response<string> blobRepositoryResponseMock = new Response<string>() { ResultType = ResultType.Success };
            blobRepositoryMock.Setup(a => a.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(blobRepositoryResponseMock));
            Response<bool> profileSettingRepositoryResponseMock = new Response<bool>() { ResultType = ResultType.Success };
            profileSettingRepositoryMock.Setup(a => a.UpdateProfilePicture(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(profileSettingRepositoryResponseMock));

            var profilePictureResponse = await profileSettingsBusiness.UpdateProfilePicture(null, CUSTOMERID, FIRSTNAME);

            blobRepositoryMock.Verify((m => m.UpdateProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            profileSettingRepositoryMock.Verify((m => m.UpdateProfilePicture(It.IsAny<string>(), It.IsAny<string>())), Times.Once());
        }


        private ProfileSettingsModel DefaultCreateProfileSettingsModel()
        {
            var profileSettingsModel = new ProfileSettingsModel();
            profileSettingsModel.FirstName = FIRSTNAME;
            profileSettingsModel.LastName = LASTNAME;
            profileSettingsModel.Gender = GENDER;
            profileSettingsModel.Email = EMAIL;
            return profileSettingsModel;
        }
    }
}