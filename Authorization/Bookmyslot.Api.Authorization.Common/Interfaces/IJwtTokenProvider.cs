namespace Bookmyslot.Api.Authorization.Common.Interfaces
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(string email);
    }
}
