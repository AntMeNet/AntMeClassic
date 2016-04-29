using System;

namespace AntMe.Simulation {


    /// <summary>
    /// Die Position eines Objekts auf dem Spielfeld.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public struct CoreCoordinate {
        private int radius;
        private int richtung;
        private int x;
        private int y;

        /// <summary>
        /// Erzeugt eine neue Instanz der Koordinate-Struktur.
        /// </summary>
        /// <param name="x">Der X-Wert des Elementes.</param>
        /// <param name="y">Der Y-Wert des Elementes.</param>
        /// <param name="radius">Der Radius des Elementes.</param>
        /// <param name="richtung">Die Richtung in die das Element blickt.</param>
        internal CoreCoordinate(int x, int y, int radius, int richtung) {
            this.x = x*SimulationEnvironment.PLAYGROUND_UNIT;
            this.y = y*SimulationEnvironment.PLAYGROUND_UNIT;

            // In diesem Konstruktor müssen alle Werte der Struktur initialisiert
            // werden, deswegen werden Radius und Richtung hier gesetzt, obwohl sie
            // im Anschluß gleich wieder überschrieben werden.
            this.radius = 0;
            this.richtung = 0;

            Radius = radius*SimulationEnvironment.PLAYGROUND_UNIT;
            Richtung = richtung;
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der Koordinate-Struktur.
        /// </summary>
        /// <param name="x">Der X-Wert des Elementes.</param>
        /// <param name="y">Der Y-Wert des Elementes.</param>
        /// <param name="radius">Der Radius des Elementes.</param>
        internal CoreCoordinate(int x, int y, int radius) {
            this.x = x*SimulationEnvironment.PLAYGROUND_UNIT;
            this.y = y*SimulationEnvironment.PLAYGROUND_UNIT;
            this.radius = 0;
            richtung = 0;
            Radius = radius*SimulationEnvironment.PLAYGROUND_UNIT;
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der Koordinate-Struktur.
        /// </summary>
        /// <param name="x">Der X-Wert des Elementes.</param>
        /// <param name="y">Der Y-Wert des Elementes.</param>
        internal CoreCoordinate(int x, int y) {
            this.x = x*SimulationEnvironment.PLAYGROUND_UNIT;
            this.y = y*SimulationEnvironment.PLAYGROUND_UNIT;
            radius = 0;
            richtung = 0;
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der Koordinate-Struktur ausgehend von einer
        /// bestehenden Koordinate.
        /// </summary>
        /// <param name="k">Die bestehende Koordinate.</param>
        /// <param name="deltaX">Der X-Wert relativ zu der Koordinate.</param>
        /// <param name="deltaY">Der Y-Wert relativ zu der Koordinate.</param>
        internal CoreCoordinate(CoreCoordinate k, int deltaX, int deltaY) {
            x = k.x + deltaX;
            y = k.y + deltaY;
            radius = k.radius;
            richtung = k.richtung;
        }

        /// <summary>
        /// Der X-Wert des Elements.
        /// </summary>
        internal int X {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Der Y-Wert des Elements.
        /// </summary>
        internal int Y {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Der Radius des Elementes. Da alle Berechnungen in der Spiel (z.B.
        /// die Bestimmung von Entfernungen) auf Punkten und Kreisen basiert, wird
        /// auch der Radius eines Objektes in der IKoordinate Struktur gespeichert.
        /// </summary>
        internal int Radius {
            get { return radius; }
            set { radius = Math.Abs(value); }
        }

        /// <summary>
        /// Die Richtung in die das Element blickt. Eine Richtung ist zwar kein
        /// echter Teil einer Koordinate und nicht alle Elemente die eine Koordinate
        /// haben benötigen die Richtung, aber die IKoordinate-Struktur ist der
        /// beste Platz um auch die Richtung eines Objektes zu speichern.
        /// </summary>
        internal int Richtung {
            get { return richtung; }
            set {
                richtung = value;
                while (richtung < 0) {
                    richtung += 360;
                }
                while (richtung > 359) {
                    richtung -= 360;
                }
            }
        }

        /// <summary>
        /// Bestimmt die Entfernung zweier Objekte auf dem Spielfeld in Schritten.
        /// </summary>
        /// <param name="o1">Objekt 1.</param>
        /// <param name="o2">Objekt 2.</param>
        /// <returns>Die Entfernung.</returns>
        internal static int BestimmeEntfernung(ICoordinate o1, ICoordinate o2) {
            return BestimmeEntfernungI(o1.CoordinateBase, o2.CoordinateBase)/SimulationEnvironment.PLAYGROUND_UNIT;
        }

        /// <summary>
        /// Bestimmt die Richtung von einem Objekt auf dem Spielfeld zu einem
        /// anderen.
        /// </summary>
        /// <param name="o1">Das Start Objekt.</param>
        /// <param name="o2">Das Ziel Objekt.</param>
        /// <returns>Die Richtung.</returns>
        internal static int BestimmeRichtung(ICoordinate o1, ICoordinate o2) {
            return BestimmeRichtung(o1.CoordinateBase, o2.CoordinateBase);
        }

        /// <summary>
        /// Bestimmt die Entfernung zweier Koordinaten auf dem Spielfeld in der
        /// internen Einheit.
        /// </summary>
        /// <param name="k1">Koordinate 1.</param>
        /// <param name="k2">Koordinate 2.</param>
        /// <returns>Die Entfernung.</returns>
        internal static int BestimmeEntfernungI(CoreCoordinate k1, CoreCoordinate k2) {
            double deltaX = k1.x - k2.x;
            double deltaY = k1.y - k2.y;
            int entfernung = (int) Math.Round(Math.Sqrt(deltaX*deltaX + deltaY*deltaY));
            entfernung = entfernung - k1.radius - k2.radius;
            if (entfernung < 0) {
                return 0;
            }
            return entfernung;
        }

        /// <summary>
        /// Bestimmt die Entfernung zweier Koordinaten auf dem Spielfeld in der 
        /// internen Einheit ohne Beachtung der Radien.
        /// </summary>
        /// <param name="k1">Koordinate 1.</param>
        /// <param name="k2">Koordinate 2.</param>
        /// <returns>Die Entfernung.</returns>
        internal static int BestimmeEntfernungDerMittelpunkteI(CoreCoordinate k1, CoreCoordinate k2) {
            double deltaX = k1.x - k2.x;
            double deltaY = k1.y - k2.y;
            return (int) Math.Round(Math.Sqrt(deltaX*deltaX + deltaY*deltaY));
        }

        /// <summary>
        /// Bestimmt die Richtung von einer Koordinate auf dem Spielfeld zu einer
        /// anderen.
        /// </summary>
        /// <param name="k1">Die Start Koordinate.</param>
        /// <param name="k2">Die Ziel Koordinate.</param>
        /// <returns>Die Richtung.</returns>
        internal static int BestimmeRichtung(CoreCoordinate k1, CoreCoordinate k2) {
            int richtung = (int) Math.Round(Math.Atan2(k2.Y - k1.Y, k2.X - k1.X)*180d/Math.PI);
            if (richtung < 0) {
                richtung += 360;
            }
            return richtung;
        }
    }
}