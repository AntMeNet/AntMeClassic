using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block
{
    internal sealed class Anthill : AnthillState, ISerializable
    {
        private bool isAlive;

        public Anthill(Serializer serializer) : base(0, 0)
        {
            Deserialize(serializer);
        }

        public Anthill(AnthillState state) : base(state.ColonyId, state.Id)
        {
            PositionX = state.PositionX;
            PositionY = state.PositionY;
            Radius = state.Radius;
        }

        public AnthillState GenerateState()
        {
            AnthillState state = new AnthillState(ColonyId, Id);
            state.PositionX = PositionX;
            state.PositionY = PositionY;
            state.Radius = Radius;
            return state;
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        #region ISerializable Member

        // Blocklayout:
        // - ushort Id
        // - ushort ColonyId
        // - ushort PositionX
        // - ushort PositionY
        // - ushort Radius

        public void Serialize(Serializer serializer)
        {
            serializer.SendUshort((ushort)Id);
            serializer.SendUshort((ushort)ColonyId);
            serializer.SendUshort((ushort)PositionX);
            serializer.SendUshort((ushort)PositionY);
            serializer.SendUshort((ushort)Radius);
        }

        public void Deserialize(Serializer serializer)
        {
            Id = serializer.ReadUShort();
            ColonyId = serializer.ReadUShort();
            PositionX = serializer.ReadUShort();
            PositionY = serializer.ReadUShort();
            Radius = serializer.ReadUShort();
        }

        #endregion
    }
}