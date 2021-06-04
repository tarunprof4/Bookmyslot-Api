using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;
using System;

namespace Bookmyslot.Api.Common.Encryption.Configuration
{
    public class EncryptionConfiguration
    {
        private readonly int hashSaltLength;

        private readonly string symmetryEncryptionKey;

        private readonly string symmetryEncryptionIv;

        public EncryptionConfiguration(IConfiguration configuration)
        {
            var encryptionSettings = configuration.GetSection(AppSettingKeysConstants.EncryptionSettings);

            this.hashSaltLength = Convert.ToInt32(encryptionSettings.GetSection(AppSettingKeysConstants.HashSaltLength).Value);
            this.symmetryEncryptionKey = encryptionSettings.GetSection(AppSettingKeysConstants.SymmetryEncryptionKey).Value;
            this.symmetryEncryptionIv = encryptionSettings.GetSection(AppSettingKeysConstants.SymmetryEncryptionIv).Value;
        }

        public int HashSaltLength => this.hashSaltLength;
        public string SymmetryEncryptionKey => this.symmetryEncryptionKey;
        public string SymmetryEncryptionIv => this.symmetryEncryptionIv;
    }
}



