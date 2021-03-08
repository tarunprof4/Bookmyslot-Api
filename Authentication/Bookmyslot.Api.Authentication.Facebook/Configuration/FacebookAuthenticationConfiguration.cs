using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;

namespace Bookmyslot.Api.Authentication.Facebook.Configuration
{
    public class FacebookAuthenticationConfiguration
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tokenValidationUrl;
        private readonly string userInfoUrl;

        public FacebookAuthenticationConfiguration(IConfiguration configuration)
        {
            var faceBookSettings = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.SocialLogin).GetSection(AppSettingKeysConstants.FacebookAuthenticationSettings);
            this.clientId = faceBookSettings.GetSection(AppSettingKeysConstants.ClientId).Value;
            this.clientSecret = faceBookSettings.GetSection(AppSettingKeysConstants.ClientSecret).Value;
            this.tokenValidationUrl = faceBookSettings.GetSection(AppSettingKeysConstants.FacebookTokenValidationUrl).Value;
            this.userInfoUrl = faceBookSettings.GetSection(AppSettingKeysConstants.FacebookUserInfoUrl).Value;
        }

        public string ClientId => this.clientId;
        public string ClientSecret => this.clientSecret;
        public string TokenValidationUrl => this.tokenValidationUrl;
        public string UserInfoUrl => this.userInfoUrl;
    }
}
