using System;

using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block {
    /// <summary>
    /// Repräsentiert den Zustandsblock
    /// </summary>
    internal sealed class Frame : SimulationState, IUpdateable<FrameUpdate, SimulationState>, ISerializable {
        #region Updateinformation

        private int aCurrentRound;
        private DateTime aTimestamp;
        private bool isAlive;

        #endregion

        public Frame(SimulationState state) {
            TotalRounds = state.TotalRounds;
            PlaygroundHeight = state.PlaygroundHeight;
            PlaygroundWidth = state.PlaygroundWidth;
            TimeStamp = state.TimeStamp;
            CurrentRound = state.CurrentRound;

            Reset();
        }

        public Frame(Serializer serializer) {
            Deserialize(serializer);

            Reset();
        }

        private void Reset() {
            aTimestamp = TimeStamp;
            aCurrentRound = CurrentRound;
        }

        #region IUpdateable<FrameUpdate,SimulationState> Member

        public void Interpolate() {
            TimeStamp = aTimestamp;
            CurrentRound = aCurrentRound;
        }

        public void Update(FrameUpdate update) {
            aTimestamp = update.aTimestamp;
            aCurrentRound = update.aCurrentRound;
        }

        public FrameUpdate GenerateUpdate(SimulationState state) {
            FrameUpdate update = new FrameUpdate();
            update.aTimestamp = state.TimeStamp;
            update.aCurrentRound = state.CurrentRound;
            Update(update);
            return update;
        }

        public SimulationState GenerateState() {
            SimulationState state = new SimulationState();
            state.CurrentRound = CurrentRound;
            state.PlaygroundHeight = PlaygroundHeight;
            state.PlaygroundWidth = PlaygroundWidth;
            state.TimeStamp = TimeStamp;
            state.TotalRounds = TotalRounds;
            return state;
        }

        public bool IsAlive {
            get { return isAlive; }
            set { isAlive = value; }
        }

        #endregion

        #region ISerializable Member

        // Blocklayout:
        // ushort TotalRounds
        // ushort Height
        // ushort Width
        // timestamp Timestamp
        // ushort CurrentRound

        public void Serialize(Serializer serializer) {
            serializer.SendUshort((ushort) TotalRounds);
            serializer.SendUshort((ushort) PlaygroundHeight);
            serializer.SendUshort((ushort) PlaygroundWidth);
            serializer.SendDateTime(TimeStamp);
            serializer.SendUshort((ushort) CurrentRound);
        }

        public void Deserialize(Serializer serializer) {
            TotalRounds = serializer.ReadUShort();
            PlaygroundHeight = serializer.ReadUShort();
            PlaygroundWidth = serializer.ReadUShort();
            TimeStamp = serializer.ReadDateTime();
            CurrentRound = serializer.ReadUShort();
        }

        #endregion
    }
}