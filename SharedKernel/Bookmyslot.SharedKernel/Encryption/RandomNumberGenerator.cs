using Bookmyslot.SharedKernel.Contracts.Encryption;
using System.Security.Cryptography;

namespace Bookmyslot.SharedKernel.Encryption
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public byte[] GenerateRandomNumber(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

    }
}
