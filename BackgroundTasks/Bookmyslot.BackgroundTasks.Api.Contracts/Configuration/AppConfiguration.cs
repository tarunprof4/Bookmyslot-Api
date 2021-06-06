using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Configuration
{

    public class AppConfiguration
    {
        private readonly string appVersion;

        private readonly string logOutputTemplate;

        private readonly string elasticSearchUrl;

        public AppConfiguration(IConfiguration configuration)
        {
            this.appVersion = configuration.GetSection(AppSettingKeysConstants.AppVersion).Value;
            this.logOutputTemplate = configuration.GetSection(AppSettingKeysConstants.LogSettings).GetSection(AppSettingKeysConstants.LogOutPutTemplate).Value;
            this.elasticSearchUrl = configuration.GetSection(AppSettingKeysConstants.ElasticSearchUrl).Value;
        }

        public string AppVersion => this.appVersion;

        public string LogOutputTemplate => this.logOutputTemplate;

        public string ElasticSearchUrl => this.elasticSearchUrl;

    }
}
