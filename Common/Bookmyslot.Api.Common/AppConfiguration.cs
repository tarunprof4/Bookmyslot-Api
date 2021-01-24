﻿using Bookmyslot.Api.Common.Contracts.Interfaces;
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
            this.appVersion = configuration.GetSection("AppVersion").Value;

            this.emailStmpHost = configuration.GetSection("EmailSettings").GetSection("SmtpHost").Value;
            this.emailPort = configuration.GetSection("EmailSettings").GetSection("EmailPort").Value;
            this.emailUserName = configuration.GetSection("EmailSettings").GetSection("EmailUserName").Value;
            this.emailPassword = configuration.GetSection("EmailSettings").GetSection("EmailPassword").Value;

            this.logOutputTemplate = configuration.GetSection("LogSettings").GetSection("outputTemplate").Value;
        }

        public string AppVersion => this.appVersion;

        public string EmailStmpHost => this.emailStmpHost;
        public string EmailPort => this.emailPort;
        public string EmailUserName => this.emailUserName;
        public string EmailPassword => this.emailPassword;

        public string LogOutputTemplate => this.logOutputTemplate;

    }
}
