using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Result<ProfileSettingsModel> CreateProfileSettingsModelResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Result<ProfileSettingsModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            var customerModel = ModelFactory.CreateProfileSettingsModel(registerCustomerEntity);
            return Result<ProfileSettingsModel>.Success(customerModel);
        }

        internal static Result<AdditionalProfileSettingsModel> CreateAdditionalProfileSettingsModelResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Result<AdditionalProfileSettingsModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            var additionalProfileSettingsModel = ModelFactory.CreateAdditionalProfileSettingsModel(registerCustomerEntity);
            return Result<AdditionalProfileSettingsModel>.Success(additionalProfileSettingsModel);
        }



        internal static Result<CustomerModel> CreateCustomerModelResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Result<CustomerModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            var customerModel = ModelFactory.CreateCustomerModel(registerCustomerEntity);
            return Result<CustomerModel>.Success(customerModel);
        }

        internal static Result<List<CustomerModel>> CreateCustomerModelsResponse(IEnumerable<RegisterCustomerEntity> registerCustomerEntities)
        {
            var customerModels = ModelFactory.CreateCustomerModels(registerCustomerEntities);
            if (customerModels.Count == 0)
            {
                return Result<List<CustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Result<List<CustomerModel>>() { Value = customerModels };
        }

        internal static Result<string> CreateCustomerIdResponse(RegisterCustomerEntity registerCustomerEntity)
        {
            if (registerCustomerEntity == null)
            {
                return Result<string>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerNotFound });
            }

            return new Result<string>() { Value = registerCustomerEntity.Id };
        }

        internal static Result<CustomerSettingsModel> CreateCustomerSettingsModelResponse(CustomerSettingsEntity customerSettingsEntity)
        {
            if (customerSettingsEntity == null)
            {
                return Result<CustomerSettingsModel>.Empty(new List<string>() { AppBusinessMessagesConstants.CustomerSettingsNotFound });
            }

            var customerSettingsModel = ModelFactory.CreateCustomerSettingsModel(customerSettingsEntity);
            return Result<CustomerSettingsModel>.Success(customerSettingsModel);
        }
    }

}
