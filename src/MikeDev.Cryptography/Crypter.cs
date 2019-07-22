﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace MikeDev.Cryptography
{
    /// <summary>
    /// Crypter exposes many APIs related to cryptography.
    /// </summary>
    public static class Crypter
    {
        private static readonly SHA512 sha512 = SHA512.Create();

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
        /// Compute hash with SHA512 algorithm. Internal.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private static string _InternalComputeHash(byte[] byteArray)
        {
            byteArray = sha512.ComputeHash(byteArray);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteArray.Length; i++)
            {
                builder.Append(byteArray[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
