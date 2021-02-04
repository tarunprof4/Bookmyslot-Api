using Bookmyslot.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerRepository
    {
        Task<Response<IEnumerable<SearchCustomer>>> GetAllCustomers();
    }
}
