using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.Encryption;
using Bookmyslot.SharedKernel.Encryption.Configuration;
using Bookmyslot.SharedKernel.Logging;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Bookmyslot.SharedKernel.Tests.EncryptionTests
{
    public class AesSymmetryEncryptionTests
    {
        private FlexibleAesSymmetricEncryption aesSymmetricEncryption;
        private ILoggerService loggerService;


        [SetUp]
        public void Setup()
        {
            loggerService = new LoggerService();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var encryptionConfiguration = new EncryptionConfiguration(configuration);

            aesSymmetricEncryption = new FlexibleAesSymmetricEncryption(encryptionConfiguration, loggerService);
        }

        [TestCase("test", "test")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("123", "123")]
        [TestCase("asdjkhsdjkhasdjkasdhjk", "asdjkhsdjkhasdjkasdhjk")]
        public void EncryptMessage_DecryptMessageBack_ReturnsOriginalMessage(string input, string output)
        {
            var encryptedMessage = this.aesSymmetricEncryption.Encrypt(input);
            var decryptedMessage = this.aesSymmetricEncryption.Decrypt(encryptedMessage);
            Assert.IsTrue(!encryptedMessage.Contains("/"));
            Assert.IsTrue(!encryptedMessage.Contains("+"));
            Assert.AreEqual(output, decryptedMessage);
        }

        [TestCase(null)]
        public void EncryptInValidMessage_ReturnsEmptyMessage(string input)
        {
            var encryptedMessage = this.aesSymmetricEncryption.Encrypt(input);
            Assert.AreEqual(string.Empty, encryptedMessage);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("asdjkhsdjkhasdjkasdhjk")]
        public void DeEncryptInValidMessage_ReturnsEmptyMessage(string input)
        {
            var decryptedMessage = this.aesSymmetricEncryption.Decrypt(input);
            Assert.AreEqual(string.Empty, decryptedMessage);
        }

    }
}
