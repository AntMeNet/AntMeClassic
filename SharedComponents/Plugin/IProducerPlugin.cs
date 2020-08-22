using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.Plugin
{

    /// <summary>
    /// Interface for all producing plugins in antme.
    /// </summary>
    /// <autor>Tom Wendel (tom@antme.net)</autor>
    public interface IProducerPlugin : IPlugin
    {

        /// <summary>
        /// Sends the filled state from consumers to the producer to put in the <see cref="SimulationState"/>. Only called by GameLoop-Thread.
        /// </summary>
        /// <param name="state">filled state</param>
        void CreateState(ref SimulationState state);
    }
}