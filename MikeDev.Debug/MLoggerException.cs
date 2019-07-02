using System;
using System.Collections.Generic;
using System.Text;

namespace MikeDev.Debug
{
    [Serializable]
    public class MLoggerException : Exception
    {
        public MLoggerException() { }
        public MLoggerException(string message) : base(message) { }
        public MLoggerException(string message, Exception inner) : base(message, inner) { }
        protected MLoggerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
