
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Repositories
{
    public class SearchCustomerRepository : ISearchCustomerRepository
    {
        public Task<Response<IEnumerable<SearchCustomer>>> GetAllCustomers()
        {
            throw new NotImplementedException();
        }
    }
}
