namespace AntMe.SharedComponents.AntVideo.Block {
    internal class Lost : ISerializable {
        private int id;

        public Lost(int id) {
            this.id = id;
        }

        public Lost(Serializer serializer) {
            Deserialize(serializer);
        }

        public int Id {
            get { return id; }
        }

        #region ISerializable Member

        public virtual void Serialize(Serializer serializer) {
            serializer.SendUshort((ushort) id);
        }

        public void Deserialize(Serializer serializer) {
            id = serializer.ReadUShort();
        }

        #endregion
    }
}