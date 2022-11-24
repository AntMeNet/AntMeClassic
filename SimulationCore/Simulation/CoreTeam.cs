using AntMe.SharedComponents.States;
using System;

namespace AntMe.Simulation
{
    internal sealed class CoreTeam
    {
        internal readonly int Id;
        internal readonly Guid Guid;
        internal readonly string Name;
        internal CoreColony[] Colonies;

        public CoreTeam(int id, Guid guid, string name)
        {
            Id = id;
            Guid = guid;
            Name = name;
        }

        public TeamState CreateState()
        {
            TeamState state = new TeamState(Id, Guid, Name);

            for (int i = 0; i < Colonies.Length; i++)
            {
                Colonies[i].Statistic.CurrentAntCount = Colonies[i].InsectsList.Count;
                state.ColonyStates.Add(Colonies[i].GenerateColonyStateInfo());
            }

            return state;
        }
    }
}