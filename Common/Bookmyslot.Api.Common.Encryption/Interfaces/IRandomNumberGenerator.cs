namespace Bookmyslot.Api.Common.Encryption.Interfaces
{
    public interface IRandomNumberGenerator
    {
        public byte[] GenerateRandomNumber(int length);
    }
}
