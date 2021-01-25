using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {


        internal static Response<CustomerModel> CreateCustomerModelResponse(CustomerEntity customerEntity)
        {
            if (customerEntity == null)
            {
                return Response<CustomerModel>.Empty(new List<string>() { AppBusinessMessages.CustomerNotFound });
            }

            var customerModel = ModelFactory.CreateCustomerModel(customerEntity);
            return Response<CustomerModel>.Success(customerModel);
        }

        internal static Response<List<CustomerModel>> CreateCustomerModelsResponse(IEnumerable<CustomerEntity> customerEntities)
        {
            var customerModels = ModelFactory.CreateCustomerModels(customerEntities);
            if (customerModels.Count == 0)
            {
                return Response<List<CustomerModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<List<CustomerModel>>() { Result = customerModels };
        }
    }

}
