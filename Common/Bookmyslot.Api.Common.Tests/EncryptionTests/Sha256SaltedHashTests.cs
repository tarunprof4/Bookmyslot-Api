using Bookmyslot.Api.Common.Encryption;
using Bookmyslot.Api.Common.Encryption.Interfaces;
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
            sha256SaltedHash = new Sha256SaltedHash(randomNumberGenerator);
        }

        [TestCase("test")]
        [TestCase("123")]
        [TestCase("asdjkhsdjkhasdjkasdhjk")]
        public void HashMessage_ReturnsHashedMessage(string input)
        {
            var hashedMessage = this.sha256SaltedHash.Create(input);
            Assert.IsTrue(!hashedMessage.Contains("/"));
            Assert.IsTrue(!hashedMessage.Contains("+"));
        }
    }
}
