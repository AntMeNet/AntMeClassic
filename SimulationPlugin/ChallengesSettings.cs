using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Plugin.Simulation
{
    [Serializable]
    public sealed class ChallengesSettings
    {
        public List<ChallengeValues> ChallengeValues { get; set; }
    }

    [Serializable]
    public sealed class ChallengeValues
    {

    }
}
