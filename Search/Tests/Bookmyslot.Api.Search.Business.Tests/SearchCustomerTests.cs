using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Business.Tests
{
    public class SearchCustomerTests
    {
        private const string SearchKey = "SearchKey";
        private SearchCustomerBusiness searchCustomerBusiness;
        private Mock<ISearchCustomerRepository> searchCustomerRepositoryMock;

        [SetUp]
        public void Setup()
        {
            searchCustomerRepositoryMock = new Mock<ISearchCustomerRepository>();
            searchCustomerBusiness = new SearchCustomerBusiness(searchCustomerRepositoryMock.Object);
        }

        [TestCase("")]
        [TestCase("   ")]
        public async Task SearchCustomers_InvalidSearchKey_ReturnsValidationErrorResponse(string searchKey)
        {
            var customer = await searchCustomerBusiness.SearchCustomers(searchKey);

            Assert.AreEqual(customer.ResultType, ResultType.ValidationError);
            Assert.AreEqual(customer.Messages.First(), AppBusinessMessagesConstants.InValidSearchKey);
        }

        [Test]
        public async Task SearchCustomers_ValidSearchKey_ReturnsSuccessResponse()
        {
            string searchKey = SearchKey;
            var customer = await searchCustomerBusiness.SearchCustomers(searchKey);

            searchCustomerRepositoryMock.Verify((m => m.SearchCustomers(searchKey)), Times.Once());
        }
    }
}