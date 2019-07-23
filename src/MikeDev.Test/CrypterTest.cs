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

        }

        /// <summary>
        /// This test requires Crypter to correctly compute hash of a byte array.
        /// </summary>
        [Test]
        public void TestB()
        {

        }

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

            string EncryptedData = Crypter.Encrypt(Data, passphrase);

            byte[] DecryptedData = Crypter.Decrypt(EncryptedData, passphrase);

            Assert.IsTrue(Crypter.ComputeHash(DecryptedData) == Crypter.ComputeHash(EncryptedData));
        }
    }
}
