using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Common.Contracts.Configuration
{
    public class AppConfiguration
    {
        private readonly string appVersion;

        private readonly string logOutputTemplate;

        private readonly string elasticSearchUrl;

        private readonly string readDatabaseConnectionString;

        public AppConfiguration(IConfiguration configuration)
        {
            this.appVersion = configuration.GetSection(AppSettingKeysConstants.AppVersion).Value;

            this.logOutputTemplate = configuration.GetSection(AppSettingKeysConstants.LogSettings).GetSection(AppSettingKeysConstants.LogOutPutTemplate).Value;

            this.elasticSearchUrl = configuration.GetSection(AppSettingKeysConstants.ElasticSearchUrl).Value;

            this.readDatabaseConnectionString = configuration.GetSection(AppSettingKeysConstants.ConnectionStrings).GetSection(AppSettingKeysConstants.BookMySlotReadDatabase).Value;
        }

        public string AppVersion => this.appVersion;

        public string LogOutputTemplate => this.logOutputTemplate;

        public string ElasticSearchUrl => this.elasticSearchUrl;

        public string ReadDatabaseConnectionString => this.readDatabaseConnectionString;
    }
}
