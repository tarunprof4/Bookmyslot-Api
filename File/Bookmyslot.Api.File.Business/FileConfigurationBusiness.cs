using Bookmyslot.Api.File.Contracts.Configuration;
using Bookmyslot.Api.File.Contracts.Constants;
using Bookmyslot.Api.File.Contracts.Interfaces;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.File.Business
{
    public class FileConfigurationBusiness : IFileConfigurationBusiness
    {
        public void CreateImageConfigurationInformation()
        {
            Dictionary<string, string> imageExtensions = CreateImageExtensions();
            Dictionary<string, List<byte[]>> imageExtensionsSignatures = CreateImageExtensionSignatures();

            ImageConfigurationSingleton.CreateInstance(imageExtensions, imageExtensionsSignatures);
        }

        public ImageConfigurationSingleton GetImageConfigurationInformation()
        {
            return ImageConfigurationSingleton.GetInstance();
        }

        private Dictionary<string, string> CreateImageExtensions()
        {
            var extensions = new Dictionary<string, string>();
            extensions.Add(ImageConstants.Jpeg, ImageConstants.Jpeg);
            extensions.Add(ImageConstants.Jpg, ImageConstants.Jpg);
            extensions.Add(ImageConstants.Png, ImageConstants.Png);
            extensions.Add(ImageConstants.Gif, ImageConstants.Gif);
            return extensions;
        }

        private Dictionary<string, List<byte[]>> CreateImageExtensionSignatures()
        {
            Dictionary<string, List<byte[]>> fileExtensionSignatures = new Dictionary<string, List<byte[]>>
{
    { ImageConstants.Jpeg, new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
        }
    },
    { ImageConstants.Jpg, new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
        }
    },
      { ImageConstants.Gif, new List<byte[]>
        {
            new byte[] { 0x47, 0x49, 0x46, 0x38 },
        }
    },

      { ImageConstants.Png, new List<byte[]>
        {
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
        }
    },
};

            return fileExtensionSignatures;
        }
    }
}
