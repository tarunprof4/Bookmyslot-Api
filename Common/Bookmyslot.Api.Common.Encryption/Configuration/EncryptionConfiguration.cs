using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.Extensions.Configuration;
using System;

namespace Bookmyslot.Api.Common.Encryption.Configuration
{
    public class EncryptionConfiguration
    {
        private readonly int hashSaltLength;

        private readonly int symmetryEncryptionKeyLength;

        private readonly int symmetryEncryptionIvLength;

        public EncryptionConfiguration(IConfiguration configuration)
        {
            var encryptionSettings = configuration.GetSection(AppSettingKeysConstants.EncryptionSettings);

            this.hashSaltLength = Convert.ToInt32(encryptionSettings.GetSection(AppSettingKeysConstants.HashSaltLength).Value);
            this.symmetryEncryptionKeyLength = Convert.ToInt32(encryptionSettings.GetSection(AppSettingKeysConstants.SymmetryEncryptionKeyLength).Value);
            this.symmetryEncryptionIvLength = Convert.ToInt32(encryptionSettings.GetSection(AppSettingKeysConstants.SymmetryEncryptionIvLength).Value);
        }

        public int HashSaltLength => this.hashSaltLength;
        public int SymmetryEncryptionKeyLength => this.symmetryEncryptionKeyLength;
        public int SymmetryEncryptionIvLength => this.symmetryEncryptionIvLength;
    }
}



