﻿using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Encryption.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bookmyslot.Api.Common.Encryption
{
    public class AesSymmetricEncryption : ISymmetryEncryption
    {
        private readonly byte[] key;
        private readonly byte[] iv;
        private readonly IRandomNumberGenerator randomNumberGenerator;
        public AesSymmetricEncryption(IRandomNumberGenerator randomNumberGenerator, EncryptionConfiguration encryptionConfiguration)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            
            this.key = this.randomNumberGenerator.GenerateRandomNumber(encryptionConfiguration.SymmetryEncryptionKeyLength);
            this.iv = this.randomNumberGenerator.GenerateRandomNumber(encryptionConfiguration.SymmetryEncryptionIvLength);
        }

        public string Encrypt(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
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
                    return encryptedMessage;
                }
            }
        }
        public string Decrypt(string encryptedMessage)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage))
            {
                return string.Empty;
            }

            using (var aes = new AesCryptoServiceProvider())
            {
                this.SetAesDefaults(aes);

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                    var encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
                    cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    var decryptedMessageBytes = memoryStream.ToArray();
                    var decryptedMessage = Encoding.UTF8.GetString(decryptedMessageBytes);
                    return decryptedMessage;
                }
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
