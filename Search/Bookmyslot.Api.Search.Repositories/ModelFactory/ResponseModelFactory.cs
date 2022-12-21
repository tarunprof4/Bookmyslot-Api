using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.SharedKernel.ValueObject;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Result<List<SearchCustomerModel>> CreateSearchCustomerModelsResponse(ISearchResponse<SearchCustomerModel> searchCustomerModels)
        {
            if (searchCustomerModels.Documents.Count == 0)
            {
                return Result<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Result<List<SearchCustomerModel>>() { Value = searchCustomerModels.Documents.ToList() };
        }

        internal static Result<SearchCustomerModel> CreateSearchCustomerModelResponse(SearchCustomerEntity searchCustomerEntity)
        {
            if (searchCustomerEntity == null)
            {
                return Result<SearchCustomerModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            var SearchCustomerModel = ModelFactory.CreateSearchCustomerModel(searchCustomerEntity);
            return Result<SearchCustomerModel>.Success(SearchCustomerModel);
        }
    }

}
