using System;
using System.Collections.Generic;
using System.IO;

namespace AntMe.Online.Client
{
    /// <summary>
    /// Zugriffsmanager für alles rund um die Replays.
    /// </summary>
    public interface IReplayManager
    {
        /// <summary>
        /// Listet die eigenen privaten Replays auf.
        /// </summary>
        /// <returns>Liste der privaten Uploads</returns>
        IEnumerable<Replay> ListPrivateReplays();

        /// <summary>
        /// Erstellt ein neues privates Replay.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Replay CreatePrivateReplay(Stream stream);

        /// <summary>
        /// Löscht ein privates Replay.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeletePrivateReplay(Guid id);

        /// <summary>
        /// Liefert Replay Meta-Infos.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Replay GetReplay(Guid id);

        /// <summary>
        /// Lädt das Replay herunter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Stream DownloadReplay(Guid id);
    }
}
