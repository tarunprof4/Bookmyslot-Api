using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Common.Contracts.Configuration
{
    public class EmailConfiguration
    {
        private readonly string emailStmpHost;
        private readonly string emailPort;
        private readonly string emailUserName;
        private readonly string emailPassword;


        public EmailConfiguration(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(AppSettingKeysConstants.EmailSettings);

            this.emailStmpHost = emailSettings.GetSection(AppSettingKeysConstants.EmailSmtpHost).Value;
            this.emailPort = emailSettings.GetSection(AppSettingKeysConstants.EmailPort).Value;
            this.emailUserName = emailSettings.GetSection(AppSettingKeysConstants.EmailUserName).Value;
            this.emailPassword = emailSettings.GetSection(AppSettingKeysConstants.EmailPassword).Value;
        }

        public string EmailStmpHost => this.emailStmpHost;
        public string EmailPort => this.emailPort;
        public string EmailUserName => this.emailUserName;
        public string EmailPassword => this.emailPassword;
    }
}
