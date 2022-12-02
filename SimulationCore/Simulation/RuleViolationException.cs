using AntMe.SharedComponents;
using System;
using System.Runtime.Serialization;

namespace AntMe.Simulation
{
    /// <summary>
    /// Is thrown for a violation of the AntMe game rules
    /// </summary>
    [Serializable]
    public sealed class RuleViolationException : AntMeException
    {
        /// <summary>
        /// Constructor of the rule violation without further details
        /// </summary>
        public RuleViolationException() { }

        /// <summary>
        /// Constructor of the rule violation with the transfer of a description of the violation
        /// </summary>
        /// <param name="message">description of the violation</param>
        public RuleViolationException(string message) : base(message) { }

        /// <summary>
        /// Constructor for rule invalidation with passing of a message and the inner exception
        /// </summary>
        /// <param name="message">description of the violation</param>
        /// <param name="innerException">causing exception</param>
        public RuleViolationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructor for serialization of this exception
        /// </summary>
        /// <param name="info">information</param>
        /// <param name="context">context</param>
        public RuleViolationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}