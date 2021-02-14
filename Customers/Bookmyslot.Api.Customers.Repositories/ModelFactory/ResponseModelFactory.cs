using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Response<ProfileSettingsModel> CreateProfileSettingsModelResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Response<ProfileSettingsModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            var customerModel = ModelFactory.CreateProfileSettingsModel(registerCustomerEntity);
            return Response<ProfileSettingsModel>.Success(customerModel);
        }


        internal static Response<CustomerModel> CreateCustomerModelResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Response<CustomerModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            var customerModel = ModelFactory.CreateCustomerModel(registerCustomerEntity);
            return Response<CustomerModel>.Success(customerModel);
        }

        internal static Response<List<CustomerModel>> CreateCustomerModelsResponse(IEnumerable<RegisterCustomerEntity> registerCustomerEntities)
        {
            var customerModels = ModelFactory.CreateCustomerModels(registerCustomerEntities);
            if (customerModels.Count == 0)
            {
                return Response<List<CustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<List<CustomerModel>>() { Result = customerModels };
        }
    }

}
