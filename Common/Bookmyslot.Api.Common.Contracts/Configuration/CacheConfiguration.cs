using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Contracts.Configuration
{
    public class CacheConfiguration
    {
        private readonly int homePageInSeconds;
        public CacheConfiguration(IConfiguration configuration)
        {
            this.homePageInSeconds = Convert.ToInt32(configuration.GetSection(AppSettingKeysConstants.CacheSettings).GetSection(AppSettingKeysConstants.CacheHomePageInSeconds).Value);
        }

        public int HomePageInSeconds => this.homePageInSeconds;
    }
}
