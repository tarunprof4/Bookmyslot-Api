using Bookmyslot.Api.Search.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
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


        [Test]
        public async Task SearchCustomers_ValidSearchKey_ReturnsSuccessResponse()
        {
            string searchKey = SearchKey;
            var customer = await searchCustomerBusiness.SearchCustomers(searchKey);

            searchCustomerRepositoryMock.Verify((m => m.SearchCustomers(searchKey)), Times.Once());
        }
    }
}