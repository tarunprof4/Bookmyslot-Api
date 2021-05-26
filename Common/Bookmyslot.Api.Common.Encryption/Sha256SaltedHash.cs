using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Encryption.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bookmyslot.Api.Common.Encryption
{
    public class Sha256SaltedHash : IHashing
    {
        private readonly IRandomNumberGenerator randomNumberGenerator;
        private readonly byte[] salt;
        public Sha256SaltedHash(IRandomNumberGenerator randomNumberGenerator, EncryptionConfiguration encryptionConfiguration)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            this.salt = randomNumberGenerator.GenerateRandomNumber(encryptionConfiguration.HashSaltLength);
        }

        public string Create(string message)
        {
            if (message == null)
            {
                return string.Empty;
            }

            using (var sha256 = SHA256.Create())
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                var hasedBytes = sha256.ComputeHash(Combine(messageBytes, this.salt));
                var hashedMessage = Convert.ToBase64String(hasedBytes);

                return HttpUtility.UrlEncode(hashedMessage);
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
