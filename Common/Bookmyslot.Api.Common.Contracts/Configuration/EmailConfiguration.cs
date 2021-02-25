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
            this.emailStmpHost = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailSmtpHost).Value;
            this.emailPort = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailPort).Value;
            this.emailUserName = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailUserName).Value;
            this.emailPassword = configuration.GetSection(AppSettingKeysConstants.EmailSettings).GetSection(AppSettingKeysConstants.EmailPassword).Value;
        }

        public string EmailStmpHost => this.emailStmpHost;
        public string EmailPort => this.emailPort;
        public string EmailUserName => this.emailUserName;
        public string EmailPassword => this.emailPassword;
    }
}
