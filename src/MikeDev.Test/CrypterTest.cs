using MikeDev.Cryptography;
using NUnit.Framework;

namespace MikeDev.Test
{
    [TestFixture]
    internal class CrypterTest
    {
        /// <summary>
        /// This test requires Crypter to correctly compute hash of a string.
        /// </summary>
        [Test]
        public void TestA()
        {
            string Data = "MikeDev@";
            string Prehashed = "bc2e57af02a38d37cdba471089d4d193b6c87ddd36a4ca5c6a4ab5c326cdf5c23aad3946ef3c420e9dc1fd421929575cda46b25d66bd5eae9301649838f31f5d";

            string Hash = Crypter.ComputeHash(Data);
            Assert.IsTrue(Hash == Prehashed);
        }

        /// <summary>
        /// This test requires Crypter to correctly compute hash of a byte array.
        /// </summary>
        [Test]
        public void TestB()
        {

        }

        string Bridge;

        /// <summary>
        /// This test requires Crypter to correcly encrypt/decrypt a byte array.
        /// </summary>
        [Test]
        public void TestC()
        {
            byte[] Data = new byte[]
            {
                0x12,
                0x58,
                0xa3,
                0x85,
                0x61,
                0x00
            };
            string passphrase = "MikeDev@";

            Bridge = Crypter.Encrypt(Data, passphrase);

            byte[] DecryptedData = Crypter.Decrypt(Bridge, passphrase);

            Assert.IsTrue(Crypter.ComputeHash(DecryptedData) == Crypter.ComputeHash(Data));

            // Given wrong passphrase
            Assert.Throws<System.Exception>(_TestC);
        }

        public void _TestC()
        {
            Crypter.Decrypt(Bridge, "Wrong password");
        }
    }
}
