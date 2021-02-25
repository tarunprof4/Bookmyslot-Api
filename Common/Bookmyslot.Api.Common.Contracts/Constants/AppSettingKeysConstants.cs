namespace Bookmyslot.Api.Common.Contracts.Constants
{
    public class AppSettingKeysConstants
    {
        public const string AppVersion = "AppVersion";

        public const string ConnectionStrings = "ConnectionStrings";
        public const string BookMySlotDatabase = "BookmyslotDatabase";
        public const string BookMySlotReadDatabase = "BookmyslotReadDatabase";
        public const string CacheDatabase = "CacheDatabase";

        public const string EmailSettings = "EmailSettings";
        public const string EmailSmtpHost = "SmtpHost";
        public const string EmailPort = "EmailPort";
        public const string EmailUserName = "EmailUserName";
        public const string EmailPassword = "EmailPassword";

        public const string LogSettings = "LogSettings";
        public const string LogOutPutTemplate = "logoutputTemplate";
        public const string StaticLogOutPutTemplate = "staticLogOutputTemplate";

        public const string ElasticSearchUrl = "ElasticSearchUrl";


        public const string AuthenticationSettings = "AuthenticationSettings";
        public const string JwtTokenSettings = "JwtTokenSettings";
        public const string SocialLogin = "SocialLogin";
        public const string GoogleAuthenticationSettings = "GoogleAuthenticationSettings";

        public const string JwtTokenSubject = "Subject";
        public const string JwtTokenIssuer = "Issuer";
        public const string JwtTokenAudience = "Audience";
        public const string JwtTokenExpiryInHours = "ExpiryInHours";
        public const string JwtTokenSecretKey = "SecretKey";

        public const string JwtTokenClaimSettings = "JwtTokenClaimSettings";
        public const string JwtTokenClaimEmail = "ClaimEmail";
        public const string ClientId = "ClientId";
        public const string ClientSecret = "ClientSecret";



        public const string CacheSettings = "CacheSettings";
        public const string CacheHomePageInSeconds = "HomePageInSeconds";
    }
}
