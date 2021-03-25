using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Common.Contracts.Configuration
{
    public class AppConfiguration
    {
        private readonly string appVersion;

        private readonly string logOutputTemplate;

        private readonly string elasticSearchUrl;

        private readonly string databaseConnectionString;

        private readonly string readDatabaseConnectionString;

        private readonly string cacheDatabaseConnectionString;

        private readonly string blobStorageConnectionString;

        public AppConfiguration(IConfiguration configuration)
        {
            this.appVersion = configuration.GetSection(AppSettingKeysConstants.AppVersion).Value;

            this.logOutputTemplate = configuration.GetSection(AppSettingKeysConstants.LogSettings).GetSection(AppSettingKeysConstants.LogOutPutTemplate).Value;

            this.elasticSearchUrl = configuration.GetSection(AppSettingKeysConstants.ElasticSearchUrl).Value;


            var connectionStringSettings = configuration.GetSection(AppSettingKeysConstants.ConnectionStrings);
            this.databaseConnectionString = connectionStringSettings.GetSection(AppSettingKeysConstants.BookMySlotDatabase).Value;
            this.readDatabaseConnectionString = connectionStringSettings.GetSection(AppSettingKeysConstants.BookMySlotReadDatabase).Value;
            this.cacheDatabaseConnectionString = connectionStringSettings.GetSection(AppSettingKeysConstants.CacheDatabase).Value;
            this.blobStorageConnectionString = connectionStringSettings.GetSection(AppSettingKeysConstants.BlobStroage).Value;
        }

        public string AppVersion => this.appVersion;

        public string LogOutputTemplate => this.logOutputTemplate;

        public string ElasticSearchUrl => this.elasticSearchUrl;

        public string DatabaseConnectionString => this.databaseConnectionString;

        public string ReadDatabaseConnectionString => this.readDatabaseConnectionString;

        public string CacheDatabaseConnectionString => this.cacheDatabaseConnectionString;

        public string BlobStorageConnectionString => this.blobStorageConnectionString;

        
    }
}
