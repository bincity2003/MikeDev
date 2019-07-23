using System;
using System.Security.Cryptography;
using System.Text;

namespace MikeDev.Cryptography
{
    /// <summary>
    /// Crypter exposes many APIs related to cryptography.
    /// </summary>
    public static class Crypter
    {
        private static readonly RandomNumberGenerator RNG = RandomNumberGenerator.Create();
        private static readonly SHA512 SHA512 = SHA512.Create();
        private static Rfc2898DeriveBytes PBKDF2;

        #region ComputeHash suite

        /// <summary>
        /// Compute hash of a System.Byte[] object.
        /// </summary>
        /// <param name="s">Byte array to be hashed.</param>
        public static string ComputeHash(byte[] byteArray)
        {
            return _InternalComputeHash(byteArray);
        }

        /// <summary>
        /// Compute hash of a System.String object.
        /// </summary>
        /// <param name="s">String to be hashed.</param>
        public static string ComputeHash(string s)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return ComputeHash(Encoding.UTF8.GetBytes(s));
        }

        /// <summary>
        /// Compute hash of a System.Object object.
        /// </summary>
        /// <param name="obj">Object to be hashed.</param>
        public static string ComputeHash(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return ComputeHash(obj.ToString());
        }

        /// <summary>
        /// Compute hash of an object type <typeparamref name="T"/>
        /// </summary>
        /// <param name="obj"><typeparamref name="T"/> object to be hashed.</param>
        public static string ComputeHash<T>(T obj) where T : ICrypto
        {
            return ComputeHash(obj.GetUniqueInfo());
        }

        /// <summary>
        /// Compute hash with SHA512 algorithm. Internal.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private static string _InternalComputeHash(byte[] byteArray)
        {
            byteArray = SHA512.ComputeHash(byteArray);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteArray.Length; i++)
            {
                builder.Append(byteArray[i].ToString("x2"));
            }

            return builder.ToString();
        }

        #endregion

        #region Encrypt/Decrypt suite

        /// <summary>
        /// Encrypt a byte array using passphrase.
        /// </summary>
        /// <param name="data">Data to be encrypted.</param>
        /// <param name="passphrase">Passphrase to encrypt data.</param>
        public static string Encrypt(byte[] data, string passphrase)
        {
            byte[] EncryptionKey = _InternalRandomKeygen(16);
            byte[][] EncryptionParams = _InternalEncryptionKeygen(EncryptionKey, passphrase);

            byte[] EncryptedKey = EncryptionParams[0];
            byte[] IntegrityKey = EncryptionParams[1];
            byte[] EKSalt = EncryptionParams[2];
            byte[] IV = EncryptionParams[3];

            byte[] EncryptedData = _InternalEncrypt(data, EncryptionKey, IV);

            byte[] result = _InternalArrayMerger(EncryptedData, EncryptedKey, IntegrityKey, EKSalt, IV);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decrypt a base64 string using passphrase
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="passphrase"></param>
        public static byte[] Decrypt(string encryptedData, string passphrase)
        {
            byte[] Data = Convert.FromBase64String(encryptedData);

            byte[] IV = new byte[16];
            byte[] EKSalt = new byte[8];
            byte[] IntegrityKey = new byte[16];
            byte[] EncryptedKey = new byte[16];

            int Length = Data.Length - 16;
            Array.Copy(Data, Length, IV, 0, 16);

            Length -= 8;
            Array.Copy(Data, Length, EKSalt, 0, 8);

            Length -= 16;
            Array.Copy(Data, Length, IntegrityKey, 0, 16);

            Length -= 16;
            Array.Copy(Data, Length, EncryptedKey, 0, 16);

            byte[] EncryptedData = new byte[Length];
            Array.Copy(Data, 0, EncryptedData, 0, Length);

            byte[] DecryptionKey = _InternalDecryptionKeygen(EncryptedKey, IntegrityKey, EKSalt, IV, passphrase);

            return _InternalDecrypt(EncryptedData, DecryptionKey, IV);
        }

        #region External CryptoSuite

        /// <summary>
        /// Internal CryptoSuite.
        /// </summary>
        private static byte[] _InternalEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.Zeros;

            aes.Key = key;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            return _InternalCryptoPerformer(data, encryptor);
        }

        /// <summary>
        /// Internal CryptoSuite.
        /// </summary>
        private static byte[] _InternalDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.Zeros;

            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            return _InternalCryptoPerformer(data, decryptor);
        }

        /// <summary>
        /// Internal CryptoSuite.
        /// </summary>
        private static byte[] _InternalCryptoPerformer(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new System.IO.MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }

        /// <summary>
        /// Internal CryptoSuite.
        /// </summary>
        private static byte[][] _InternalEncryptionKeygen(byte[] EncryptionKey, string passphrase)
        {
            byte[] IV = new byte[16];
            byte[] Salt = new byte[8];
            RNG.GetBytes(IV);
            RNG.GetBytes(Salt);

            PBKDF2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passphrase), Salt, 2048, HashAlgorithmName.SHA256);
            byte[] temp = PBKDF2.GetBytes(32);
            byte[] key2 = temp[0..16];
            byte[] IntegrityKey = temp[16..^0];

            byte[] EncryptedKey = _InternalEncrypt(EncryptionKey, key2, IV);

            return new byte[][] { EncryptedKey, IntegrityKey, Salt, IV };
        }

        /// <summary>
        /// Internal CryptoSuite.
        /// </summary>
        private static byte[] _InternalDecryptionKeygen(byte[] EncryptedKey, byte[] IntegrityKey, byte[] Salt, byte[] IV,
                                                        string passphrase)
        {
            PBKDF2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passphrase), Salt, 2048, HashAlgorithmName.SHA256);
            byte[] temp = PBKDF2.GetBytes(256);
            byte[] key2 = temp[0..127];
            byte[] IntegrityCheckKey = temp[128..^0];

            if (ComputeHash(IntegrityCheckKey) != ComputeHash(IntegrityKey))
            {
                throw new Exception("Wrong passphrase or data is tampered!");
            }
            else
            {
                byte[] DecryptionKey = _InternalDecrypt(EncryptedKey, key2, IV);
                return DecryptionKey;
            }
        }

        /// <summary>
        /// Internal CryptoSuite.
        /// </summary>
        private static byte[] _InternalRandomKeygen(int length)
        {
            byte[] key = new byte[length];
            RNG.GetBytes(key);
            return key;
        }

        #endregion

        /// <summary>
        /// Merge multiple array into one.
        /// </summary>
        private static T[] _InternalArrayMerger<T>(params T[][] arrays)
        {
            int Length = 0;
            foreach (var item in arrays)
            {
                Length += item.Length;
            }
            T[] result = new T[Length];

            Length = 0;
            foreach (var item in arrays)
            {
                Array.Copy(item, 0, result, Length, item.Length);
                Length += item.Length;
            }

            return result;
        }

        #endregion
    }
}
