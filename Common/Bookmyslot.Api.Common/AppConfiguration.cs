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



        public AppConfiguration(IConfiguration configuration)
        {
            this.appVersion = configuration.GetSection(AppConfigurationConstants.AppVersion).Value;

            this.emailStmpHost = configuration.GetSection(AppConfigurationConstants.EmailSettings).GetSection(AppConfigurationConstants.EmailSmtpHost).Value;
            this.emailPort = configuration.GetSection(AppConfigurationConstants.EmailSettings).GetSection(AppConfigurationConstants.EmailPort).Value;
            this.emailUserName = configuration.GetSection(AppConfigurationConstants.EmailSettings).GetSection(AppConfigurationConstants.EmailUserName).Value;
            this.emailPassword = configuration.GetSection(AppConfigurationConstants.EmailSettings).GetSection(AppConfigurationConstants.EmailPassword).Value;

            this.logOutputTemplate = configuration.GetSection(AppConfigurationConstants.LogSettings).GetSection(AppConfigurationConstants.LogOutPutTemplate).Value;
        }

        public string AppVersion => this.appVersion;

        public string EmailStmpHost => this.emailStmpHost;
        public string EmailPort => this.emailPort;
        public string EmailUserName => this.emailUserName;
        public string EmailPassword => this.emailPassword;

        public string LogOutputTemplate => this.logOutputTemplate;

    }
}
