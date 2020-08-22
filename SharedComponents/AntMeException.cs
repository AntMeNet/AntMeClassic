using System;
using System.Runtime.Serialization;

namespace AntMe.SharedComponents
{
    /// <summary>
    /// Exception for special AntMe-Exceptions.
    /// </summary>
    [Serializable]
    public class AntMeException : Exception
    {
        public AntMeException() { }
        public AntMeException(string message) : base(message) { }
        public AntMeException(string message, Exception innerException) : base(message, innerException) { }
        public AntMeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}