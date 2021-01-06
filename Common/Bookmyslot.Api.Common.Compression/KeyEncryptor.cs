using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.Security.Cryptography;
using System.Web;

namespace Bookmyslot.Api.Common.Compression
{
    public class KeyEncryptor : IKeyEncryptor
    {
        public string Encrypt(string strValue)
        {
            var encryptKey = CompressionConstants.BookMySlotEncryptionKey;
            byte[] results;
            var utf8 = new System.Text.UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(encryptKey));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider();
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] dataToEncrypt = utf8.GetBytes(strValue);
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            var strResult = Convert.ToBase64String(results);
            strResult = HttpUtility.UrlEncode(strResult);
            return strResult;
        }

        public string Decrypt(string strValue)
        {
            strValue = HttpUtility.UrlDecode(strValue);
            byte[] results;
            var encryptKey = CompressionConstants.BookMySlotEncryptionKey;
            var utf8 = new System.Text.UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(encryptKey));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider();
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] dataToDecrypt;
            try
            {
                dataToDecrypt = Convert.FromBase64String(strValue);
            }
            catch (Exception)
            {
                dataToDecrypt = Convert.FromBase64String(strValue.Replace(" ", "+"));
            }
            try
            {
                var Decryptor = tdesAlgorithm.CreateDecryptor();

                results = Decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            var strResult = utf8.GetString(results);
            return strResult;
        }
    }
}
