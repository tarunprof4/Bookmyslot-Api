namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(string email);
    }
}
