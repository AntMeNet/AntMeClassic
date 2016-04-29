using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.Plugin.Simulation
{
    internal sealed class SummaryTeam
    {
        public string name = string.Empty;
        public Guid guid;
        public Dictionary<Guid, SummaryPlayer> players = new Dictionary<Guid, SummaryPlayer>();
    }
}
