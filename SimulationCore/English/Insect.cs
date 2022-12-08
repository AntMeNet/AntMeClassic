using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Its the interface for all lifeforms on playground.
    /// </summary>
    public abstract class Insect : Item
    {
        internal Insect(CoreInsect insect) : base(insect) { }

        /// <summary>
        /// Returns the unique ID of this insect.
        /// </summary>
        public override int Id => ((CoreInsect)Baseitem).Id;

        /// <summary>
        /// Returns the current energy of this insect.
        /// </summary>
        public int CurrentEnergy => ((CoreInsect)Baseitem).CurrentEnergyCoreInsect;

        /// <summary>
        /// Returns the current speed of the insect.
        /// </summary>
        public int CurrentSpeed => ((CoreInsect)Baseitem).CurrentSpeedCoreInsect;

        /// <summary>
        /// Returns the attack strength of the insect.
        /// </summary>
        public int AttackStrength => ((CoreInsect)Baseitem).AttackStrengthCoreInsect;

        /// <summary>
        /// Returns the rotation speed of the insect.
        /// </summary>
        public int RotationSpeed => ((CoreInsect)Baseitem).RotationSpeedCoreInsect;

        /// <summary>
        /// Returns the maximum energy of the insect.
        /// </summary>
        public int MaximumEnergy => ((CoreInsect)Baseitem).MaximumEnergyCoreInsect;

        /// <summary>
        /// Returns the maximum speed of the insect.
        /// </summary>
        public int MaximumSpeed => ((CoreInsect)Baseitem).MaximumSpeedCoreInsect;

        /// <summary>
        /// Returns the view range of the insect.
        /// </summary>
        public int ViewRange => ((CoreInsect)Baseitem).ViewRangeCoreInsect;

        /// <summary>
        /// Returns the residual angle of the insect.
        /// </summary>
        public int ResidualAngle => ((CoreInsect)Baseitem).ResidualAngle;

        /// <summary>
        /// Returns the direction the insect is heading to.
        /// </summary>
        public int Direction => ((CoreInsect)Baseitem).GetDirectionCoreInsect();

        /// <summary>
        /// Returns the distance the insect has to cover.
        /// </summary>
        public int DistanceToDestination => ((CoreInsect)Baseitem).DistanceToDestinationCoreInsect;
    }
}