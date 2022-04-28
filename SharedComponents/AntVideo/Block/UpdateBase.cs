using System;

namespace AntMe.SharedComponents.AntVideo.Block
{
    /// <summary>
    /// Basisklasse für alle Updateblocks für ein einheitliches Speichern der veränderten Felder
    /// </summary>
    internal abstract class UpdateBase : ISerializable
    {
        #region internal Variables

        private int m_id;
        private int m_changedFields;

        #endregion

        protected UpdateBase() { }

        protected UpdateBase(Serializer serializer)
        {
            m_changedFields = serializer.ReadUShort();
            m_id = serializer.ReadUShort();
        }

        /// <summary>
        /// Sets the given field to changed-state
        /// </summary>
        /// <param name="field">Changed field</param>
        protected void Change(int field)
        {
            m_changedFields |= field;
        }

        /// <summary>
        /// Indicates changes in given field.
        /// </summary>
        /// <param name="field">field to check</param>
        /// <returns>Changes in field</returns>
        protected bool HasChanged(int field)
        {
            return ((m_changedFields & field) > 0);
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        #region ISerializable Member

        // Blocklayout:
        // - ushort changedFields
        // - ushort id
        // - ...

        /// <summary>
        /// Serializes the updateinformation into the given stream.
        /// </summary>
        /// <param name="serializer">outputstream</param>
        public virtual void Serialize(Serializer serializer)
        {
            serializer.SendUshort((ushort)m_changedFields);
            serializer.SendUshort((ushort)m_id);
        }

        /// <summary>
        /// Deserializes the updateinformation out of given stream.
        /// </summary>
        /// <param name="serializer">inputstream</param>
        public void Deserialize(Serializer serializer)
        {
            throw new NotImplementedException("Deserializer is not needed.");
        }

        #endregion
    }
}