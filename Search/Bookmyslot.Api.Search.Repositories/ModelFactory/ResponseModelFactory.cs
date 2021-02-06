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
            var searchCustomerModels = ModelFactory.CreateSearchCustomerModels(searchCustomerEntities);
            if (searchCustomerModels.Count == 0)
            {
                return Response<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<List<SearchCustomerModel>>() { Result = searchCustomerModels };
        }
    }

}
