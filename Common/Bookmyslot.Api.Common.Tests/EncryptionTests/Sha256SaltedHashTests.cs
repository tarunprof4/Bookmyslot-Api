using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Encryption;
using Bookmyslot.Api.Common.Encryption.Configuration;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Bookmyslot.Api.Common.Tests.EncryptionTests
{

    public class Sha256SaltedHashTests
    {
        private Sha256SaltedHash sha256SaltedHash;
        private IRandomNumberGenerator randomNumberGenerator;

        [SetUp]
        public void Setup()
        {
            randomNumberGenerator = new RandomNumberGenerator();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var encryptionConfiguration = new EncryptionConfiguration(configuration);

            sha256SaltedHash = new Sha256SaltedHash(randomNumberGenerator, encryptionConfiguration);
        }

        [TestCase("test")]
        [TestCase("123")]
        [TestCase("asdjkhsdjkhasdjkasdhjk")]
        public void HashMessage_ReturnsHashedMessage(string input)
        {
            var hashedMessage = this.sha256SaltedHash.Create(input);
            Assert.IsTrue(!hashedMessage.Contains("/"));
            Assert.IsTrue(!hashedMessage.Contains("+"));
            Assert.IsNotEmpty(hashedMessage);
        }

        [Test]
        public void InValidHashMessage_ReturnsEmptyHashedMessage()
        {
            var hashedMessage = this.sha256SaltedHash.Create(null);
            Assert.AreEqual(string.Empty, hashedMessage);
        }
    }
}
