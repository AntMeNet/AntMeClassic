using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.AntVideo.Block {
    internal sealed class Caste : CasteState, ISerializable {
        public Caste(CasteState zustand) : base(zustand.ColonyId, zustand.Id) {
            Name = zustand.Name;
            SpeedModificator = zustand.SpeedModificator;
            RotationSpeedModificator = zustand.RotationSpeedModificator;
            LoadModificator = zustand.LoadModificator;
            ViewRangeModificator = zustand.ViewRangeModificator;
            RangeModificator = zustand.RangeModificator;
            VitalityModificator = zustand.VitalityModificator;
            AttackModificator = zustand.AttackModificator;
        }

        public Caste(Serializer serializer)
            : base(0, 0) {
            Deserialize(serializer);
        }

        public CasteState GenerateState() {
            CasteState state = new CasteState(ColonyId, Id);
            state.Name = Name;
            state.SpeedModificator = SpeedModificator;
            state.RotationSpeedModificator = RotationSpeedModificator;
            state.LoadModificator = LoadModificator;
            state.ViewRangeModificator = ViewRangeModificator;
            state.RangeModificator = RangeModificator;
            state.VitalityModificator = VitalityModificator;
            state.AttackModificator = AttackModificator;
            return state;
        }

        #region ISerializable Member

        // Blocklayout:
        // - ushort Id
        // - ushort ColonyId
        // - string Name
        // - byte Speed
        // - byte Rotate
        // - byte Load
        // - byte Viewrange
        // - byte Range
        // - byte Vitality
        // - byte Attack

        public void Serialize(Serializer serializer) {
            serializer.SendUshort((ushort) Id);
            serializer.SendUshort((ushort) ColonyId);
            serializer.SendString(Name);
            serializer.SendByte(SpeedModificator);
            serializer.SendByte(RotationSpeedModificator);
            serializer.SendByte(LoadModificator);
            serializer.SendByte(ViewRangeModificator);
            serializer.SendByte(RangeModificator);
            serializer.SendByte(VitalityModificator);
            serializer.SendByte(AttackModificator);
        }

        public void Deserialize(Serializer serializer) {
            Id = serializer.ReadUShort();
            ColonyId = serializer.ReadUShort();
            Name = serializer.ReadString();
            SpeedModificator = serializer.ReadByte();
            RotationSpeedModificator = serializer.ReadByte();
            LoadModificator = serializer.ReadByte();
            ViewRangeModificator = serializer.ReadByte();
            RangeModificator = serializer.ReadByte();
            VitalityModificator = serializer.ReadByte();
            AttackModificator = serializer.ReadByte();
        }

        #endregion
    }
}