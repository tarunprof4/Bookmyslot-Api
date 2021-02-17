namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface IAuthenticationConfiguration
    {
        string Subject { get; }

        string Issuer { get; }

        string Audience { get; }
        int ExpiryInHours { get; }

        string SecretKey { get; }


        string ClaimEmail { get; }

        string GoogleClientId { get; }
        


    }
}
