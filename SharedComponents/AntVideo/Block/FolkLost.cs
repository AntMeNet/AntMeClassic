using System.IO;

namespace AntMe.SharedComponents.AntVideo.Block {
    internal class FolkLost : Lost, ISerializable {
        private int folkId;

        public FolkLost(int folkId, int id) : base(id) {
            this.folkId = (ushort) folkId;
        }

        public FolkLost(Stream stream) : base(stream) {
            Deserialize(stream);
        }

        public int FolkId {
            get { return folkId; }
        }

        #region ISerializable Member

        public override void Serialize(Stream outputstream) {
            base.Serialize(outputstream);
            Serializer.SendUshort(outputstream, folkId);
        }

        public new void Deserialize(Stream stream) {
            folkId = Serializer.ReadUShort(stream);
        }

        #endregion
    }
}