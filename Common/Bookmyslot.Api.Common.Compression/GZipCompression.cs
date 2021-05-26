using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Bookmyslot.Api.Common.Compression
{
    public class GZipCompression : ICompression
    {
        public T Decompress<T>(string str)
        {
            var unzipped = Unzip(str);
            if (string.IsNullOrEmpty(unzipped))
            {
                return default(T);
            }

            var result = JsonConvert.DeserializeObject<T>(unzipped);

            return result;
        }

        public string Compress<T>(T model)
        {
            var jsonString = JsonConvert.SerializeObject(model);

            var zipped = Zip(jsonString);

            return Convert.ToBase64String(zipped);
        }

        private static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress, true))
            {
                gs.Write(bytes, 0, bytes.Length);
            }

            var gZipBuffer = CopyFrom(mso, bytes);
            return gZipBuffer;
        }

        private static byte[] CopyFrom(Stream sourceStream, byte[] bytes)
        {
            sourceStream.Position = 0;
            var compressed = new byte[sourceStream.Length];
            sourceStream.Read(compressed, 0, compressed.Length);

            var gZipBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);

            return gZipBuffer;
        }

        private static string Unzip(string str)
        {
            var bytes = Convert.FromBase64String(str);

            using (var ms = new MemoryStream())
            {
                var dataLength = BitConverter.ToInt32(bytes, 0);
                ms.Write(bytes, 4, bytes.Length - 4);

                var buffer = new byte[dataLength];
                ms.Position = 0;

                using (var gs = new GZipStream(ms, CompressionMode.Decompress))
                {
                    gs.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

    }
}
