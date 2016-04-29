using System;
using System.Runtime.Serialization;

using AntMe.SharedComponents;

namespace AntMe.Simulation {
    /// <summary>
    /// Exception for implementation-Problems in player-ai-files.
    /// </summary>
    [Serializable]
    internal class AiException : AntMeException {
        /// <summary>
        /// KOnstruktor der Regelverletzung ohne weitere Angaben
        /// </summary>
        public AiException() {}

        /// <summary>
        /// Konsruktor der Regelverletzung mit der Übergabe einer Beschreibung zur Verletzung
        /// </summary>
        /// <param name="message">Beschreibung der Regelverletzung</param>
        public AiException(string message) : base(message) {}

        /// <summary>
        /// Konstruktor zur Regelverletung mit übergabe einer Nachricht sowie einer verursachenden Exception
        /// </summary>
        /// <param name="message">Beschreibung zum Problem</param>
        /// <param name="innerException">Verursachende Exception</param>
        public AiException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Konstruktor für die Serialisierung dieser Exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public AiException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}