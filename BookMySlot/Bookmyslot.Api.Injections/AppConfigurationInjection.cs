﻿using Bookmyslot.Api.Authentication.Facebook.Configuration;
using Bookmyslot.Api.Authentication.Google.Configuration;
using Bookmyslot.Api.Azure.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Encryption.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AppConfigurationInjection
    {
        public static void LoadInjections(IServiceCollection services, IConfiguration configuration)
        {
            AppInjections(services, configuration);
        }
        private static void AppInjections(IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfiguration = new CacheConfiguration(configuration);
            services.AddSingleton(cacheConfiguration);
            var emailConfiguration = new EmailConfiguration(configuration);
            services.AddSingleton(emailConfiguration);
            var encryptionConfiguration = new EncryptionConfiguration(configuration);
            services.AddSingleton(encryptionConfiguration);
            var googleAuthenticationConfiguration = new GoogleAuthenticationConfiguration(configuration);
            services.AddSingleton(googleAuthenticationConfiguration);
            var facebookAuthenticationConfiguration = new FacebookAuthenticationConfiguration(configuration);
            services.AddSingleton(facebookAuthenticationConfiguration);
            var azureConfiguration = new AzureConfiguration(configuration);
            services.AddSingleton(azureConfiguration);
        }
    }
}
