using AntMe.Simulation;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Hilfsklasse für Entfernungs- und Richtungsberechnungen
    /// </summary>
    public static class Koordinate
    {
        /// <summary>
        /// Bestimmt die Entfernung zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Schritten</returns>
        public static int BestimmeEntfernung(Spielobjekt objekt1, Spielobjekt objekt2)
        {
            return CoreCoordinate.DetermineDistance(objekt1.Element, objekt2.Element);
        }

        /// <summary>
        /// Bestimmt die Entfernung zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Schritten</returns>
        public static int BestimmeEntfernung(CoreAnt objekt1, Spielobjekt objekt2)
        {
            return CoreCoordinate.DetermineDistance(objekt1, objekt2.Element);
        }

        /// <summary>
        /// Bestimmt die Entfernung zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Schritten</returns>
        public static int BestimmeEntfernung(Spielobjekt objekt1, CoreAnt objekt2)
        {
            return CoreCoordinate.DetermineDistance(objekt1.Element, objekt2);
        }

        /// <summary>
        /// Bestimmt die Entfernung zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Schritten</returns>
        public static int BestimmeEntfernung(CoreAnt objekt1, CoreAnt objekt2)
        {
            return CoreCoordinate.DetermineDistance(objekt1, objekt2);
        }

        /// <summary>
        /// Ermittelt den Richtungsunterscheid zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Gradschritten</returns>
        public static int BestimmeRichtung(Spielobjekt objekt1, Spielobjekt objekt2)
        {
            return CoreCoordinate.DetermineDirection(objekt1.Element, objekt2.Element);
        }

        /// <summary>
        /// Ermittelt den Richtungsunterscheid zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Gradschritten</returns>
        public static int BestimmeRichtung(CoreAnt objekt1, Spielobjekt objekt2)
        {
            return CoreCoordinate.DetermineDirection(objekt1, objekt2.Element);
        }

        /// <summary>
        /// Ermittelt den Richtungsunterscheid zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Gradschritten</returns>
        public static int BestimmeRichtung(Spielobjekt objekt1, CoreAnt objekt2)
        {
            return CoreCoordinate.DetermineDirection(objekt1.Element, objekt2);
        }

        /// <summary>
        /// Ermittelt den Richtungsunterscheid zwischen zwei Spielelementen
        /// </summary>
        /// <param name="objekt1">Element 1</param>
        /// <param name="objekt2">Element 2</param>
        /// <returns>Entfernung in Gradschritten</returns>
        public static int BestimmeRichtung(CoreAnt objekt1, CoreAnt objekt2)
        {
            return CoreCoordinate.DetermineDirection(objekt1, objekt2);
        }
    }
}