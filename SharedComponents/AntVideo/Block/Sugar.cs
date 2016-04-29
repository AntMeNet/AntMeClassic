using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block {
    internal sealed class Sugar : SugarState, IUpdateable<SugarUpdate, SugarState>, ISerializable {
        #region Updateinformation

        private bool m_isAlive;
        private int m_aAmount;
        private int m_aRadius;

        #endregion

        public Sugar(Serializer serializer)
            : base(0) {
            Deserialize(serializer);

            Reset();
        }

        public Sugar(SugarState zustand) : base(zustand.Id) {
            PositionX = zustand.PositionX;
            PositionY = zustand.PositionY;
            Radius = zustand.Radius;
            Amount = zustand.Amount;

            Reset();
        }

        private void Reset() {
            m_aAmount = Amount;
            m_aRadius = Radius;
        }

        #region IUpdateable<SugarUpdate,SugarState> Member

        public void Interpolate() {
            Amount = m_aAmount;
            Radius = m_aRadius;
        }

        public void Update(SugarUpdate update) {
            if (update.HasChanged(SugarFields.Amount)) {
                m_aAmount = update.AbsoluteAmount;
            }
            if (update.HasChanged(SugarFields.Range)) {
                m_aRadius = update.AbsoluteRadius;
            }
        }

        public SugarUpdate GenerateUpdate(SugarState state) {
            SugarUpdate update = new SugarUpdate();
            update.Id = Id;
            bool changed = false;

            if (Radius != state.Radius) {
                update.Change(SugarFields.Range);
                update.AbsoluteRadius = (ushort) state.Radius;
                changed = true;
            }
            if (Amount != state.Amount) {
                update.Change(SugarFields.Amount);
                update.AbsoluteAmount = (ushort) state.Amount;
                changed = true;
            }

            if (changed) {
                Update(update);
                return update;
            }
            return null;
        }

        public SugarState GenerateState() {
            SugarState zustand = new SugarState(Id);
            zustand.PositionX = PositionX;
            zustand.PositionY = PositionY;
            zustand.Radius = Radius;
            zustand.Amount = Amount;
            return zustand;
        }

        public bool IsAlive {
            get { return m_isAlive; }
            set { m_isAlive = value; }
        }

        #endregion

        #region ISerializable Member

        // Blocklayout:
        // - ushort ID
        // - ushort PosX
        // - ushort PosY
        // - ushort Amount
        // - ushort Radius

        public void Serialize(Serializer serializer) {
            serializer.SendUshort((ushort) Id);
            serializer.SendUshort((ushort) PositionX);
            serializer.SendUshort((ushort) PositionY);
            serializer.SendUshort((ushort) Amount);
            serializer.SendUshort((ushort) Radius);
        }

        public void Deserialize(Serializer serializer) {
            Id = serializer.ReadUShort();
            PositionX = serializer.ReadUShort();
            PositionY = serializer.ReadUShort();
            Amount = serializer.ReadUShort();
            Radius = serializer.ReadUShort();
        }

        #endregion
    }
}