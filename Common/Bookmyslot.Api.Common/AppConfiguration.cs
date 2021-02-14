using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Common
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly string appVersion;

        private readonly string emailStmpHost;
        private readonly string emailPort;
        private readonly string emailUserName;
        private readonly string emailPassword;

        private readonly string logOutputTemplate;

        private readonly string elasticSearchUrl;

        private readonly string readDatabaseConnectionString;

        public AppConfiguration(IConfiguration configuration)
        {
            this.appVersion = configuration.GetSection(AppSettingKeysConstants.AppVersion).Value;

            this.emailStmpHost = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailSmtpHost).Value;
            this.emailPort = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailPort).Value;
            this.emailUserName = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailUserName).Value;
            this.emailPassword = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailPassword).Value;

            this.logOutputTemplate = configuration.GetSection(AppSettingKeysConstants.LogSettings).GetSection(AppSettingKeysConstants.LogOutPutTemplate).Value;

            this.elasticSearchUrl = configuration.GetSection(AppSettingKeysConstants.ElasticSearchUrl).Value;

            this.readDatabaseConnectionString = configuration.GetSection(AppSettingKeysConstants.ConnectionStrings).GetSection(AppSettingKeysConstants.BookMySlotReadDatabase).Value;
        }

        public string AppVersion => this.appVersion;

        public string EmailStmpHost => this.emailStmpHost;
        public string EmailPort => this.emailPort;
        public string EmailUserName => this.emailUserName;
        public string EmailPassword => this.emailPassword;

        public string LogOutputTemplate => this.logOutputTemplate;

        public string ElasticSearchUrl => this.elasticSearchUrl;

        public string ReadDatabaseConnectionString => this.readDatabaseConnectionString;
    }
}
