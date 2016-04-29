using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.Plugin.Simulation
{
    /// <summary>
    /// Class, to hold the summary of a simulation-set.
    /// </summary>
    internal sealed class SummarySimulation
    {
        public DateTime startDate = DateTime.Now;
        public List<SummaryLoop> loops = new List<SummaryLoop>();

    }
}
