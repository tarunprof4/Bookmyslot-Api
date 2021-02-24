using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Constants.cs;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness;
        private readonly AuthenticationConfiguration authenticationConfiguration;
        private readonly ICustomerBusiness customerBusiness;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUser(IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness, AuthenticationConfiguration authenticationConfiguration, ICustomerBusiness customerBusiness, IHttpContextAccessor httpContextAccessor)
        {
            this.distributedInMemoryCacheBuisness = distributedInMemoryCacheBuisness;
            this.authenticationConfiguration = authenticationConfiguration;
            this.customerBusiness = customerBusiness;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetEmailFromClaims()
        {
            var email = this.httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == this.authenticationConfiguration.ClaimEmail).Value;
            return email;
        }

        public async Task<Response<string>> GetCurrentUserFromCache()
        {
            var email = GetEmailFromClaims();
            var cacheModel = CreateCacheModel(email);
            var customerIdResponse =
                  await
                  this.distributedInMemoryCacheBuisness.GetFromCacheAsync(
                      cacheModel,
                      () => this.customerBusiness.GetCustomerIdByEmail(email));

            return customerIdResponse;
        }

        public async Task SetCurrentUserInCache(string email)
        {
            var cacheModel = CreateCacheModel(email);
            var customerIdResponse =
                  await
                  this.distributedInMemoryCacheBuisness.GetFromCacheAsync(
                      cacheModel,
                      () => this.customerBusiness.GetCustomerIdByEmail(email));
        }

        private CacheModel CreateCacheModel(string email)
        {
            var cacheModel = new CacheModel();
            cacheModel.Key = string.Format(CacheConstants.CustomerInfomationCacheKey, email);
            cacheModel.ExpiryTimeUtc = new TimeSpan(authenticationConfiguration.AuthenticationTokenExpiryInHours, 0, 0);
            return cacheModel;
        }
    }
}
