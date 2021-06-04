using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Encryption.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bookmyslot.Api.Common.Encryption
{
    public class AesSymmetricEncryption : ISymmetryEncryption
    {
        private readonly byte[] key;
        private readonly byte[] iv;
        private readonly IRandomNumberGenerator randomNumberGenerator;
        private readonly ILoggerService loggerService;
        public AesSymmetricEncryption(IRandomNumberGenerator randomNumberGenerator, EncryptionConfiguration encryptionConfiguration, ILoggerService loggerService)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            this.loggerService = loggerService;

            this.key = this.randomNumberGenerator.GenerateRandomNumber(encryptionConfiguration.SymmetryEncryptionKeyLength);
            this.iv = this.randomNumberGenerator.GenerateRandomNumber(encryptionConfiguration.SymmetryEncryptionIvLength);
        }

        public string Encrypt(string message)
        {
            if (message == null)
            {
                return string.Empty;
            }


            using (var aes = new AesCryptoServiceProvider())
            {
                this.SetAesDefaults(aes);

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    cryptoStream.Write(messageBytes, 0, messageBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    var encryptedMessageBytes = memoryStream.ToArray();
                    var encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);
                    return UrlEncode(encryptedMessage);
                }
            }
        }
        public string Decrypt(string encodedEncryptedMessage)
        {
            if (string.IsNullOrWhiteSpace(encodedEncryptedMessage))
            {
                return string.Empty;
            }

            try
            {
                using (var aes = new AesCryptoServiceProvider())
                {
                    this.SetAesDefaults(aes);

                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                        var encryptedMessage = this.UrlDecode(encodedEncryptedMessage);
                        var encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
                        cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        var decryptedMessageBytes = memoryStream.ToArray();
                        var decryptedMessage = Encoding.UTF8.GetString(decryptedMessageBytes);
                        return decryptedMessage;
                    }
                }
            }

            catch (Exception exp)
            {
                this.loggerService.Error(exp, string.Empty);
                return string.Empty;
            }

        }

        private string UrlEncode(string encryptedMessage)
        {
            return encryptedMessage.Replace("/", "_").Replace("+", "-");
        }

        private string UrlDecode(string encodedEcryptedMesage)
        {
            return encodedEcryptedMesage.Replace("_", "/").Replace("-", "+");
        }

        private void SetAesDefaults(AesCryptoServiceProvider aes)
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = this.key;
            aes.IV = this.iv;
        }

    }
}
