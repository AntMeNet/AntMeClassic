namespace AntMe.Deutsch
{
    /// <summary>
    /// Repräsentiert ein beliebiges Lebewesen auf dem Spielfeld
    /// </summary>
    public abstract class Insekt : Spielobjekt
    {
        internal Insekt(Simulation.CoreInsect insekt) : base(insekt) { }

        /// <summary>
        /// Liefer die eindeutige ID dieses Insektes
        /// </summary>
        public override int Id
        {
            get { return ((Simulation.CoreInsect)element).id; }
        }

        /// <summary>
        /// Gibt die aktuelle Lebensenergie an
        /// </summary>
        public int AktuelleEnergie
        {
            get { return ((Simulation.CoreInsect)element).currentEnergyBase; }
        }

        /// <summary>
        /// Gibt die aktuelle geschwindigkeit an
        /// </summary>
        public int AktuelleGeschwindigkeit
        {
            get { return ((Simulation.CoreInsect)element).CurrentSpeedBase; }
        }

        /// <summary>
        /// Gibt die Angriffsstärke an
        /// </summary>
        public int Angriffstaerke
        {
            get { return ((Simulation.CoreInsect)element).AttackStrengthBase; }
        }

        /// <summary>
        /// Gibt die aktuelle Geschwindigkeit an
        /// </summary>
        public int Drehgeschwindigkeit
        {
            get { return ((Simulation.CoreInsect)element).RotationSpeedBase; }
        }

        /// <summary>
        /// Gibt die maximale Lebensenergie an
        /// </summary>
        public int MaximaleEnergie
        {
            get { return ((Simulation.CoreInsect)element).MaximumEnergyBase; }
        }

        /// <summary>
        /// Gibt die maximale Geschwindigkeit an
        /// </summary>
        public int MaximaleGeschwindigkeit
        {
            get { return ((Simulation.CoreInsect)element).MaximumSpeedBase; }
        }

        /// <summary>
        /// Gibt die Sichtweite an
        /// </summary>
        public int Sichtweite
        {
            get { return ((Simulation.CoreInsect)element).ViewRangeBase; }
        }

        /// <summary>
        /// Gibt den noch zu rotierenden Winkel an
        /// </summary>
        public int RestWinkel
        {
            get { return ((Simulation.CoreInsect)element).ResidualAngle; }
        }

        /// <summary>
        /// Gibt die Ausrichtung der Ameise an
        /// </summary>
        public int Richtung
        {
            get { return ((Simulation.CoreInsect)element).DirectionBase; }
        }

        /// <summary>
        /// Gibt die noch zu laufende Strecke an
        /// </summary>
        public int RestStrecke
        {
            get { return ((Simulation.CoreInsect)element).DistanceToDestinationBase; }
        }
    }
}