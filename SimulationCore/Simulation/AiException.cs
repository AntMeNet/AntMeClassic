using System;
using System.Runtime.Serialization;

using AntMe.SharedComponents;
using System.Security.Permissions;
using System.Security;

namespace AntMe.Simulation
{
    /// <summary>
    /// Exception for implementation-Problems in player-ai-files.
    /// </summary>
    [Serializable]
    public sealed class AiException : AntMeException
    {
        /// <summary>
        /// KOnstruktor der Regelverletzung ohne weitere Angaben
        /// </summary>
        /// <param name="player">Player ID</param>
        public AiException()
        {
        }

        /// <summary>
        /// Konsruktor der Regelverletzung mit der Übergabe einer Beschreibung zur Verletzung
        /// </summary>
        /// <param name="player">Player ID</param>
        /// <param name="message">Beschreibung der Regelverletzung</param>
        public AiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Konstruktor zur Regelverletung mit übergabe einer Nachricht sowie einer verursachenden Exception
        /// </summary>
        /// <param name="player">Player ID</param>
        /// <param name="message">Beschreibung zum Problem</param>
        /// <param name="innerException">Verursachende Exception</param>
        public AiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Konstruktor für die Serialisierung dieser Exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public AiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}