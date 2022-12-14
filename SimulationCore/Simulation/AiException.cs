using AntMe.SharedComponents;
using System;
using System.Runtime.Serialization;

namespace AntMe.Simulation
{
    /// <summary>
    /// Exceptions for implementation problems in player AI files.
    /// </summary>
    [Serializable]
    public sealed class AiException : AntMeException
    {
        /// <summary>
        /// Constructor of AI exception.
        /// </summary>
        public AiException()
        {
        }

        /// <summary>
        /// Constructor of AI exception corresponding to a rule violation with handover of the description,
        /// </summary>
        /// <param name="message">description of the rule violation</param>
        public AiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor of AI exception corresponding to a rule violation with handover of the description,
        /// and the corresponding exception
        /// </summary>
        /// <param name="message">description of the rule violation</param>
        /// <param name="innerException">exception</param>
        public AiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor for serialization of the AI exception,
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public AiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}