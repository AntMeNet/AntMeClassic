using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.Plugin.Simulation
{
    /// <summary>
    /// Class, to hold all statistic collections.
    /// </summary>
    internal sealed class SummaryRoot
    {
        public List<SummarySimulation> simulations = new List<SummarySimulation>();
    }
}
