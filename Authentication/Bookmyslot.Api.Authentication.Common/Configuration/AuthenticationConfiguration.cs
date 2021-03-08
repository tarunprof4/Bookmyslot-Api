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
            this.subject = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenSubject).Value;
            this.issuer = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenIssuer).Value;
            this.audience = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenAudience).Value;
            this.tokenexpiryInHours = Convert.ToInt32(configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenExpiryInHours).Value);
            this.secretKey = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenSecretKey).Value; ;

            this.claimEmail = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenClaimSettings).GetSection(AppSettingKeysConstants.JwtTokenClaimEmail).Value; 
        }

        public string Subject => this.subject;

        public string Issuer => this.issuer;

        public string Audience => this.audience;

        public int TokenExpiryInHours => this.tokenexpiryInHours;

        public string SecretKey => this.secretKey;
        public string ClaimEmail => this.claimEmail;
    }
}
