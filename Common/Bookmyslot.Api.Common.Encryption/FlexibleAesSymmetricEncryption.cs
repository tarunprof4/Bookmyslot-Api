using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Encryption.Configuration;
using Bookmyslot.Api.Common.Encryption.Helpers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Bookmyslot.Api.Common.Encryption
{
    public class FlexibleAesSymmetricEncryption : ISymmetryEncryption
    {
        //32 bit randomNumberGenerator
        private readonly byte[] key;
        //16 bit randomNumberGenerator
        private readonly byte[] iv;
        private readonly ILoggerService loggerService;
        public FlexibleAesSymmetricEncryption(EncryptionConfiguration encryptionConfiguration, ILoggerService loggerService)
        {
            this.loggerService = loggerService;
            this.key = Convert.FromBase64String(encryptionConfiguration.SymmetryEncryptionKey); 
            this.iv = Convert.FromBase64String(encryptionConfiguration.SymmetryEncryptionIv);
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
                    
                    return EncryptionHelper.UrlEncode(encryptedMessage);
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

                        var encryptedMessage = EncryptionHelper.UrlDecode(encodedEncryptedMessage);
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

   
        private void SetAesDefaults(AesCryptoServiceProvider aes)
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = this.key;
            aes.IV = this.iv;
        }

    }
}
