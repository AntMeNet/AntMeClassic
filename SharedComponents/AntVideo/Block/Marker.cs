using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block
{
    internal class Marker : MarkerState, IUpdateable<MarkerUpdate, MarkerState>, ISerializable
    {
        #region Updateinformation

        public int aRichtung;
        public int dRadius;
        private bool isAlive;

        #endregion

        public Marker(Serializer serializer)
            : base(0, 0)
        {
            Deserialize(serializer);

            Reset();
        }

        public Marker(MarkerState zustand) : base(zustand.ColonyId, zustand.Id)
        {
            PositionX = zustand.PositionX;
            PositionY = zustand.PositionY;
            Radius = zustand.Radius;
            Direction = zustand.Direction;

            Reset();
        }

        private void Reset()
        {
            aRichtung = Direction;
            dRadius = 0;
        }

        #region IUpdateable<MarkerUpdate,MarkerState> Member

        public void Interpolate()
        {
            Radius += dRadius;
            Direction = aRichtung;
        }

        public void Update(MarkerUpdate update)
        {
            if (update.HasChanged(MarkerFields.Radius))
            {
                dRadius = update.dRadius;
            }
            if (update.HasChanged(MarkerFields.Direction))
            {
                aRichtung = update.aDirection;
            }
        }

        public MarkerUpdate GenerateUpdate(MarkerState state)
        {
            MarkerUpdate update = new MarkerUpdate();
            update.Id = Id;
            bool changed = false;

            if (state.Radius != (Radius + dRadius))
            {
                update.Change(MarkerFields.Radius);
                update.dRadius = state.Radius - Radius;
                changed = true;
            }
            if (state.Direction != Direction)
            {
                update.Change(MarkerFields.Direction);
                update.aDirection = state.Direction;
                changed = true;
            }

            if (changed)
            {
                Update(update);
                return update;
            }
            return null;
        }

        public MarkerState GenerateState()
        {
            MarkerState state = new MarkerState(ColonyId, Id);
            state.PositionX = PositionX;
            state.PositionY = PositionY;
            state.Radius = Radius;
            state.Direction = Direction;
            return state;
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        #endregion

        #region ISerializable Member

        // Blocklayout:
        // - ushort Id
        // - ushort ColonyId
        // - ushort PositionX
        // - ushort PositionY
        // - ushort Radius
        // - ushort Direction
        public void Serialize(Serializer serializer)
        {
            serializer.SendUshort((ushort)Id);
            serializer.SendUshort((ushort)ColonyId);
            serializer.SendUshort((ushort)PositionX);
            serializer.SendUshort((ushort)PositionY);
            serializer.SendUshort((ushort)Radius);
            serializer.SendUshort((ushort)Direction);
        }

        public void Deserialize(Serializer serializer)
        {
            Id = serializer.ReadUShort();
            ColonyId = serializer.ReadUShort();
            PositionX = serializer.ReadUShort();
            PositionY = serializer.ReadUShort();
            Radius = serializer.ReadUShort();
            Direction = serializer.ReadUShort();
        }

        #endregion
    }
}