namespace Bookmyslot.Api.Authorization.Common.Constants
{
    public class JwtTokenConstants
    {
        public const string Subject = "Bookmyslot";
        public const string Issuer = "Bookmyslot";
        public const string Audience = "Bookmyslot-client";

        public const string SecretKey = "asdv234234^&%&^%&^hjsdfb2%%%";

        public const string ClaimEmail = "Email";

        public const int TokenExpiryHours = 500;
    }
}
