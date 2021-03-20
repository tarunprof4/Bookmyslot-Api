using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Repositories.Enitites;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ModelFactory
    {
        internal static SearchCustomerModel CreateSearchCustomerModel(SearchCustomerEntity searchCustomerEntity)
        {
            return new SearchCustomerModel()
            {
                Id = searchCustomerEntity.UniqueId,
                UserName= searchCustomerEntity.UserName,
                FullName = string.Format("{0} {1}", searchCustomerEntity.FirstName, searchCustomerEntity.LastName),
                PhotoUrl = searchCustomerEntity.PhotoUrl
            };
        }


        internal static List<SearchCustomerModel> CreateSearchCustomerModels(IEnumerable<SearchCustomerEntity> searchCustomerEntities)
        {
            List<SearchCustomerModel> searchCustomerModels = new List<SearchCustomerModel>();
            foreach (var searchCustomerEntity in searchCustomerEntities)
            {
                searchCustomerModels.Add(CreateSearchCustomerModel(searchCustomerEntity));
            }
            return searchCustomerModels;
        }
    }
}
