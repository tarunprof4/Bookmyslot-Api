using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;
using System;

namespace Bookmyslot.Api.Cache.Contracts.Configuration
{
    public class CacheConfiguration
    {
        private readonly int homePageInSeconds;

        private readonly int customerSearchInSeconds;
        public CacheConfiguration(IConfiguration configuration)
        {
            var cacheSettings = configuration.GetSection(AppSettingKeysConstants.CacheSettings);
            this.homePageInSeconds = Convert.ToInt32(cacheSettings.GetSection(AppSettingKeysConstants.CacheHomePageInSeconds).Value);
            this.customerSearchInSeconds = Convert.ToInt32(cacheSettings.GetSection(AppSettingKeysConstants.CacheHomePageInSeconds).Value);
        }

        public int HomePageInSeconds => this.homePageInSeconds;

        public int CustomerSearchInSeconds => this.customerSearchInSeconds;
    }
}
