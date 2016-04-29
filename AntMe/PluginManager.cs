using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml.Serialization;

using AntMe.Gui.Properties;
using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;

namespace AntMe.Gui
{
    /// <summary>
    /// Manager for all plugins and host for central game-loop
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    internal sealed class PluginManager
    {
        #region Constants

        private const int FRAMERATE_SPAN = 10;

        #endregion

        #region Variables

        // Pluginlists
        private readonly Dictionary<Guid, PluginItem> activeConsumers;
        private readonly Dictionary<Guid, PluginItem> consumerList;
        private readonly Dictionary<Guid, PluginItem> producerList;
        private PluginItem activeProducer;

        private Configuration config;
        private PluginItem visiblePlugin;
        private readonly List<Exception> exceptions;
        private PluginState lastState = PluginState.NotReady;
        private bool ignoreStateupdate = false;

        private Thread requestThread;

        private bool speedLimiter;
        private float frameLimit;
        private float frameLimitMs;
        private SimulationState lastSimulationState;

        private readonly float[] frameRateHistory;
        private bool frameRateInvalidate;
        private float frameRateAverage;
        private int frameRatePosition;

        private readonly SynchronizationContext context = SynchronizationContext.Current;

        private readonly string configPath = Environment.GetFolderPath(
                                                 Environment.SpecialFolder.ApplicationData) +
                                             Resources.ConfiguationPath;

        private readonly DirectoryInfo applicationPath;
        private readonly DirectoryInfo pluginPath;

        #endregion

        #region Constructor

        public PluginManager()
        {
            producerList = new Dictionary<Guid, PluginItem>();
            consumerList = new Dictionary<Guid, PluginItem>();
            activeConsumers = new Dictionary<Guid, PluginItem>();
            exceptions = new List<Exception>();
            config = new Configuration();

            frameRateHistory = new float[FRAMERATE_SPAN];

            // Default Speed
            SetSpeedLimit(true, 15.0f);

            applicationPath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
            pluginPath = applicationPath.GetDirectories(Resources.PluginSearchFolder).FirstOrDefault();
        }

        #endregion

        #region Plugin Discovery

        /// <summary>
        /// Search in default folders for a new plugin.
        /// </summary>
        public void SearchForPlugins()
        {
            // List dlls from Exe-Path
            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(applicationPath.GetFiles(Resources.PluginSearchFilter));

            // List dlls from "plugin"-Folder
            if (pluginPath != null) files.AddRange(pluginPath.GetFiles(Resources.PluginSearchFilter));

            // Load root Plugins
            for (int i = 0; i < files.Count; i++)
                CheckForPlugin(files[i]);

            // Check known Plugins
            foreach (var knownFile in config.knownPluginFiles.ToArray())
            {
                FileInfo external = new FileInfo(knownFile);

                // Skip some files
                if (!external.Exists ||
                    files.Contains(external) ||
                    external.Directory.FullName.ToLower().Equals(applicationPath.FullName.ToLower()) ||
                    (pluginPath != null && external.Directory.FullName.ToLower().Equals(pluginPath.FullName.ToLower())))
                {
                    // Drop from list
                    config.knownPluginFiles.Remove(knownFile);
                    continue;
                }

                // Try to load
                if (!CheckForPlugin(external))
                {
                    config.knownPluginFiles.Remove(knownFile);
                    continue;
                }
            }
        }

        /// <summary>
        /// Checks a single file for a new plugin.
        /// </summary>
        /// <param name="file">filename</param>
        /// <returns>true, if there are valid plugins inside</returns>
        public bool CheckForPlugin(FileInfo file)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                if (addPlugin(assembly))
                {
                    if (
                        !file.Directory.FullName.ToLower().Equals(applicationPath.FullName.ToLower()) && 
                        !file.Directory.FullName.ToLower().Equals(pluginPath.FullName.ToLower()) &&
                        !config.knownPluginFiles.Contains(file.FullName.ToLower()))
                    {
                        config.knownPluginFiles.Add(file.FullName.ToLower());
                    }
                        
                    return true;
                }
            }
            catch (FileLoadException ex)
            {
                // Problems to open the file
                exceptions.Add(ex);
            }
            catch (BadImageFormatException ex)
            {
                // There is a wrong fileformat
                exceptions.Add(ex);
            }
            catch (SecurityException ex)
            {
                // no accessrights
                exceptions.Add(ex);
            }
            catch (MissingMethodException ex)
            {
                // problems with plugin
                exceptions.Add(ex);
            }
            catch (TargetInvocationException ex)
            {
                // missing references
                exceptions.Add(
                    new Exception(
                        string.Format(Resource.PluginManagerMissingReferences, file),
                        ex));
            }
            catch (ReflectionTypeLoadException ex)
            {
                // missing references
                exceptions.Add(
                    new Exception(
                        string.Format(Resource.PluginManagerMissingReferences, file),
                        ex));
            }
            catch (Exception ex)
            {
                // unknown exception
                exceptions.Add(ex);
            }
            return false;
        }

        /// <summary>
        /// search in given assembly for a new plugin
        /// </summary>
        /// <param name="assembly">assembly to search in</param>
        /// <returns>true, if there are valid plugins inside</returns>
        private bool addPlugin(Assembly assembly)
        {
            bool hit = false;

            // Get all includes Types
            foreach (Type type in assembly.GetExportedTypes())
            {
                // Find the attribute
                List<CustomStateItem> readCustomStates = new List<CustomStateItem>();
                List<CustomStateItem> writeCustomStates = new List<CustomStateItem>();

                foreach (CustomAttributeData customAttribute in CustomAttributeData.GetCustomAttributes(type))
                {
                    string name;
                    string dataType;
                    string description;
                    switch (customAttribute.Constructor.ReflectedType.FullName)
                    {
                        case "AntMe.SharedComponents.Plugin.ReadCustomStateAttribute":
                            name = string.Empty;
                            dataType = string.Empty;
                            description = string.Empty;
                            foreach (CustomAttributeNamedArgument argument in customAttribute.NamedArguments)
                            {
                                switch (argument.MemberInfo.Name)
                                {
                                    case "Name":
                                        name = (string)argument.TypedValue.Value;
                                        break;
                                    case "Type":
                                        dataType = (string)argument.TypedValue.Value;
                                        break;
                                    case "Description":
                                        description = (string)argument.TypedValue.Value;
                                        break;
                                }
                            }
                            readCustomStates.Add(new CustomStateItem(name, dataType, description));
                            break;
                        case "AntMe.SharedComponents.Plugin.WriteCustomStateAttribute":
                            name = string.Empty;
                            dataType = string.Empty;
                            description = string.Empty;
                            foreach (CustomAttributeNamedArgument argument in customAttribute.NamedArguments)
                            {
                                switch (argument.MemberInfo.Name)
                                {
                                    case "Name":
                                        name = (string)argument.TypedValue.Value;
                                        break;
                                    case "Type":
                                        dataType = (string)argument.TypedValue.Value;
                                        break;
                                    case "Description":
                                        description = (string)argument.TypedValue.Value;
                                        break;
                                }
                            }
                            writeCustomStates.Add(new CustomStateItem(name, dataType, description));
                            break;
                    }
                }

                // If type has an attribute, search for the interfaces
                foreach (Type plugin in type.GetInterfaces())
                {
                    // Producer found
                    if (plugin == typeof(IProducerPlugin))
                    {
                        // Create an instance of plugin and add to list
                        PluginItem item = null;
                        try
                        {
                            IProducerPlugin producerPlugin =
                                (IProducerPlugin)Activator.CreateInstance(type, false);
                            item =
                                new PluginItem(producerPlugin, writeCustomStates.ToArray(), readCustomStates.ToArray());
                            hit = true;
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(
                                new Exception(
                                    string.Format(
                                        Resource.PluginManagerProducerPluginCommonProblems,
                                        type.FullName,
                                        assembly.GetFiles()[0].Name),
                                    ex));
                        }

                        // Warnings, of there is another Version of that plugin
                        if (item != null && producerList.ContainsKey(item.Guid))
                        {
                            if (producerList[item.Guid].Version > item.Version)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerProducerPluginNewerVersionLoaded,
                                            item.Name,
                                            item.Version)));
                                item = null;
                            }
                            else if (producerList[item.Guid].Version < item.Version)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerProducerPluginNewerVersion,
                                            item.Name,
                                            item.Version)));
                                DeactivateProducer(item.Guid);
                                producerList.Remove(item.Guid);
                            }
                            else
                            {
                                // Samle plugin still loaded
                                item = null;
                            }
                        }

                        // add to list
                        if (item != null)
                        {
                            // Check, if plugin is preselected or saved as selected
                            producerList.Add(item.Guid, item);
                            if (config.selectedPlugins.Contains(item.Guid) ||
                                (!config.loaded &&
                                 type.GetCustomAttributes(typeof(PreselectedAttribute), false).Length > 0))
                            {
                                ActivateProducer(item.Guid);
                            }

                            // Load Settings
                            if (File.Exists(configPath + item.Guid + Resources.PluginSettingsFileExtension))
                            {
                                try
                                {
                                    item.Producer.Settings =
                                        File.ReadAllBytes(
                                            configPath + item.Guid + Resources.PluginSettingsFileExtension);
                                }
                                catch (Exception ex)
                                {
                                    exceptions.Add(
                                        new Exception(
                                            string.Format(
                                                Resource.PluginManagerProducerPluginSettingsLoadFailed,
                                                item.Name,
                                                item.Version),
                                            ex));
                                }
                            }
                        }
                    }

                        // Consumer found
                    else if (plugin == typeof(IConsumerPlugin))
                    {
                        // Create an instance of plugin and add to list
                        PluginItem item = null;
                        try
                        {
                            IConsumerPlugin consumerPlugin =
                                (IConsumerPlugin)Activator.CreateInstance(type, false);
                            item =
                                new PluginItem(consumerPlugin, writeCustomStates.ToArray(), readCustomStates.ToArray());
                            hit = true;
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(
                                new Exception(
                                    string.Format(
                                        Resource.PluginManagerConsumerPluginCommonProblems,
                                        type.FullName,
                                        assembly.GetFiles()[0].Name),
                                    ex));
                        }

                        // Warnings, of there is another Version of that plugin
                        if (item != null && consumerList.ContainsKey(item.Guid))
                        {
                            if (consumerList[item.Guid].Version > item.Version)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerConsumerPluginNewerVersionLoaded,
                                            item.Name,
                                            item.Version)));
                                item = null;
                            }
                            else if (consumerList[item.Guid].Version < item.Version)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerConsumerPluginNewerVersion,
                                            item.Name,
                                            item.Version)));
                                DeactivateConsumer(item.Guid);
                                consumerList.Remove(item.Guid);
                            }
                            else
                            {
                                // Same plugin still loaded
                                item = null;
                            }
                        }

                        // add to list
                        if (item != null)
                        {
                            consumerList.Add(item.Guid, item);

                            // Check, if plugin is preselected or saved as selected
                            if (config.selectedPlugins.Contains(item.Guid) ||
                                (!config.loaded &&
                                 type.GetCustomAttributes(typeof(PreselectedAttribute), false).Length > 0))
                            {
                                ActivateConsumer(item.Guid);
                            }

                            // Load Settings
                            if (File.Exists(configPath + item.Guid + Resources.PluginSettingsFileExtension))
                            {
                                try
                                {
                                    item.Consumer.Settings =
                                        File.ReadAllBytes(
                                            configPath + item.Guid + Resources.PluginSettingsFileExtension);
                                }
                                catch (Exception ex)
                                {
                                    exceptions.Add(
                                        new Exception(
                                            string.Format(
                                                Resource.PluginManagerConsumerPluginSettingsLoadFailed,
                                                item.Name,
                                                item.Version),
                                            ex));
                                }
                            }
                        }
                    }
                }
            }
            return hit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gives a list of available producer-plugins.
        /// </summary>
        public PluginItem[] ProducerPlugins
        {
            get
            {
                PluginItem[] plugins = new PluginItem[producerList.Count];
                producerList.Values.CopyTo(plugins, 0);
                return plugins;
            }
        }

        /// <summary>
        /// Gives a list of available consumer-plugins.
        /// </summary>
        public PluginItem[] ConsumerPlugins
        {
            get
            {
                PluginItem[] plugins = new PluginItem[consumerList.Count];
                consumerList.Values.CopyTo(plugins, 0);
                return plugins;
            }
        }

        /// <summary>
        /// Gives a list of activated consumer-plugins.
        /// </summary>
        public PluginItem[] ActiveConsumerPlugins
        {
            get
            {
                PluginItem[] plugins = new PluginItem[activeConsumers.Count];
                activeConsumers.Values.CopyTo(plugins, 0);
                return plugins;
            }
        }

        /// <summary>
        /// Returns a list of exceptions that happened during the last call
        /// </summary>
        public List<Exception> Exceptions
        {
            get { return exceptions; }
        }

        /// <summary>
        /// Gives the active producer-plugin or null, if no plugin is active.
        /// </summary>
        public PluginItem ActiveProducerPlugin
        {
            get { return activeProducer; }
        }

        /// <summary>
        /// Is manager ready to start.
        /// </summary>
        public bool CanStart
        {
            get { return (State == PluginState.Ready || State == PluginState.Paused); }
        }

        /// <summary>
        /// Is manger ready for pause-mode.
        /// </summary>
        public bool CanPause
        {
            get { return (State == PluginState.Running || State == PluginState.Ready); }
        }

        /// <summary>
        /// Is manager still running and can stop.
        /// </summary>
        public bool CanStop
        {
            get { return (State == PluginState.Running || State == PluginState.Paused); }
        }

        /// <summary>
        /// Returns the current state of this manager.
        /// </summary>
        public PluginState State
        {
            get
            {
                if (!ignoreStateupdate)
                {
                    PluginState output = PluginState.NotReady;

                    // capture producerstate
                    if (activeProducer != null)
                    {
                        output = activeProducer.Producer.State;
                    }

                    // check for changes
                    switch (output)
                    {
                        case PluginState.NotReady:
                        case PluginState.Ready:
                            if (lastState == PluginState.Running ||
                                lastState == PluginState.Paused)
                            {
                                // Producer switched from running/paused to notReady or ready
                                // All running consumer have to stop
                                foreach (PluginItem item in activeConsumers.Values)
                                {
                                    if (item.Consumer.State == PluginState.Running ||
                                        item.Consumer.State == PluginState.Paused)
                                    {
                                        item.Consumer.Stop();
                                    }
                                }
                            }
                            break;
                        case PluginState.Running:
                            if (lastState == PluginState.NotReady ||
                                lastState == PluginState.Ready ||
                                lastState == PluginState.Paused)
                            {
                                // Producer switched from somewhere to running
                                // All ready or paused consumer have to start
                                foreach (PluginItem item in activeConsumers.Values)
                                {
                                    if (item.Consumer.State == PluginState.Paused ||
                                        item.Consumer.State == PluginState.Ready)
                                    {
                                        item.Consumer.Start();
                                    }
                                }
                            }
                            break;
                        case PluginState.Paused:
                            if (lastState == PluginState.Running ||
                                lastState == PluginState.Ready)
                            {
                                // Producer switched to pause-mode
                                // All ready or running consumer have to pause
                                foreach (PluginItem item in activeConsumers.Values)
                                {
                                    if (item.Consumer.State == PluginState.Running ||
                                        item.Consumer.State == PluginState.Ready)
                                    {
                                        item.Consumer.Pause();
                                    }
                                }
                            }
                            break;
                    }

                    // Start requestLoop, if needed
                    if ((output == PluginState.Running || output == PluginState.Paused) && requestThread == null)
                    {
                        requestThread = new Thread(requestLoop);
                        requestThread.IsBackground = true;
                        requestThread.Priority = ThreadPriority.Normal;
                        requestThread.Start();
                    }

                    // return
                    lastState = output;
                    return output;
                }
                else
                {
                    return lastState;
                }
            }
        }

        /// <summary>
        /// Gives the number of current simulation-round.
        /// </summary>
        public int CurrentRound
        {
            get
            {
                if (lastSimulationState != null)
                {
                    return lastSimulationState.CurrentRound;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gives the number of total rounds for the current simulation.
        /// </summary>
        public int TotalRounds
        {
            get
            {
                if (lastSimulationState != null)
                {
                    return Math.Max(lastSimulationState.CurrentRound, lastSimulationState.TotalRounds);
                }
                return 0;
            }
        }

        /// <summary>
        /// Gives the current frame-rate for this simulation.
        /// </summary>
        public float FrameRate
        {
            get
            {
                // calculate new average
                if (frameRateInvalidate)
                {
                    frameRateAverage = 0;
                    for (int i = 0; i < FRAMERATE_SPAN; i++)
                    {
                        frameRateAverage += frameRateHistory[i];
                    }
                    frameRateAverage /= FRAMERATE_SPAN;
                    frameRateInvalidate = false;
                }

                // deliver
                return frameRateAverage;
            }
            private set
            {
                // write new value in ringbuffer
                frameRateInvalidate = true;
                frameRateHistory[frameRatePosition++ % FRAMERATE_SPAN] = value;
            }
        }

        /// <summary>
        /// Gives the current frame-rate-limit, if limiter is enabled.
        /// </summary>
        public float FrameLimit
        {
            get
            {
                if (FrameLimiterEnabled)
                {
                    return frameLimit;
                }
                return 0.0f;
            }
        }

        /// <summary>
        /// Gives the state of the frame-rate-limiter.
        /// </summary>
        public bool FrameLimiterEnabled
        {
            get { return speedLimiter; }
        }

        /// <summary>
        /// Gives the current configuration.
        /// </summary>
        public Configuration Configuration
        {
            get { return config; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// drops a consumer from active list.
        /// </summary>
        /// <param name="guid"><c>guid</c> of plugin to drop</param>
        public void DeactivateConsumer(Guid guid)
        {
            // Check, if plugin exists
            if (!consumerList.ContainsKey(guid))
            {
                throw new InvalidOperationException(Resource.PluginManagerDeactivateConsumerNotInList);
            }

            // Drop from active list
            lock (activeConsumers)
            {
                if (activeConsumers.ContainsKey(guid))
                {
                    PluginItem plugin = activeConsumers[guid];

                    activeConsumers.Remove(guid);
                    config.selectedPlugins.Remove(guid);

                    // Stop, if still running
                    if (plugin.Consumer.State == PluginState.Running ||
                        plugin.Consumer.State == PluginState.Paused)
                    {
                        plugin.Consumer.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Adds a plugin to active list
        /// </summary>
        /// <param name="guid"><c>guid</c> of new plugin</param>
        public void ActivateConsumer(Guid guid)
        {
            // Check, if plugin exists
            if (!consumerList.ContainsKey(guid))
            {
                throw new InvalidOperationException(Resource.PluginManagerActivateConsumerNotInList);
            }

            // Add to list
            lock (activeConsumers)
            {
                if (!activeConsumers.ContainsKey(guid))
                {
                    PluginItem plugin = consumerList[guid];

                    // Activate, if simulation still running
                    if (State == PluginState.Running)
                    {
                        plugin.Consumer.Start();
                    }
                    else if (State == PluginState.Paused)
                    {
                        plugin.Consumer.Pause();
                    }

                    activeConsumers.Add(guid, plugin);

                    // mark as selected in config
                    if (!config.selectedPlugins.Contains(guid))
                    {
                        config.selectedPlugins.Add(guid);
                    }
                }
            }
        }

        /// <summary>
        /// Deactivates the given producer
        /// </summary>
        /// <param name="guid"><c>guid</c> of producer</param>
        public void DeactivateProducer(Guid guid)
        {
            ignoreStateupdate = true;

            if (activeProducer != null && activeProducer.Guid == guid)
            {
                // unhook producer
                Stop();
                activeProducer = null;
                config.selectedPlugins.Remove(guid);
            }
            ignoreStateupdate = false;
        }

        /// <summary>
        /// Changes the active Producer
        /// </summary>
        /// <param name="guid"><c>guid</c> of new producer</param>
        public void ActivateProducer(Guid guid)
        {
            ignoreStateupdate = true;
            // check, if plugin with that guid exists
            if (!producerList.ContainsKey(guid))
            {
                throw new InvalidOperationException(Resource.PluginManagerActivateProducerNotInList);
            }

            // check, if same plugin is still active
            if (activeProducer == null || activeProducer.Guid != guid)
            {
                // unhook old producer 
                if (activeProducer != null)
                {
                    DeactivateProducer(activeProducer.Guid);
                }

                // hook the new one
                activeProducer = producerList[guid];

                if (!config.selectedPlugins.Contains(guid))
                {
                    config.selectedPlugins.Add(guid);
                }
            }
            ignoreStateupdate = false;
        }

        /// <summary>
        /// Starts the manager, of its ready
        /// </summary>
        public void Start()
        {
            ignoreStateupdate = true;

            // Start the producer
            lock (activeProducer)
            {
                if (activeProducer != null &&
                    (activeProducer.Producer.State == PluginState.Ready ||
                     activeProducer.Producer.State == PluginState.Paused))
                {
                    activeProducer.Producer.Start();
                }
            }

            ignoreStateupdate = false;
        }

        /// <summary>
        /// pause the manager
        /// </summary>
        public void Pause()
        {
            ignoreStateupdate = true;

            // Suspend the producer
            lock (activeProducer)
            {
                if (activeProducer != null &&
                    (activeProducer.Producer.State == PluginState.Running ||
                     activeProducer.Producer.State == PluginState.Ready))
                {
                    activeProducer.Producer.Pause();
                }
            }

            ignoreStateupdate = false;
        }

        /// <summary>
        ///  Stops the manager
        /// </summary>
        public void Stop()
        {
            ignoreStateupdate = true;

            lock (activeProducer)
            {
                if (activeProducer != null &&
                    (activeProducer.Producer.State == PluginState.Running ||
                     activeProducer.Producer.State == PluginState.Paused))
                {
                    activeProducer.Producer.Stop();
                }
            }

            ignoreStateupdate = false;
        }

        /// <summary>
        /// Loads the settings to configuration
        /// </summary>
        public void LoadSettings()
        {
            ignoreStateupdate = true;
            // check, if configfile exists
            if (File.Exists(configPath + Resources.GlobalSettingsFileName))
            {
                // read configfile
                FileStream file = File.Open(configPath + Resources.GlobalSettingsFileName, FileMode.Open);

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    config = (Configuration)serializer.Deserialize(file);
                    config.loaded = true;
                    SetSpeedLimit(config.speedLimitEnabled, config.speedLimit);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        Resource.PluginManagerSettingsLoadFailed,
                        ex);
                }
                finally
                {
                    file.Close();
                }
            }
            ignoreStateupdate = false;
        }

        /// <summary>
        /// Saves the settings of plugin-manager to configuration-file.
        /// </summary>
        public void SaveSettings()
        {
            ignoreStateupdate = true;
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            MemoryStream puffer = new MemoryStream();
            serializer.Serialize(puffer, config);
            File.WriteAllBytes(configPath + Resources.GlobalSettingsFileName, puffer.ToArray());
            ignoreStateupdate = false;

            // Save also plugin-Settings
            // Producer-Stuff
            foreach (PluginItem plugin in producerList.Values)
            {
                try
                {
                    byte[] temp = plugin.Producer.Settings;
                    if (temp != null && temp.Length > 0)
                        File.WriteAllBytes(configPath + plugin.Guid + Resources.PluginSettingsFileExtension,
                            temp);
                }
                catch (Exception ex)
                {
                    exceptions.Add(
                        new Exception(
                            string.Format(
                                Resource.PluginManagerProducerPluginSettingsSaveFailed, plugin.Name, plugin.Version),
                            ex));
                }
            }

            // Consumer-Stuff
            foreach (PluginItem plugin in consumerList.Values)
            {
                try
                {
                    byte[] temp = plugin.Consumer.Settings;
                    if (temp != null && temp.Length > 0)
                        File.WriteAllBytes(
                            configPath + plugin.Guid + Resources.PluginSettingsFileExtension, temp);
                }
                catch (Exception ex)
                {
                    exceptions.Add(
                        new Exception(
                            string.Format(
                                Resource.PluginManagerConsumerPluginSettingsSaveFailed, plugin.Name, plugin.Version),
                            ex));
                }
            }
        }

        /// <summary>
        /// Sets the current visible plugin
        /// </summary>
        /// <param name="guid">visible Plugin</param>
        public void SetVisiblePlugin(Guid guid)
        {
            ignoreStateupdate = true;
            // Set old plugin to invisible
            if (visiblePlugin != null)
            {
                if (visiblePlugin.IsConsumer)
                {
                    try
                    {
                        visiblePlugin.Consumer.SetVisibility(false);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(
                            new Exception(
                                string.Format(
                                    Resource.PluginManagerProducerVisibilitySetFailed,
                                    visiblePlugin.Name,
                                    visiblePlugin.Version),
                                ex));
                    }
                }
                else
                {
                    try
                    {
                        visiblePlugin.Producer.SetVisibility(false);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(
                            new Exception(
                                string.Format(
                                    Resource.PluginManagerConsumerVisibilitySetFailed,
                                    visiblePlugin.Name,
                                    visiblePlugin.Version),
                                ex));
                    }
                }
            }

            // Set new plugin to visible
            if (producerList.ContainsKey(guid))
            {
                visiblePlugin = producerList[guid];
                try
                {
                    visiblePlugin.Producer.SetVisibility(true);
                }
                catch (Exception ex)
                {
                    exceptions.Add(
                        new Exception(
                            string.Format(
                                Resource.PluginManagerProducerVisibilitySetFailed,
                                visiblePlugin.Name,
                                visiblePlugin.Version),
                            ex));
                }
            }
            else if (consumerList.ContainsKey(guid))
            {
                visiblePlugin = consumerList[guid];
                try
                {
                    visiblePlugin.Consumer.SetVisibility(true);
                }
                catch (Exception ex)
                {
                    exceptions.Add(
                        new Exception(
                            string.Format(
                                Resource.PluginManagerConsumerVisibilitySetFailed,
                                visiblePlugin.Name,
                                visiblePlugin.Version),
                            ex));
                }
            }
            else
            {
                visiblePlugin = null;
            }
            ignoreStateupdate = false;
        }

        /// <summary>
        /// Sets the speed-limitation for running simulations.
        /// </summary>
        /// <param name="enabled">sets the limitation to enabled</param>
        /// <param name="framesPerSecond">limits the speed to a specific frame-rate</param>
        public void SetSpeedLimit(bool enabled, float framesPerSecond)
        {
            if (enabled)
            {
                // Check for supported value
                if (framesPerSecond <= 0.0f)
                {
                    throw new ArgumentOutOfRangeException(
                        "framesPerSecond", framesPerSecond, Resource.PluginManagerFrameRateTooLow);
                }

                frameLimit = framesPerSecond;
                frameLimitMs = 1000 / framesPerSecond;
                speedLimiter = true;

                config.speedLimit = frameLimit;
                config.speedLimitEnabled = true;
            }
            else
            {
                frameLimit = 0.0f;
                frameLimitMs = 0.0f;
                speedLimiter = false;

                config.speedLimit = 0.0f;
                config.speedLimitEnabled = false;
            }
        }

        #endregion

        #region Requestloop

        /// <summary>
        /// The game-loop. Runs, until state is set to Ready or notReady
        /// </summary>
        private void requestLoop()
        {
            // Limiter- and frame-rate-handling
            Stopwatch watch = new Stopwatch();

            // Interrupt-Handling
            bool interrupt = false;

            // Mainloop
            while (activeProducer != null &&
                   (activeProducer.Producer.State == PluginState.Running ||
                    activeProducer.Producer.State == PluginState.Paused))
            {

                // request Simulationstate, if loop is not paused
                if (activeProducer != null && activeProducer.Producer.State == PluginState.Running)
                {
                    watch.Reset();
                    watch.Start();

                    // Create new Simulationstate
                    SimulationState simulationState = new SimulationState();

                    // Request all consumers with CreateState-Method
                    lock (activeConsumers)
                    {
                        foreach (PluginItem item in activeConsumers.Values)
                        {
                            try
                            {
                                lock (item)
                                {
                                    if (item.Consumer.State == PluginState.Running)
                                    {
                                        item.Consumer.CreateState(ref simulationState);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerLoopConsumer1Failed, item.Name, item.Version),
                                        ex));
                                interrupt = true;
                                break;
                            }
                        }
                    }

                    // Break, if there was an interrupt
                    if (interrupt)
                    {
                        break;
                    }

                    // Request producers Simulationstate
                    lock (activeProducer)
                    {
                        try
                        {
                            if (activeProducer != null && activeProducer.Producer.State == PluginState.Running)
                            {
                                activeProducer.Producer.CreateState(ref simulationState);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(
                                new Exception(
                                    string.Format(
                                        Resource.PluginManagerLoopProducerFailed,
                                        activeProducer.Name,
                                        activeProducer.Version),
                                    ex));
                            interrupt = true;
                            break;
                        }
                    }

                    // Request all consumers with CreatingState-Method
                    lock (activeConsumers)
                    {
                        foreach (PluginItem item in activeConsumers.Values)
                        {
                            try
                            {
                                lock (item)
                                {
                                    if (item.Consumer.State == PluginState.Running)
                                    {
                                        item.Consumer.CreatingState(ref simulationState);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerLoopConsumer2Failed, item.Name, item.Version),
                                        ex));
                                interrupt = true;
                                break;
                            }
                        }
                    }

                    // On interrupt stop loop
                    if (interrupt)
                    {
                        break;
                    }

                    // Request all consumers with CreatedState-Method and also check for interrupt-Request
                    lock (activeConsumers)
                    {
                        foreach (PluginItem item in activeConsumers.Values)
                        {
                            try
                            {
                                lock (item)
                                {
                                    if (item.Consumer.State == PluginState.Running)
                                    {
                                        item.Consumer.CreatedState(ref simulationState);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(
                                    new Exception(
                                        string.Format(
                                            Resource.PluginManagerLoopConsumer3Failed, item.Name, item.Version),
                                        ex));
                                interrupt = true;
                                break;
                            }
                        }
                    }

                    // On interrupt stop loop
                    if (interrupt)
                    {
                        break;
                    }

                    // Update UI
                    try
                    {
                        if (activeProducer != null && activeProducer.Producer.State == PluginState.Running)
                        {
                            context.Send(delegate { activeProducer.Producer.UpdateUI(simulationState); }, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(
                            new Exception(
                                string.Format(
                                    Resource.PluginManagerLoopProducerUIFailed,
                                    activeProducer.Name,
                                    activeProducer.Version),
                                ex));
                        interrupt = true;
                        break;
                    }

                    foreach (PluginItem item in activeConsumers.Values)
                    {
                        try
                        {
                            if (item.Consumer.State == PluginState.Running)
                            {
                                context.Send(delegate { item.Consumer.UpdateUI(simulationState); }, null);
                                interrupt |= item.Consumer.Interrupt;
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(
                                new Exception(
                                    string.Format(
                                        Resource.PluginManagerLoopConsumerUIFailed, item.Name, item.Version),
                                    ex));
                            interrupt = true;
                            break;
                        }
                    }

                    // On interrupt stop loop
                    if (interrupt)
                    {
                        break;
                    }

                    // Save state for statistics
                    lastSimulationState = simulationState;

                    // Framelimiter
                    if (FrameLimiterEnabled)
                    {
                        while (watch.ElapsedMilliseconds < frameLimitMs)
                        {
                            Thread.Sleep(1);
                        }
                    }

                    // calculation of frame-time
                    FrameRate = watch.ElapsedMilliseconds > 0 ? 1000 / watch.ElapsedMilliseconds : 0;
                }
                else
                {
                    Thread.Sleep(20);
                }
            }

            // Interrupt
            if (interrupt)
            {
                try
                {
                    context.Send(delegate { activeProducer.Producer.Stop(); }, null);
                }
                catch (Exception ex)
                {
                    exceptions.Add(
                        new Exception(
                            string.Format(
                                Resource.PluginManagerLoopInterruptFailed,
                                activeProducer.Name,
                                activeProducer.Version),
                            ex));
                }
            }

            requestThread = null;

            watch.Reset();
        }

        #endregion
    }
}