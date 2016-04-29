using System;
using System.Runtime.Serialization;
using AntMe.SharedComponents;

namespace AntMe.Simulation {
    /// <summary>
    /// Wird bei einer Regelverletzung der AntMe-Spielregeln geworfen
    /// </summary>
    [Serializable]
    public sealed class RuleViolationException : AntMeException {
        /// <summary>
        /// KOnstruktor der Regelverletzung ohne weitere Angaben
        /// </summary>
        public RuleViolationException() { }

        /// <summary>
        /// Konsruktor der Regelverletzung mit der Übergabe einer Beschreibung zur Verletzung
        /// </summary>
        /// <param name="message">Beschreibung der Regelverletzung</param>
        public RuleViolationException(string message) : base(message) { }

        /// <summary>
        /// Konstruktor zur Regelverletung mit übergabe einer Nachricht sowie einer verursachenden Exception
        /// </summary>
        /// <param name="message">Beschreibung zum Problem</param>
        /// <param name="innerException">Verursachende Exception</param>
        public RuleViolationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Konstruktor für die Serialisierung dieser Exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public RuleViolationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}