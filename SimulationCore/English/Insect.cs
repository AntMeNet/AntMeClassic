﻿using AntMe.Simulation;

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
            get { return ((CoreInsect)Baseitem).AktuelleEnergieBase; }
        }

        /// <summary>
        /// Delivers the current speed
        /// </summary>
        public int CurrentSpeed
        {
            get { return ((CoreInsect)Baseitem).AktuelleGeschwindigkeitBase; }
        }

        /// <summary>
        /// Delivers the strength
        /// </summary>
        public int AttackStrength
        {
            get { return ((CoreInsect)Baseitem).AngriffBase; }
        }

        /// <summary>
        /// Delivers the rotationspeed
        /// </summary>
        public int RotationSpeed
        {
            get { return ((CoreInsect)Baseitem).DrehgeschwindigkeitBase; }
        }

        /// <summary>
        /// delivers the maximum energy
        /// </summary>
        public int MaximumEnergy
        {
            get { return ((CoreInsect)Baseitem).MaximaleEnergieBase; }
        }

        /// <summary>
        /// delivers the maximum speed
        /// </summary>
        public int MaximumSpeed
        {
            get { return ((CoreInsect)Baseitem).MaximaleGeschwindigkeitBase; }
        }

        /// <summary>
        /// delivers the viewrange
        /// </summary>
        public int Viewrange
        {
            get { return ((CoreInsect)Baseitem).SichtweiteBase; }
        }

        /// <summary>
        /// delivers the degrees to rotate
        /// </summary>
        public int DegreesToTarget
        {
            get { return ((CoreInsect)Baseitem).RestWinkelBase; }
        }

        /// <summary>
        /// delivers the direction
        /// </summary>
        public int Direction
        {
            get { return ((CoreInsect)Baseitem).RichtungBase; }
        }

        /// <summary>
        /// delivers the distance to go
        /// </summary>
        public int DistanceToTarget
        {
            get { return ((CoreInsect)Baseitem).RestStreckeBase; }
        }
    }
}