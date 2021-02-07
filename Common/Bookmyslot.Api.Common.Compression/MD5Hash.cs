using Bookmyslot.Api.Common.Compression.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Bookmyslot.Api.Common.Compression
{
    public class MD5Hash : IHashing
    {
        public string Create(object value)
        {
            var serialized = JsonConvert.SerializeObject(value);

            var md5 = MD5.Create();
            var byteArray = Encoding.UTF8.GetBytes(serialized);

            var hashedValue = Convert.ToBase64String(md5.ComputeHash(byteArray));

            // Replaces values to make it url friendly
            hashedValue = hashedValue.Replace("/", "_");
            hashedValue = hashedValue.Replace("+", "-");
            var substring = hashedValue.Substring(0, 22);

            return substring;
        }
    }
}
