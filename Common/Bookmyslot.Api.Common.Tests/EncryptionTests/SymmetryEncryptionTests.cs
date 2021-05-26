using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Encryption;
using NUnit.Framework;

namespace Bookmyslot.Api.Common.Tests.EncryptionTests
{
    public class SymmetryEncryptionTests
    {
        private AesSymmetricEncryption aesSymmetricEncryption;
        private IRandomNumberGenerator randomNumberGenerator;


        [SetUp]
        public void Setup()
        {
            randomNumberGenerator = new RandomNumberGenerator();
            aesSymmetricEncryption = new AesSymmetricEncryption(randomNumberGenerator);
        }

        [TestCase("test", "test")]
        [TestCase("", "")]
        [TestCase(null, "")]
        [TestCase("123", "123")]
        [TestCase("asdjkhsdjkhasdjkasdhjk", "asdjkhsdjkhasdjkasdhjk")]
        public void EncryptMessage_DecryptMessageBack_ReturnsOriginalMessage(string input, string output)
        {
            var encryptedMessage = this.aesSymmetricEncryption.Encrypt(input);
            var decryptedMessage = this.aesSymmetricEncryption.Decrypt(encryptedMessage);
            Assert.AreEqual(output, decryptedMessage);
        }
    }
}
