using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block {
    internal sealed class Bug : BugState, IUpdateable<BugUpdate, BugState>, ISerializable {
        #region Updateinformation

        public int aVitality;
        public int dDirection;
        public int dPositionX;
        public int dPositionY;

        private bool isAlive;

        #endregion

        public Bug(Serializer serializer) : base(0) {
            Deserialize(serializer);

            Reset();
        }

        public Bug(BugState zustand) : base(zustand.Id) {
            PositionX = zustand.PositionX;
            PositionY = zustand.PositionY;
            Direction = zustand.Direction;
            Vitality = zustand.Vitality;

            Reset();
        }

        public void Reset() {
            dPositionX = 0;
            dPositionY = 0;
            dDirection = 0;
            aVitality = Vitality;
        }

        #region IUpdateable<BugUpdate,BugState> Member

        public void Interpolate() {
            PositionX += dPositionX;
            PositionY += dPositionY;
            Direction = Angle.Interpolate(Direction, dDirection);
            Vitality = aVitality;
        }

        public void Update(BugUpdate update) {
            if (update.HasChanged(BugFields.PositionX)) {
                dPositionX = update.dPositionX;
            }
            if (update.HasChanged(BugFields.PositionY)) {
                dPositionY = update.dPositionY;
            }
            if (update.HasChanged(BugFields.Direction)) {
                dDirection = update.dRichtung;
            }
            if (update.HasChanged(BugFields.Vitality)) {
                aVitality = update.aEnergie;
            }
        }

        public BugUpdate GenerateUpdate(BugState state) {
            BugUpdate update = new BugUpdate();
            update.Id = Id;
            bool changed = false;

            if (state.PositionX != (PositionX + dPositionX)) {
                update.Change(BugFields.PositionX);
                update.dPositionX = state.PositionX - PositionX;
                changed = true;
            }

            if (state.PositionY != (PositionY + dPositionY)) {
                update.Change(BugFields.PositionY);
                update.dPositionY = state.PositionY - PositionY;
                changed = true;
            }

            if (state.Direction != Angle.Interpolate(Direction, dDirection)) {
                update.Change(BugFields.Direction);
                update.dRichtung = Angle.Delta(Direction, state.Direction);
                changed = true;
            }

            if (state.Vitality != aVitality) {
                update.Change(BugFields.Vitality);
                update.aEnergie = state.Vitality;
                changed = true;
            }

            if (changed) {
                Update(update);
                return update;
            }
            return null;
        }

        public BugState GenerateState() {
            BugState state = new BugState(Id);
            state.PositionX = PositionX;
            state.PositionY = PositionY;
            state.Vitality = Vitality;
            state.Direction = Direction;
            return state;
        }

        public bool IsAlive {
            get { return isAlive; }
            set { isAlive = value; }
        }

        #endregion

        #region ISerializable Member

        // Blocklayout:
        // - ushort Id
        // - ushort PositionX
        // - ushort PositionY
        // - ushort Direction
        // - ushort Vitality

        public void Serialize(Serializer serializer) {
            serializer.SendUshort((ushort) Id);
            serializer.SendUshort((ushort) PositionX);
            serializer.SendUshort((ushort) PositionY);
            serializer.SendUshort((ushort) Direction);
            serializer.SendUshort((ushort) Vitality);
        }

        public void Deserialize(Serializer serializer) {
            Id = serializer.ReadUShort();
            PositionX = serializer.ReadUShort();
            PositionY = serializer.ReadUShort();
            Direction = serializer.ReadUShort();
            Vitality = serializer.ReadUShort();
        }

        #endregion
    }
}