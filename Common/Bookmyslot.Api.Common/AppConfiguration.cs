using Bookmyslot.Api.Common.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Common
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly string emailStmpHost;
        private readonly string emailPort;
        private readonly string emailUserName;
        private readonly string emailPassword;

        private readonly string appVersion;
       

        public AppConfiguration(IConfiguration configuration)
        {
            this.emailStmpHost = configuration.GetSection("SmtpHost").Value;
            this.emailPort = configuration.GetSection("EmailPort").Value;
            this.emailUserName = configuration.GetSection("EmailUserName").Value;
            this.emailPassword = configuration.GetSection("EmailPassword").Value;

            
    }

        public string EmailStmpHost => this.emailStmpHost;
        public string EmailPort => this.emailPort;
        public string EmailUserName => this.emailUserName;
        public string EmailPassword => this.emailPassword;
    }
}
