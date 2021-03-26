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

        internal static Response<AdditionalProfileSettingsModel> CreateAdditionalProfileSettingsModelResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Response<AdditionalProfileSettingsModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            var additionalProfileSettingsModel = ModelFactory.CreateAdditionalProfileSettingsModel(registerCustomerEntity);
            return Response<AdditionalProfileSettingsModel>.Success(additionalProfileSettingsModel);
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

        internal static Response<string> CreateCustomerIdResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Response<string>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            return new Response<string>() { Result = registerCustomerEntity.Id };
        }

        internal static Response<CustomerSettingsModel> CreateCustomerSettingsModelResponse(CustomerSettingsEntity customerSettingsEntity)
        {
            if (customerSettingsEntity == null)
            {
                return Response<CustomerSettingsModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerSettingsNotFound });
            }

            var customerSettingsModel = ModelFactory.CreateCustomerSettingsModel(customerSettingsEntity);
            return Response<CustomerSettingsModel>.Success(customerSettingsModel);
        }
    }

}
