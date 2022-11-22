using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Its the interface for all lifeforms on playground
    /// </summary>
    public abstract class Insect : Item
    {
        internal Insect(CoreInsect insekt) : base(insekt) { }

        /// <summary>
        /// Returns the unique ID of this insect
        /// </summary>
        public override int Id
        {
            get { return ((CoreInsect)Baseitem).id; }
        }

        /// <summary>
        /// Delivers the current energy of this ant
        /// </summary>
        public int CurrentEnergy
        {
            get { return ((CoreInsect)Baseitem).currentEnergyBase; }
        }

        /// <summary>
        /// Delivers the current speed
        /// </summary>
        public int CurrentSpeed
        {
            get { return ((CoreInsect)Baseitem).CurrentSpeedBase; }
        }

        /// <summary>
        /// Delivers the strength
        /// </summary>
        public int AttackStrength
        {
            get { return ((CoreInsect)Baseitem).AttackStrengthBase; }
        }

        /// <summary>
        /// Delivers the rotationspeed
        /// </summary>
        public int RotationSpeed
        {
            get { return ((CoreInsect)Baseitem).RotationSpeedBase; }
        }

        /// <summary>
        /// delivers the maximum energy
        /// </summary>
        public int MaximumEnergy
        {
            get { return ((CoreInsect)Baseitem).MaximumEnergyBase; }
        }

        /// <summary>
        /// delivers the maximum speed
        /// </summary>
        public int MaximumSpeed
        {
            get { return ((CoreInsect)Baseitem).MaximumSpeedBase; }
        }

        /// <summary>
        /// delivers the viewrange
        /// </summary>
        public int Viewrange
        {
            get { return ((CoreInsect)Baseitem).ViewRangeBase; }
        }

        /// <summary>
        /// delivers the degrees to rotate
        /// </summary>
        public int DegreesToTarget
        {
            get { return ((CoreInsect)Baseitem).angleToGo; }
        }

        /// <summary>
        /// delivers the direction
        /// </summary>
        public int Direction
        {
            get { return ((CoreInsect)Baseitem).DirectionBase; }
        }

        /// <summary>
        /// delivers the distance to go
        /// </summary>
        public int DistanceToTarget
        {
            get { return ((CoreInsect)Baseitem).DistanceToDestinationBase; }
        }
    }
}