using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AntMe.Online.Client
{
    internal sealed class ReplayManager : IReplayManager
    {
        private Connection connection;

        internal ReplayManager(Connection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Listet die eigenen privaten Replays auf.
        /// </summary>
        /// <returns>Liste der privaten Uploads</returns>
        public IEnumerable<Replay> ListPrivateReplays()
        {
            if (!connection.IsLoggedIn)
                throw new NotSupportedException();

            return connection.Get<IEnumerable<Replay>>("Replay");
        }

        /// <summary>
        /// Erstellt ein neues privates Replay.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Replay CreatePrivateReplay(Stream stream)
        {
            if (!connection.IsLoggedIn)
                throw new NotSupportedException();

            return connection.Upload<Replay>("Replay/Create", stream);
        }

        /// <summary>
        /// Löscht ein privates Replay.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeletePrivateReplay(Guid id)
        {
            if (!connection.IsLoggedIn)
                throw new NotSupportedException();

            return connection.Get<bool>(string.Format("Replay/{0}/Delete", id.ToString()));
        }

        /// <summary>
        /// Liefert Replay Meta-Infos.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Replay GetReplay(Guid id)
        {
            if (!connection.IsLoggedIn)
                throw new NotSupportedException();

            return connection.Get<Replay>(string.Format("Replay/{0}", id.ToString()));
        }

        /// <summary>
        /// Lädt das Replay herunter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Stream DownloadReplay(Guid id)
        {
            if (!connection.IsLoggedIn)
                throw new NotSupportedException();

            string path = connection.Get<string>(string.Format("Replay/{0}/Download", id.ToString()));

            if (!string.IsNullOrEmpty(path))
            {
                WebClient client = new WebClient();
                byte[] file = client.DownloadData(path);
                return new MemoryStream(file);
            }
            return null;
        }
    }
}
