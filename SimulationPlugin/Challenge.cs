using System;

namespace AntMe.Plugin.Simulation
{
    internal sealed class Challenge
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte Rating { get; set; }
    }
}
