using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Repositories.Enitites;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Response<List<SearchCustomerModel>> CreateSearchCustomerModelsResponse(IEnumerable<SearchCustomerEntity> searchCustomerEntities)
        {
            var customerModels = ModelFactory.CreateSearchCustomerModels(searchCustomerEntities);
            if (customerModels.Count == 0)
            {
                return Response<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<List<SearchCustomerModel>>() { Result = customerModels };
        }
    }

}
