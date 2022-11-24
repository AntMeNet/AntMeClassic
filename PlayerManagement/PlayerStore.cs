using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace AntMe.PlayerManagement
{
    /// <summary>
    /// central player store
    /// </summary>
    public sealed class PlayerStore
    {
        #region Singleton

        private static PlayerStore instance;

        public static PlayerStore Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerStore();
                instance.Init();
                return instance;
            }
        }

        #endregion

        private bool init = false;
        private readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AntMe\Player17.conf";
        private PlayerStoreConfiguration configuration = new PlayerStoreConfiguration();
        private List<string> scannedFiles = new List<string>();
        private List<PlayerInfoFilename> knownPlayer = new List<PlayerInfoFilename>();

        private PlayerStore()
        {
        }

        private void Init()
        {
            lock (this)
            {
                if (init) return;

                // Load Config
                LoadConfiguration();

                // Load local files
                DirectoryInfo root = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
                foreach (var file in root.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        ScanFile(file.FullName);
                    }
                    catch (Exception) { }
                }

                // Load Files from Config
                foreach (var file in configuration.KnownFiles.ToArray())
                {
                    try
                    {
                        ScanFile(file);
                    }
                    catch (Exception)
                    {
                        UnregisterFile(file);
                    }
                }

                init = true;
            }
        }

        /// <summary>
        /// Lists all known Player.
        /// </summary>
        public ReadOnlyCollection<PlayerInfoFilename> KnownPlayer { get { return knownPlayer.AsReadOnly(); } }

        /// <summary>
        /// Registeres a new File to the Storage.
        /// </summary>
        /// <param name="file"></param>
        public void RegisterFile(string file)
        {
            lock (this)
            {
                if (scannedFiles.Contains(file.ToLower()))
                    return;

                // unregister old file
                if (configuration.KnownFiles.Contains(file.ToLower()))
                    UnregisterFile(file);

                var result = AiAnalysis.Analyse(file, true);

                if (result.Count > 0)
                {
                    configuration.KnownFiles.Add(file.ToLower());
                    foreach (var player in result)
                    {
                        var playerInfo = new PlayerInfoFilename(player, file.ToLower());
                        knownPlayer.Add(playerInfo);
                    }
                }

                SaveConfiguration();
            }
        }

        /// <summary>
        /// Unregister a known file from the storage.
        /// </summary>
        /// <param name="file"></param>
        public void UnregisterFile(string file)
        {
            lock (this)
            {
                if (configuration.KnownFiles.Contains(file.ToLower()))
                {
                    scannedFiles.Remove(file.ToLower());
                    configuration.KnownFiles.Remove(file.ToLower());
                    knownPlayer.RemoveAll((p) => string.Compare(((PlayerInfoFilename)p).File, file.ToLower(), true) == 0);
                    SaveConfiguration();
                }
            }
        }

        /// <summary>
        /// Adds all included players without adding to the registered files.
        /// </summary>
        /// <param name="file"></param>
        private void ScanFile(string file)
        {
            try
            {
                var result = AiAnalysis.Analyse(file.ToLower(), false);
                foreach (var player in result)
                    knownPlayer.Add(new PlayerInfoFilename(player, file));

                scannedFiles.Add(file.ToLower());
            }
            catch (Exception) { throw; }
        }

        private void SaveConfiguration()
        {
            try
            {

                using (Stream stream = File.Open(path, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PlayerStoreConfiguration));
                    serializer.Serialize(stream, configuration);
                }
            }
            catch (Exception) { }
        }

        private void LoadConfiguration()
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PlayerStoreConfiguration));
                    var config = (PlayerStoreConfiguration)serializer.Deserialize(stream);
                    if (config != null)
                        configuration = config;
                }
            }
            catch (Exception) { }

        }
    }
}
