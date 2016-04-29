using System;
using System.Collections.Generic;

namespace AntMe.Plugin.Simulation {
    internal sealed class SummaryPlayer {
        public string name = string.Empty;
        public Guid guid;
        public List<SummaryValueSet> values = new List<SummaryValueSet>();
    }
}