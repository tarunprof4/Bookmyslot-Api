using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Bookmyslot.Api.File.Contracts.Configuration
{
    public class ImageConfigurationSingleton
    {
        private static ImageConfigurationSingleton instance;
        private static readonly object padlock = new object();

        public ReadOnlyDictionary<string, string> Extensions { get; }
        public ReadOnlyDictionary<string, List<byte[]>> ExtensionSignatures { get; }

        

        private ImageConfigurationSingleton(Dictionary<string, string> extensions, Dictionary<string, List<byte[]>> extensionSignatures)
        {
            this.Extensions = new ReadOnlyDictionary<string, string>(extensions);
            this.ExtensionSignatures = new ReadOnlyDictionary<string, List<byte[]>>(extensionSignatures);

        }

        public static ImageConfigurationSingleton CreateInstance(Dictionary<string, string> extensions, Dictionary<string, List<byte[]>> extensionSignatures)
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ImageConfigurationSingleton(extensions, extensionSignatures);
                    }
                }
            }
            return instance;
        }

        public static ImageConfigurationSingleton GetInstance()
        {
            if (instance == null)
            {
                throw new InvalidOperationException(ExceptionMessagesConstants.ImageConfigurationSingletonSingletonNotInitialized);
            }
            return instance;
        }


        public bool isImageExtensionValid(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !this.Extensions.ContainsKey(ext))
            {
                return false;
            }

            return true;
        }

        public bool isImageExtensionSignatureValid(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var isSignatureValid = isFileExtensionSignatures(this.ExtensionSignatures, file, ext);
            if (isSignatureValid)
            {
                return true;
            }

            return false;
        }



        private bool isFileExtensionSignatures(ReadOnlyDictionary<string, List<byte[]>> fileSignatures, IFormFile file, string ext)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    var signatures = fileSignatures[ext];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    return signatures.Any(signature =>
                        headerBytes.Take(signature.Length).SequenceEqual(signature));
                }
            }
        }


    }
}
