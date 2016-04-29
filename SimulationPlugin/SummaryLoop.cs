using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.Plugin.Simulation
{
    /// <summary>
    /// Class, to hold a summary of a single simulation-loop.
    /// </summary>
    internal sealed class SummaryLoop
    {
        public bool completed;
        public int rounds;
        public Dictionary<Guid, SummaryTeam> teams = new Dictionary<Guid, SummaryTeam>();
    }
}
