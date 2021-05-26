using Bookmyslot.Api.Common.Encryption.Constants;
using Bookmyslot.Api.Common.Encryption.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bookmyslot.Api.Common.Encryption
{
    public class Sha256SaltedHash : IHashing
    {
        private readonly IRandomNumberGenerator randomNumberGenerator;
        private readonly byte[] salt;
        public Sha256SaltedHash(IRandomNumberGenerator randomNumberGenerator)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            this.salt = randomNumberGenerator.GenerateRandomNumber(EncryptionConstants.SaltLength);
        }

        public string Create(string message)
        {
            using (var sha256 = SHA256.Create())
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                var hasedBytes =  sha256.ComputeHash(Combine(messageBytes, this.salt));
                var hashedMessage = Convert.ToBase64String(hasedBytes);

                hashedMessage = hashedMessage.Replace("/", "_");
                hashedMessage = hashedMessage.Replace("+", "-");
                var substring = hashedMessage.Substring(0, 22);

                return substring;
            }
        }


        private byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

      
    }
}
