using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Represents a marker.
    /// </summary>
    public sealed class Marker : Item
    {
        internal Marker(CoreMarker marker) : base(marker) { }

        /// <summary>
        /// Gives the information saved in that mark.
        /// </summary>
        public int Information
        {
            get { return ((CoreMarker)Baseitem).Information; }
        }

        /// <summary>
        /// Delivers the unique ID of this marker.
        /// </summary>
        public override int Id
        {
            get { return ((CoreMarker)Baseitem).Id; }
        }
    }
}