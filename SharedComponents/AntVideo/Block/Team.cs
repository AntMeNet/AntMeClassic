using AntMe.SharedComponents.States;
using System;

namespace AntMe.SharedComponents.AntVideo.Block
{
    internal sealed class Team : TeamState, ISerializable
    {
        public Team(TeamState state) : base(state.Id, state.Guid, state.Name) { }

        public Team(Serializer serializer)
            : base(0, new Guid(), string.Empty)
        {
            Deserialize(serializer);
        }

        public TeamState GenerateState()
        {
            TeamState state = new TeamState(Id);
            state.Guid = Guid;
            state.Name = Name;
            return state;
        }

        #region ISerializable Members

        // - ushort ID
        // - guid Guid
        // - string Name

        public void Serialize(Serializer serializer)
        {
            serializer.SendUshort((ushort)Id);
            serializer.SendGuid(Guid);
            serializer.SendString(Name);
        }

        public void Deserialize(Serializer serializer)
        {
            Id = serializer.ReadUShort();
            Guid = serializer.ReadGuid();
            Name = serializer.ReadString();
        }

        #endregion
    }
}