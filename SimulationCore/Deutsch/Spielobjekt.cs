using AntMe.Simulation;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Allgemeines Spielelement
    /// </summary>
    public abstract class Spielobjekt
    {
        /// <summary>
        /// Speichert die Referenz auf das original Spielobjekt
        /// </summary>
        protected ICoordinate element;

        internal Spielobjekt(ICoordinate element)
        {
            this.element = element;
        }

        /// <summary>
        /// Gibt das Basiselement zurück
        /// </summary>
        internal ICoordinate Element
        {
            get { return element; }
        }

        /// <summary>
        /// Liefert die eindeutige ID dieses Elements
        /// </summary>
        public abstract int Id { get; }

        #region Vergleichsoperatoren

        /// <summary>
        /// operator ==
        /// </summary>
        /// <param name="a">Vergleichsobjekt 1</param>
        /// <param name="b">Vergleichsobjekt 2</param>
        /// <returns></returns>
        public static bool operator ==(Spielobjekt a, Spielobjekt b)
        {
            // prüfen, ob beide Elemente null sind
            if ((object)a == null)
            {
                if ((object)b == null)
                {
                    return true;
                }
                return false;
            }

            // prüfen, ob b null ist
            if ((object)b == null)
            {
                return false;
            }

            // Beides Instanzen - echte Prüfung
            return a.Equals(b);
        }

        /// <summary>
        /// operator !=
        /// </summary>
        /// <param name="a">Vergleichsobjekt 1</param>
        /// <param name="b">Vergleichsobjekt 2</param>
        /// <returns></returns>
        public static bool operator !=(Spielobjekt a, Spielobjekt b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Vergleicht dieses Element mit einem anderen
        /// </summary>
        /// <param name="obj">Vergleichsobjekt</param>
        /// <returns>Haben den gleichen Inhalt</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() == GetType())
            {
                return obj.GetHashCode() == GetHashCode();
            }
            return false;
        }

        /// <summary>
        /// Erstellt einen eindeutigen Code für dieses Objekt
        /// </summary>
        /// <returns>eindeutiger Code</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}