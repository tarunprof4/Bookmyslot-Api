//using Bookmyslot.Api.Common.Contracts;
//using Bookmyslot.Api.Common.Contracts.Constants;
//using Bookmyslot.Api.Customers.Contracts;
//using Bookmyslot.Api.Customers.Contracts.Interfaces;
//using Bookmyslot.Api.Location.Contracts.Configuration;
//using Moq;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Bookmyslot.Api.Customers.Business.Tests
//{
//    [TestFixture]
//    public class CustomerAdditionalInformationBusinessTests
//    {
//        private const string CUSTOMERID = "customerid";
//        private const string ValidTimeZone = "Asia/Kolkata";
//        private const string ValidTimeZoneCountry = "India";
//        private const string InvalidTimeZone = "InvalidTimeZone";

//        private Mock<ICustomerAdditionalInformationRepository> customerAdditionalInformationRepositoryMock;
//        private NodaTimeZoneLocationConfiguration nodaTimeZoneLocationConfiguration;
//        private CustomerAdditionalInformationBusiness customerAdditionalInformationBusiness;

//        [SetUp]
//        public void SetUp()
//        {
//            this.customerAdditionalInformationRepositoryMock = new Mock<ICustomerAdditionalInformationRepository>();
//            this.nodaTimeZoneLocationConfiguration = DefaultNodaTimeLocationConfiguration();
//            this.customerAdditionalInformationBusiness = new CustomerAdditionalInformationBusiness(this.customerAdditionalInformationRepositoryMock.Object, this.nodaTimeZoneLocationConfiguration);
//        }

//        [Test]
//        public async Task UpdateCustomerAdditionalInformation_InvalidTimeZone_ReturnsValidationErrorResponse()
//        {
//            var customerAdditionalInformationModelResponse = await this.customerAdditionalInformationBusiness.UpdateCustomerAdditionalInformation(CUSTOMERID, DefaultInValidCustomerAdditionalInformationModel());

//            Assert.AreEqual(customerAdditionalInformationModelResponse.ResultType, ResultType.ValidationError);
//            Assert.AreEqual(customerAdditionalInformationModelResponse.Messages.First(), AppBusinessMessagesConstants.InValidTimeZone);
//            this.customerAdditionalInformationRepositoryMock.Verify((m => m.UpdateCustomerAdditionalInformation(It.IsAny<string>(), It.IsAny<CustomerAdditionalInformationModel>())), Times.Never());
//        }




//        [Test]
//        public async Task UpdateCustomerAdditionalInformation_ValidTimeZone_ReturnsSuccessResponse()
//        {
//            var updateCustomerAdditionInformationResponse = new Response<bool>() { Result = true };
//            this.customerAdditionalInformationRepositoryMock.Setup(a => a.UpdateCustomerAdditionalInformation(It.IsAny<string>(), It.IsAny<CustomerAdditionalInformationModel>())).Returns(Task.FromResult(updateCustomerAdditionInformationResponse));
//            var customerAdditionalInformationModelResponse = await this.customerAdditionalInformationBusiness.UpdateCustomerAdditionalInformation(CUSTOMERID, DefaultValidCustomerAdditionalInformationModel());

//            Assert.AreEqual(customerAdditionalInformationModelResponse.ResultType, ResultType.Success);
//            this.customerAdditionalInformationRepositoryMock.Verify((m => m.UpdateCustomerAdditionalInformation(It.IsAny<string>(), It.IsAny<CustomerAdditionalInformationModel>())), Times.Once());
//        }


//        private NodaTimeZoneLocationConfiguration DefaultNodaTimeLocationConfiguration()
//        {
//            Dictionary<string, string> zoneWithCountryId = new Dictionary<string, string>();
//            zoneWithCountryId.Add(ValidTimeZone, ValidTimeZoneCountry);
//            var countries = zoneWithCountryId.Values.Distinct().ToList();
//            return new NodaTimeZoneLocationConfiguration(zoneWithCountryId, countries);
//        }

//        private CustomerAdditionalInformationModel DefaultValidCustomerAdditionalInformationModel()
//        {
//            return new CustomerAdditionalInformationModel() { TimeZone = ValidTimeZone };
//        }

//        private CustomerAdditionalInformationModel DefaultInValidCustomerAdditionalInformationModel()
//        {
//            return new CustomerAdditionalInformationModel() { TimeZone = InvalidTimeZone };
//        }

//    }
//}