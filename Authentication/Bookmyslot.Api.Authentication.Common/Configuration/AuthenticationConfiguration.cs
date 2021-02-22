using Bookmyslot.Api.Authentication.Common.Interfaces;
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

        private readonly int expiryInHours;

        private readonly string secretKey;

        private readonly string claimEmail;

        private readonly string googleClientId;
        public AuthenticationConfiguration(IConfiguration configuration)
        {
            this.subject = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenSubject).Value;
            this.issuer = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenIssuer).Value;
            this.audience = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenAudience).Value;
            this.expiryInHours = Convert.ToInt32(configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenExpiryInHours).Value);
            this.secretKey = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenSecretKey).Value; ;

            this.googleClientId = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.SocialLogin).GetSection(AppSettingKeysConstants.GoogleAuthenticationSettings).GetSection(AppSettingKeysConstants.ClientId).Value;


            this.claimEmail = configuration.GetSection(AppSettingKeysConstants.AuthenticationSettings).GetSection(AppSettingKeysConstants.JwtTokenSettings).GetSection(AppSettingKeysConstants.JwtTokenClaimSettings).GetSection(AppSettingKeysConstants.JwtTokenClaimEmail).Value; 
        }

        public string Subject => this.subject;

        public string Issuer => this.issuer;

        public string Audience => this.audience;

        public int ExpiryInHours => this.expiryInHours;

        public string SecretKey => this.secretKey;

        public string GoogleClientId => this.googleClientId;

        public string ClaimEmail => this.claimEmail;
    }
}
