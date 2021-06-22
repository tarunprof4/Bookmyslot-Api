using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;
using System;

namespace Bookmyslot.Api.Authentication.Common.Configuration
{
    public class AuthenticationConfiguration
    {
        private readonly string subject;

        private readonly string issuer;

        private readonly string audience;

        private readonly int tokenexpiryInHours;

        private readonly string secretKey;

        private readonly string claimEmail;


        public AuthenticationConfiguration(IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings);

            this.subject = authenticationSettings.GetSection(AppSettingKeysConstants.JwtTokenSubject).Value;
            this.issuer = authenticationSettings.GetSection(AppSettingKeysConstants.JwtTokenIssuer).Value;
            this.audience = authenticationSettings.GetSection(AppSettingKeysConstants.JwtTokenAudience).Value;
            this.tokenexpiryInHours = Convert.ToInt32(authenticationSettings.GetSection(AppSettingKeysConstants.JwtTokenExpiryInHours).Value);
            this.secretKey = authenticationSettings.GetSection(AppSettingKeysConstants.JwtTokenSecretKey).Value;

            this.claimEmail = authenticationSettings.GetSection(AppSettingKeysConstants.JwtTokenClaimSettings).GetSection(AppSettingKeysConstants.JwtTokenClaimEmail).Value;
        }

        public string Subject => this.subject;

        public string Issuer => this.issuer;

        public string Audience => this.audience;

        public int TokenExpiryInHours => this.tokenexpiryInHours;

        public string SecretKey => this.secretKey;
        public string ClaimEmail => this.claimEmail;
    }
}
