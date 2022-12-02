using AntMe.SharedComponents.Plugin;
using System;

namespace AntMe.Gui
{
    /// <summary>
    /// class, that represents a plugin
    /// </summary>
    internal sealed class PluginItem
    {
        private readonly IConsumerPlugin consumer;
        private readonly IProducerPlugin producer;
        private readonly string name;
        private readonly string description;
        private readonly Version version;
        private readonly Guid guid;
        private readonly CustomStateItem[] writeCustomStates;
        private readonly CustomStateItem[] readCustomStates;


        /// <summary>
        /// Creates an instance of plugin item, based on a producer.
        /// </summary>
        /// <param name="plugin">Producer plugin</param>
        /// <param name="writeCustomStates">List of custom states for write access</param>
        /// <param name="readCustomStates">List of custom states for read access</param>
        public PluginItem(IProducerPlugin plugin, CustomStateItem[] writeCustomStates, CustomStateItem[] readCustomStates)
            : this(writeCustomStates, readCustomStates, plugin)
        {

            // Check for null
            if (plugin == null)
            {
                throw new ArgumentNullException("plugin", Resource.PluginItemConstructorPluginIsNull);
            }

            producer = plugin;
        }

        /// <summary>
        /// Creates an instance of plugin item, based on a producer.
        /// </summary>
        /// <param name="plugin">Consumer plugin</param>
        /// <param name="writeCustomStates">List of custom states for write access</param>
        /// <param name="readCustomStates">List of custom states for read access</param>
        public PluginItem(IConsumerPlugin plugin, CustomStateItem[] writeCustomStates, CustomStateItem[] readCustomStates)
            : this(writeCustomStates, readCustomStates, plugin)
        {
            // Check if plugin has been specified
            if (plugin == null)
            {
                throw new ArgumentNullException("plugin", Resource.PluginItemConstructorPluginIsNull);
            }

            consumer = plugin;
        }

        /// <summary>
        /// Private constructor for a common way to handle attributes.
        /// </summary>
        /// <param name="plugin">Plugin</param>
        /// <param name="writeCustomStates">List of custom states for write access</param>
        /// <param name="readCustomStates">List of custom states for read access</param>
        private PluginItem(CustomStateItem[] writeCustomStates, CustomStateItem[] readCustomStates, IPlugin plugin)
        {

            // Check for null
            if (plugin == null)
            {
                throw new ArgumentNullException("plugin", Resource.PluginItemConstructorPluginIsNull);
            }

            // Check for valid name
            if (plugin.Name == string.Empty)
            {
                throw new ArgumentException(Resource.PluginItemConstructorPluginHasNoName, "plugin");
            }

            name = plugin.Name;
            description = plugin.Description;
            guid = plugin.Guid;
            version = plugin.Version;

            // Custom states
            this.writeCustomStates = writeCustomStates;
            if (this.writeCustomStates == null)
            {
                this.writeCustomStates = new CustomStateItem[0];
            }
            this.readCustomStates = readCustomStates;
            if (this.readCustomStates == null)
            {
                this.readCustomStates = new CustomStateItem[0];
            }
        }

        /// <summary>
        /// Gets the consumer plugin or null, if its a producer plugin.
        /// </summary>
        public IConsumerPlugin Consumer
        {
            get { return consumer; }
        }

        /// <summary>
        /// Gets the producer plugin or null, if its a consumer plugin.
        /// </summary>
        public IProducerPlugin Producer
        {
            get { return producer; }
        }

        /// <summary>
        /// True, if its a consumer plugin, false in case of a producer plugin.
        /// </summary>
        public bool IsConsumer
        {
            get { return consumer != null; }
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets a short description of this plugin.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public Version Version
        {
            get { return version; }
        }

        /// <summary>
        /// Gets the plugin <see cref="guid"/>.
        /// </summary>
        public Guid Guid
        {
            get { return guid; }
        }

        /// <summary>
        /// Compares two instances of <see cref="PluginItem"/>.
        /// </summary>
        /// <param name="obj">other instance of <see cref="PluginItem"/></param>
        /// <returns>true, if equal</returns>
        public override bool Equals(object obj)
        {

            // Check for right datatype
            if (!(obj is PluginItem))
            {
                return false;
            }

            PluginItem other = (PluginItem)obj;

            // compare guid
            if (other.guid != guid)
            {
                return false;
            }

            // compare version
            if (other.version != version)
            {
                return false;
            }

            // seams to be equal
            return true;
        }

        /// <summary>
        /// Generates a hash for this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }

        /// <summary>
        /// Gives the name of this plugin.
        /// </summary>
        /// <returns>Name of plugin</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}