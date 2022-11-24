using AntMe.SharedComponents;
using System;
using System.Runtime.Serialization;

namespace AntMe.Simulation
{
    /// <summary>
    /// exception for implementation problems in player AI files
    /// </summary>
    [Serializable]
    public sealed class AiException : AntMeException
    {
        /// <summary>
        /// constructor of AI exception
        /// </summary>
        public AiException()
        {
        }

        /// <summary>
        /// constructor of AI exception corresponding to a rule violation with handover of the description
        /// </summary>
        /// <param name="message">description of the rule violation</param>
        public AiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// constructor of AI exception corresponding to a rule violation with handover of the description
        /// and the corresponding exception
        /// </summary>
        /// <param name="message">description of the rule violation</param>
        /// <param name="innerException">exception</param>
        public AiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// constructor for serialization of the AI exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public AiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}