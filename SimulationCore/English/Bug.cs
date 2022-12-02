using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Represents a bug.
    /// </summary>
    public sealed class Bug : Insect
    {
        internal Bug(CoreBug bug) : base(bug) { }
    }
}