using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block
{
    internal sealed class Fruit : FruitState, IUpdateable<FruitUpdate, FruitState>, ISerializable
    {
        #region Updateinformation

        public byte aCarringAnts;
        public int dPositionX;
        public int dPositionY;

        private bool isAlive;

        #endregion

        public Fruit(Serializer serializer)
            : base(0)
        {
            Deserialize(serializer);

            Reset();
        }

        public Fruit(FruitState state) : base(state.Id)
        {
            Amount = state.Amount;
            Radius = state.Radius;
            PositionX = state.PositionX;
            PositionY = state.PositionY;
            CarryingAnts = state.CarryingAnts;

            Reset();
        }

        private void Reset()
        {
            dPositionX = 0;
            dPositionY = 0;
            aCarringAnts = CarryingAnts;
        }

        #region IUpdateable<FruitUpdate,FruitState> Member

        public void Interpolate()
        {
            PositionX += dPositionX;
            PositionY += dPositionY;
            CarryingAnts = aCarringAnts;
        }

        public void Update(FruitUpdate update)
        {
            if (update.HasChanged(FruitFields.PositionX))
            {
                dPositionX = update.dPositionX;
            }

            if (update.HasChanged(FruitFields.PositionY))
            {
                dPositionY = update.dPositionY;
            }
            if (update.HasChanged(FruitFields.CarringAnts))
            {
                aCarringAnts = update.aCarringAnts;
            }
        }

        public FruitUpdate GenerateUpdate(FruitState state)
        {
            FruitUpdate update = new FruitUpdate();
            update.Id = Id;
            bool changed = false;

            if (state.PositionX != (PositionX + dPositionX))
            {
                update.Change(FruitFields.PositionX);
                update.dPositionX = state.PositionX - PositionX;
                changed = true;
            }
            if (state.PositionY != (PositionY + dPositionY))
            {
                update.Change(FruitFields.PositionY);
                update.dPositionY = state.PositionY - PositionY;
                changed = true;
            }
            if (state.CarryingAnts != CarryingAnts)
            {
                update.Change(FruitFields.CarringAnts);
                update.aCarringAnts = state.CarryingAnts;
                changed = true;
            }

            if (changed)
            {
                Update(update);
                return update;
            }
            return null;
        }

        public FruitState GenerateState()
        {
            FruitState state = new FruitState(Id);
            state.PositionX = PositionX;
            state.PositionY = PositionY;
            state.Radius = Radius;
            state.Amount = Amount;
            state.CarryingAnts = CarryingAnts;
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
        // - ushort Amount
        // - ushort Radius
        // - ushort PositionX
        // - ushort PositionY
        // - byte CarringAnts

        public void Serialize(Serializer serializer)
        {
            serializer.SendUshort((ushort)Id);
            serializer.SendUshort((ushort)Amount);
            serializer.SendUshort((ushort)Radius);
            serializer.SendUshort((ushort)PositionX);
            serializer.SendUshort((ushort)PositionY);
            serializer.SendByte(CarryingAnts);
        }

        public void Deserialize(Serializer serializer)
        {
            Id = serializer.ReadUShort();
            Amount = serializer.ReadUShort();
            Radius = serializer.ReadUShort();
            PositionX = serializer.ReadUShort();
            PositionY = serializer.ReadUShort();
            CarryingAnts = serializer.ReadByte();
        }

        #endregion
    }
}