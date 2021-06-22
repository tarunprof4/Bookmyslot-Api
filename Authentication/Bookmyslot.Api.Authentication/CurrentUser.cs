using Bookmyslot.Api.Authentication.Common;
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

        private string GetEmailFromClaims()
        {
            var email = this.httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == this.authenticationConfiguration.ClaimEmail).Value;
            return email;
        }


        public async Task<Response<CurrentUserModel>> GetCurrentUserFromCache()
        {
            var email = GetEmailFromClaims();
            var cacheModel = CreateCacheModel(email);
            var currentUserResponse = await GetCurrentUserResponse(email, cacheModel);
            return currentUserResponse;
        }


        public async Task SetCurrentUserInCache(string email)
        {
            var cacheModel = CreateCacheModel(email);
            await GetCurrentUserResponse(email, cacheModel, true);
        }


        private async Task<Response<CurrentUserModel>> GetCurrentUserResponse(string email, CacheModel cacheModel, bool refresh = false)
        {
            return await this.distributedInMemoryCacheBuisness.GetFromCacheAsync(
                                  cacheModel,
                                  () => this.customerBusiness.GetCurrentUserByEmail(email), refresh);
        }

        private CacheModel CreateCacheModel(string email)
        {
            var cacheModel = new CacheModel();
            cacheModel.Key = string.Format(CacheConstants.CustomerInfomationCacheKey, email);
            cacheModel.ExpiryTime = TimeSpan.FromHours(authenticationConfiguration.TokenExpiryInHours);
            return cacheModel;
        }
    }
}
