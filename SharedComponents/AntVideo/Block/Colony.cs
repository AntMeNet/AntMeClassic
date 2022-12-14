using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block
{
    internal sealed class Colony : ColonyState, IUpdateable<ColonyUpdate, ColonyState>, ISerializable
    {
        #region Base information

        public int TeamId;

        #endregion

        #region Update information

        private int aBeatenAnts;
        private int aCollectedFood;
        private int aEatenAnts;
        private int aKilledBugs;
        private int aKilledEnemies;
        private int aPoints;
        private int aStarvedAnts;

        private bool isAlive;

        #endregion

        public Colony(ColonyState state, int teamId) : base(state.Id)
        {
            Guid = state.Guid;
            TeamId = teamId;
            PlayerName = state.PlayerName;
            ColonyName = state.ColonyName;
            CollectedFood = state.CollectedFood;
            StarvedAnts = state.StarvedAnts;
            BeatenAnts = state.BeatenAnts;
            EatenAnts = state.EatenAnts;
            KilledBugs = state.KilledBugs;
            KilledEnemies = state.KilledEnemies;
            Points = state.Points;

            Reset();
        }

        public Colony(Serializer serializer)
            : base(0)
        {
            Deserialize(serializer);

            Reset();
        }

        /// <summary>
        /// Sets the update data to expected values.
        /// </summary>
        private void Reset()
        {
            aStarvedAnts = StarvedAnts;
            aBeatenAnts = BeatenAnts;
            aEatenAnts = EatenAnts;
            aKilledBugs = KilledBugs;
            aKilledEnemies = KilledEnemies;
            aCollectedFood = CollectedFood;
            aPoints = Points;
        }

        #region IUpdateable<ColonyUpdate,ColonyState> Member

        public void Interpolate()
        {
            StarvedAnts = aStarvedAnts;
            BeatenAnts = aBeatenAnts;
            EatenAnts = aEatenAnts;
            KilledBugs = aKilledBugs;
            KilledEnemies = aKilledEnemies;
            CollectedFood = aCollectedFood;
            Points = aPoints;
        }

        public void Update(ColonyUpdate update)
        {
            if (update.HasChanged(ColonyFields.StarvedAnts))
            {
                aStarvedAnts = update.aStarvedAnts;
            }
            if (update.HasChanged(ColonyFields.EatenAnts))
            {
                aEatenAnts = update.aEatenAnts;
            }
            if (update.HasChanged(ColonyFields.BeatenAnts))
            {
                aBeatenAnts = update.aBeatenAnts;
            }
            if (update.HasChanged(ColonyFields.KilledBugs))
            {
                aKilledBugs = update.aKilledBugs;
            }
            if (update.HasChanged(ColonyFields.KilledEnemies))
            {
                aKilledEnemies = update.aKilledEnemies;
            }
            if (update.HasChanged(ColonyFields.CollectedFood))
            {
                aCollectedFood = update.aCollectedFood;
            }
            if (update.HasChanged(ColonyFields.Points))
            {
                aPoints = update.aPoints;
            }
        }

        public ColonyUpdate GenerateUpdate(ColonyState state)
        {
            ColonyUpdate update = new ColonyUpdate();
            update.Id = Id;
            update.TeamId = TeamId;
            bool changed = false;

            if (state.StarvedAnts != aStarvedAnts)
            {
                update.Change(ColonyFields.StarvedAnts);
                update.aStarvedAnts = state.StarvedAnts;
                changed = true;
            }
            if (state.EatenAnts != aEatenAnts)
            {
                update.Change(ColonyFields.EatenAnts);
                update.aEatenAnts = state.EatenAnts;
                changed = true;
            }

            if (state.BeatenAnts != aBeatenAnts)
            {
                update.Change(ColonyFields.BeatenAnts);
                update.aBeatenAnts = state.BeatenAnts;
                changed = true;
            }

            if (state.KilledBugs != aKilledBugs)
            {
                update.Change(ColonyFields.KilledBugs);
                update.aKilledBugs = state.KilledBugs;
                changed = true;
            }
            if (state.KilledEnemies != aKilledEnemies)
            {
                update.Change(ColonyFields.KilledEnemies);
                update.aKilledEnemies = state.KilledEnemies;
                changed = true;
            }

            if (state.CollectedFood != aCollectedFood)
            {
                update.Change(ColonyFields.CollectedFood);
                update.aCollectedFood = state.CollectedFood;
                changed = true;
            }

            if (state.Points != aPoints)
            {
                update.Change(ColonyFields.Points);
                update.aPoints = state.Points;
                changed = true;
            }

            if (changed)
            {
                Update(update);
                return update;
            }
            return null;
        }

        public ColonyState GenerateState()
        {
            ColonyState state = new ColonyState(Id);
            state.Guid = Guid;
            state.PlayerName = PlayerName;
            state.ColonyName = ColonyName;
            state.StarvedAnts = StarvedAnts;
            state.EatenAnts = EatenAnts;
            state.BeatenAnts = BeatenAnts;
            state.KilledBugs = KilledBugs;
            state.KilledEnemies = KilledEnemies;
            state.CollectedFood = CollectedFood;
            state.Points = Points;
            return state;
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        #endregion

        #region ISerializable Member

        // Block layout:
        // - ushort ID
        // - guid Guid
        // - ushort Team
        // - string PlayerName
        // - string ColonyName
        // - ushort StarvedAnts
        // - ushort EatenAnts
        // - ushort BeatenAnts
        // - ushort KilledBugs
        // - ushort KilledEnemies
        // - ushort CollectedFood
        // - int Points

        public void Serialize(Serializer serializer)
        {
            serializer.SendUshort((ushort)Id);
            serializer.SendGuid(Guid);
            serializer.SendUshort((ushort)TeamId);
            serializer.SendString(PlayerName);
            serializer.SendString(ColonyName);
            serializer.SendUshort((ushort)StarvedAnts);
            serializer.SendUshort((ushort)EatenAnts);
            serializer.SendUshort((ushort)BeatenAnts);
            serializer.SendUshort((ushort)KilledBugs);
            serializer.SendUshort((ushort)KilledEnemies);
            serializer.SendUshort((ushort)CollectedFood);
            serializer.SendInt(Points);
        }

        public void Deserialize(Serializer serializer)
        {
            Id = serializer.ReadUShort();
            Guid = serializer.ReadGuid();
            TeamId = serializer.ReadUShort();
            PlayerName = serializer.ReadString();
            ColonyName = serializer.ReadString();
            StarvedAnts = serializer.ReadUShort();
            EatenAnts = serializer.ReadUShort();
            BeatenAnts = serializer.ReadUShort();
            KilledBugs = serializer.ReadUShort();
            KilledEnemies = serializer.ReadUShort();
            CollectedFood = serializer.ReadUShort();
            Points = serializer.ReadInt();
        }

        #endregion
    }
}