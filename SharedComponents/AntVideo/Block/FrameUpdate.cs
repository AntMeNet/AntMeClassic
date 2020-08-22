using System;

namespace AntMe.SharedComponents.AntVideo.Block
{
    internal sealed class FrameUpdate : ISerializable
    {
        public DateTime aTimestamp = DateTime.MinValue;
        public int aCurrentRound;

        public FrameUpdate() { }

        // Blocklayout:
        // - Datetime TimeStamp
        // - ushort aCurrentRound

        public FrameUpdate(Serializer serializer)
        {
            Deserialize(serializer);
        }

        #region ISerializable Member

        public void Serialize(Serializer serializer)
        {
            serializer.SendDateTime(aTimestamp);
            serializer.SendUshort((ushort)aCurrentRound);
        }

        public void Deserialize(Serializer serializer)
        {
            aTimestamp = serializer.ReadDateTime();
            aCurrentRound = serializer.ReadUShort();
        }

        #endregion
    }
}