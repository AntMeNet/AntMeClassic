using AntMe.SharedComponents.States;
using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// scent marking with information, age and size
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreMarker : ICoordinate
    {
        // id of the next marker
        private static int newId = 0;

        /// <summary>
        /// id will uniquely identify the marker throughout the simulation
        /// </summary>
        public readonly int Id;

        private readonly int colonyId;
        private CoreCoordinate coordinate;

        private int age = 0;
        private int totalAge;
        private int finalSize;
        private int information;


        /// <summary>
        /// construtor for new instances of marker
        /// </summary>
        /// <param name="coordinate">coordinate</param>
        /// <param name="finalSize">size of marker in steps</param>
        /// <param name="colonyId">id of colony</param>
        internal CoreMarker(CoreCoordinate coordinate, int finalSize, int colonyId)
        {
            this.colonyId = colonyId;
            Id = newId++;
            this.coordinate = coordinate;

            // calculation of the smallest possible marker volume (r-square * PI/2) for the semi-sphere 
            double baseVolume = Math.Pow(SimulationSettings.Custom.MarkerSizeMinimum, 3) * (Math.PI / 2);

            // correction for bigger markers
            baseVolume *= 10f;

            // total volume for the hole lifespan
            double totalvolume = baseVolume * SimulationSettings.Custom.MarkerMaximumAge;

            // calculation of maximum size
            int maxSize = (int)Math.Pow(4 * ((totalvolume - baseVolume) / Math.PI), 1f / 3f);

            // final size limited by minum and maximum markersize
            this.finalSize = Math.Max(SimulationSettings.Custom.MarkerSizeMinimum, Math.Min(maxSize, finalSize));

            // calculation of volume for the maximum marker //// MarkerSizeMinimum?
            int diffRadius = this.finalSize - SimulationSettings.Custom.MarkerSizeMinimum;
            double diffVolume = Math.Pow(diffRadius, 3) * (Math.PI / 4);

            // total age of the marker depends on the size
            totalAge = (int)Math.Floor(totalvolume / (baseVolume + diffVolume));
            Update();
        }

        /// <summary>
        /// marker information
        /// </summary>
        public int Information
        {
            get { return information; }
            internal set { information = value; }
        }

        /// <summary>
        /// false means that the marker is not active any more 
        /// because the age is greater than the total age
        /// </summary>
        public bool IsActive
        {
            get { return age <= totalAge; }
        }

        #region IKoordinate Members

        /// <summary>
        /// Die Position der Markierung auf dem Spielfeld.
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return coordinate; }
        }

        #endregion

        /// <summary>
        /// updates age and radius of the marker 
        /// </summary>
        internal void Update()
        {
            age++;
            if (IsActive)
            {
                coordinate.Radius = (int)(
                    SimulationSettings.Custom.MarkerSizeMinimum +
                    finalSize * ((float)age / totalAge)) * SimulationEnvironment.PLAYGROUND_UNIT;
            }
        }

        /// <summary>
        /// populates marker state with current informations of the marker
        /// </summary>
        internal MarkerState PopulateMarkerState()
        {
            MarkerState markerStateInformation = new MarkerState(colonyId, Id);
            markerStateInformation.PositionX = coordinate.X / SimulationEnvironment.PLAYGROUND_UNIT;
            markerStateInformation.PositionY = coordinate.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            markerStateInformation.Radius = coordinate.Radius / SimulationEnvironment.PLAYGROUND_UNIT;
            markerStateInformation.Direction = coordinate.Direction;
            return markerStateInformation;
        }
    }
}