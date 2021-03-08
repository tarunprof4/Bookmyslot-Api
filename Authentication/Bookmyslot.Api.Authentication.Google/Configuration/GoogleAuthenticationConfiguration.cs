using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Authentication.Google.Configuration
{
    public class GoogleAuthenticationConfiguration
    {
        private readonly string googleClientId;

        public GoogleAuthenticationConfiguration(IConfiguration configuration)
        {
            var googleSettings = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.SocialLogin).GetSection(AppSettingKeysConstants.GoogleAuthenticationSettings);
            this.googleClientId = googleSettings.GetSection(AppSettingKeysConstants.ClientId).Value;
        }

        public string GoogleClientId => this.googleClientId;
    }
}
