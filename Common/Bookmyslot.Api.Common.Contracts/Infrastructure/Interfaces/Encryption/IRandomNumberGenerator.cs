namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption
{
    public interface IRandomNumberGenerator
    {
        public byte[] GenerateRandomNumber(int length);
    }
}
