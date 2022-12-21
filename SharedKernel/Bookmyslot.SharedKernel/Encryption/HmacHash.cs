//using System;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;

//namespace Bookmyslot.SharedKernel.Encryption
//{
//    public class HmacHash
//    {
//        public (string, string) CreateHash(string message)
//        {
//            if (message == null)
//            {
//                return (string.Empty, string.Empty);
//            }

//            byte[] messageHashBytes, messageSaltBytes;
//            CreateHdmcHash(message, out messageHashBytes, out messageSaltBytes);

//            var hashedMessage = HttpUtility.UrlEncode(Convert.ToBase64String(messageHashBytes));
//            var saltMessage = HttpUtility.UrlEncode(Convert.ToBase64String(messageSaltBytes));

//            return (hashedMessage, saltMessage);
//        }

//        private void CreateHdmcHash(string message, out byte[] messageHash, out byte[] messageSalt)
//        {
//            using (var hmac = new HMACSHA256())
//            {
//                messageSalt = hmac.Key;
//                messageHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
//            }
//        }

//    }
//}
