using System;
using System.Collections.Generic;
using System.Text;

namespace MikeDev.Cryptography
{
    /// <summary>
    /// Class implementing IHashable can be used in MikeDev.Cryptography suite
    /// </summary>
    public interface ICrypto
    {
        /// <summary>
        /// Return (cryptographically secure) UNIQUE information of the object.
        /// </summary>
        public string GetUniqueInfo();
    }
}