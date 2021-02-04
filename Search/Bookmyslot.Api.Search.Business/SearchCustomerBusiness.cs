using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Business
{
    public class SearchCustomerBusiness : ISearchCustomerBusiness
    {
        private readonly ISearchCustomerRepository searchCustomerRepository;
        public SearchCustomerBusiness(ISearchCustomerRepository searchCustomerRepository)
        {
            this.searchCustomerRepository = searchCustomerRepository;
        }
        public async Task<Response<IEnumerable<SearchCustomer>>> SearchCustomers(string searchKey)
        {
            return await this.searchCustomerRepository.SearchCustomers(searchKey);
        }
    }
}
