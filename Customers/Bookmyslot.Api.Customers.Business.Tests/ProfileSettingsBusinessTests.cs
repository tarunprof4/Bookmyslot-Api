using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;
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
        private Mock<ICustomerRepository> customerRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            profileSettingRepositoryMock = new Mock<IProfileSettingsRepository>();
            customerRepositoryMock = new Mock<ICustomerRepository>();
            profileSettingsBusiness = new ProfileSettingsBusiness(profileSettingRepositoryMock.Object, customerRepositoryMock.Object);
        }

        [TestCase("")]
        [TestCase("   ")]
        public async Task GetProfileSettingsByEmail_InvalidEmailId_ReturnsValidationErrorResponse(string email)
        {
            var profileSettingsResponse = await profileSettingsBusiness.GetProfileSettingsByCustomerId(email);

            Assert.AreEqual(profileSettingsResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(profileSettingsResponse.Messages.First(), AppBusinessMessagesConstants.EmailIdNotValid);
            profileSettingRepositoryMock.Verify((m => m.GetProfileSettingsByCustomerId(It.IsAny<string>())), Times.Never());
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
            var profileSettingsModel = CreateProfileSettingsModel();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { ResultType= ResultType.Success };
            customerRepositoryMock.Setup(a => a.GetCustomerById(CUSTOMERID)).Returns(Task.FromResult(customerModelResponse));

            var profileSettingsResponse = await profileSettingsBusiness.UpdateProfileSettings(profileSettingsModel, CUSTOMERID);

            customerRepositoryMock.Verify((m => m.GetCustomerById(It.IsAny<string>())), Times.Once());
            profileSettingRepositoryMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task UpdateProfileSettings_MissingProfileSettingsDetails_ReturnsValidationError()
        {
            var profileSettingsResponse = await profileSettingsBusiness.UpdateProfileSettings(null, CUSTOMERID);

            Assert.IsTrue(profileSettingsResponse.Messages.Contains(AppBusinessMessagesConstants.ProfileSettingDetailsMissing));
            Assert.AreEqual(profileSettingsResponse.ResultType, ResultType.ValidationError);
        }

        [Test]
        public async Task UpdateProfileSettings_CustomerDoesntExists_ReturnsCustomerNotFoundError()
        {

            var profileSettingsModel = CreateProfileSettingsModel();
            Response<CustomerModel> customerModelResponse = new Response<CustomerModel>() { ResultType = ResultType.Empty };
            customerRepositoryMock.Setup(a => a.GetCustomerById(CUSTOMERID)).Returns(Task.FromResult(customerModelResponse));

            var profileSettingsResponse = await profileSettingsBusiness.UpdateProfileSettings(profileSettingsModel, CUSTOMERID);

            customerRepositoryMock.Verify((m => m.GetCustomerById(It.IsAny<string>())), Times.Once());
            profileSettingRepositoryMock.Verify((m => m.UpdateProfileSettings(It.IsAny<ProfileSettingsModel>(), It.IsAny<string>())), Times.Never());
            Assert.AreEqual(profileSettingsResponse.ResultType, ResultType.Empty);
            Assert.IsTrue(profileSettingsResponse.Messages.Contains(AppBusinessMessagesConstants.CustomerNotFound));
        }


        private ProfileSettingsModel CreateProfileSettingsModel()
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