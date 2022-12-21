namespace Bookmyslot.SharedKernel.Contracts.Encryption
{
    public interface IRandomNumberGenerator
    {
        public byte[] GenerateRandomNumber(int length);
    }
}
