namespace AntMe.SharedComponents.AntVideo {
    /// <summary>
    /// Interface for all serializable blocks.
    /// </summary>
    internal interface ISerializable {
        /// <summary>
        /// Serializes the object into the given stream.
        /// </summary>
        /// <param name="serializer">output-stream</param>
        void Serialize(Serializer serializer);

        /// <summary>
        /// De-serializes the object out of given stream.
        /// </summary>
        /// <param name="serializer">input-stream</param>
        void Deserialize(Serializer serializer);
    }
}