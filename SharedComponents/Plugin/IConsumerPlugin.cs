using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.Plugin {
    /// <summary>
    /// Interface for all consuming plugins.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    public interface IConsumerPlugin : IPlugin
    {
        /// <summary>
        /// Allows a plugin to signal an interrupt. Only called by GameLoop-Thread.
        /// </summary>
        bool Interrupt { get; }

        /// <summary>
        /// Sends the empty state to push some custom fields to control the simulation. Only called by GameLoop-Thread.
        /// </summary>
        /// <param name="state">empty state</param>
        void CreateState(ref SimulationState state);

        /// <summary>
        /// Sends the filled state to push some custom fields to control the other consumers. Only called by GameLoop-Thread.
        /// </summary>
        /// <param name="state">filled state</param>
        void CreatingState(ref SimulationState state);

        /// <summary>
        /// Sends the complete filled state to consume. Only called by GameLoop-Thread.
        /// </summary>
        /// <param name="state">complete state</param>
        void CreatedState(ref SimulationState state);
    }
}