using System;

namespace AntMe.SharedComponents.AntVideo.Block {
    [Flags]
    internal enum MarkerFields {
        Radius = 1,
        Direction = 2
    } ;

    internal sealed class MarkerUpdate : UpdateBase {
        public int aDirection;
        public int dRadius;

        public MarkerUpdate() {}

        // Blocklayout:
        // ...
        // - ushort aRadius
        // - ushort aDirection

        public MarkerUpdate(Serializer serializer)
            : base(serializer) {
            if (HasChanged(MarkerFields.Radius)) {
                dRadius = serializer.ReadUShort();
            }
            if (HasChanged(MarkerFields.Direction)) {
                aDirection = serializer.ReadUShort();
            }
        }

        public override void Serialize(Serializer serializer) {
            base.Serialize(serializer);
            if (HasChanged(MarkerFields.Radius)) {
                serializer.SendUshort((ushort) dRadius);
            }
            if (HasChanged(MarkerFields.Direction)) {
                serializer.SendUshort((ushort) aDirection);
            }
        }

        public void Change(MarkerFields field) {
            Change((int) field);
        }

        public bool HasChanged(MarkerFields field) {
            return HasChanged((int) field);
        }
    }
}