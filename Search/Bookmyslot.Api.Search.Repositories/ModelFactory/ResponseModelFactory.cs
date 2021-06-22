using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Response<List<SearchCustomerModel>> CreateSearchCustomerModelsResponse(ISearchResponse<SearchCustomerModel> searchCustomerModels)
        {
            if (searchCustomerModels.Documents.Count == 0)
            {
                return Response<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<List<SearchCustomerModel>>() { Result = searchCustomerModels.Documents.ToList() };
        }

        internal static Response<SearchCustomerModel> CreateSearchCustomerModelResponse(SearchCustomerEntity searchCustomerEntity)
        {
            if (searchCustomerEntity == null)
            {
                return Response<SearchCustomerModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            var SearchCustomerModel = ModelFactory.CreateSearchCustomerModel(searchCustomerEntity);
            return Response<SearchCustomerModel>.Success(SearchCustomerModel);
        }
    }

}
