using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


    }
}
