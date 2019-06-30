using System;
using System.Collections.Generic;
using System.Text;

namespace MikeDev.Db
{
    [Serializable]
    public class DbTableException : Exception
    {
        public DbTableException() { }
        public DbTableException(string message) : base(message) { }
        public DbTableException(string message, Exception inner) : base(message, inner) { }
        protected DbTableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
