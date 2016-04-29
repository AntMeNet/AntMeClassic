using System;

namespace AntMe.SharedComponents.AntVideo.Block {
    [Flags]
    internal enum ColonyFields {
        CollectedFood = 1,
        StarvedAnts = 2,
        EatenAnts = 4,
        BeatenAnts = 8,
        KilledBugs = 16,
        KilledEnemies = 32,
        Points = 64
    }

    internal sealed class ColonyUpdate : UpdateBase {
        public int TeamId;
        public int aBeatenAnts;
        public int aEatenAnts;
        public int aCollectedFood;
        public int aKilledBugs;
        public int aKilledEnemies;
        public int aPoints;
        public int aStarvedAnts;

        public ColonyUpdate() {}

        // Blocklayout:
        // - ...
        // - ushort TeamId
        // - ushort CollectedFood
        // - ushort StarvedAnts
        // - ushort EatenAnts
        // - ushort BeatenAnts
        // - ushort KilledBugs
        // - ushort KilledEnemies
        // - int Points

        public ColonyUpdate(Serializer serializer)
            : base(serializer) {
            TeamId = serializer.ReadUShort();

            if (HasChanged(ColonyFields.CollectedFood)) {
                aCollectedFood = serializer.ReadUShort();
            }

            if (HasChanged(ColonyFields.StarvedAnts)) {
                aStarvedAnts = serializer.ReadUShort();
            }
            if (HasChanged(ColonyFields.EatenAnts)) {
                aEatenAnts = serializer.ReadUShort();
            }
            if (HasChanged(ColonyFields.BeatenAnts)) {
                aBeatenAnts = serializer.ReadUShort();
            }

            if (HasChanged(ColonyFields.KilledBugs)) {
                aKilledBugs = serializer.ReadUShort();
            }
            if (HasChanged(ColonyFields.KilledEnemies)) {
                aKilledEnemies = serializer.ReadUShort();
            }

            if (HasChanged(ColonyFields.Points)) {
                aPoints = serializer.ReadInt();
            }
        }

        public override void Serialize(Serializer serializer) {
            base.Serialize(serializer);
            serializer.SendUshort((ushort) TeamId);

            if (HasChanged(ColonyFields.CollectedFood)) {
                serializer.SendUshort((ushort) aCollectedFood);
            }

            if (HasChanged(ColonyFields.StarvedAnts)) {
                serializer.SendUshort((ushort) aStarvedAnts);
            }
            if (HasChanged(ColonyFields.EatenAnts)) {
                serializer.SendUshort((ushort) aEatenAnts);
            }
            if (HasChanged(ColonyFields.BeatenAnts)) {
                serializer.SendUshort((ushort) aBeatenAnts);
            }

            if (HasChanged(ColonyFields.KilledBugs)) {
                serializer.SendUshort((ushort) aKilledBugs);
            }
            if (HasChanged(ColonyFields.KilledEnemies)) {
                serializer.SendUshort((ushort) aKilledEnemies);
            }

            if (HasChanged(ColonyFields.Points)) {
                serializer.SendInt(aPoints);
            }
        }

        public void Change(ColonyFields field) {
            Change((int) field);
        }

        public bool HasChanged(ColonyFields field) {
            return HasChanged((int) field);
        }
    }
}