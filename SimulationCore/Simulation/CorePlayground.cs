using System;
using System.Collections.Generic;
using AntMe.Deutsch;

namespace AntMe.Simulation {
    /// <summary>
    /// Das Spielfeld.
    /// </summary>
    /// <author>Wolfgang Gallo (gallo@antme.net)</author>
    internal class CorePlayground {
        public readonly List<CoreAnthill> AntHills;
        public readonly int Width;
        public readonly int Height;
        private readonly Random mapRandom;

        public readonly List<CoreFruit> Fruits;
        public readonly List<CoreSugar> SugarHills;

        //TODO: Koordinate-Struktur benutzen.
        private Punkt[] punkte;

        /// <summary>
        /// Erzeugt eine neue Instanz der Spielfeld-Klasse.
        /// </summary>
        /// <param name="breite">Die Breite in Schritten.</param>
        /// <param name="höhe">Die Höhe in Schritten.</param>
        /// <param name="randomSeed">Initialwert für Zufallsgenerator</param>
        public CorePlayground(int breite, int höhe, int randomSeed) {
            Width = breite;
            Height = höhe;

            // Random initialize with mapInitValue
            if (randomSeed != 0)
            {
                mapRandom = new Random(randomSeed);
            }
            else {
                mapRandom = new Random();
            }

            SugarHills = new List<CoreSugar>();
            Fruits = new List<CoreFruit>();
            AntHills = new List<CoreAnthill>();
        }

        // Bestimmt den minimalen Abstand des übergebenen Punktes zu allen 
        // Elementen auf dem Spielfeld und dem Spielfeldrand.
        private int minimum(int x, int y) {
            int dx, dy;

            // Speichert die Quadrate des gemerkten und berechneten Abstandes.
            // So sparen wir uns beim Pythargoras das Wurzelziehen.
            int minimum, abstand;

            // Abstand zum Spielfeldrand bestimmen, diesen aber nur 1/2 Gewichten.
            minimum = Math.Min(Math.Min(x, Width - x), Math.Min(y, Height - y));
            minimum *= 2;
            minimum *= minimum;

            for (int index = 0; index < punkte.Length; index++) {
                dx = punkte[index].X - x;
                dy = punkte[index].Y - y;
                abstand = dx*dx + dy*dy;
                abstand -= punkte[index].Radius*punkte[index].Radius;

                minimum = Math.Min(abstand, minimum);
            }

            return minimum;
        }

        // Findet den Punkt mit dem angenähert minimalen Abstand zu allen Elementen
        // auf dem Spielfeld und dem Spielfeldrand.
        private Punkt findePunkt() {
            int index = 0;
            int anzahl = 4;
            anzahl += SugarHills.Count;
            anzahl += Fruits.Count;
            anzahl += AntHills.Count;

            punkte = new Punkt[anzahl];
            foreach (CoreSugar zucker in SugarHills) {
                punkte[index++] = new Punkt(zucker.CoordinateBase);
            }
            foreach (CoreFruit obst in Fruits) {
                punkte[index++] = new Punkt(obst.CoordinateBase);
            }
            foreach (CoreAnthill bau in AntHills) {
                punkte[index++] = new Punkt(bau.CoordinateBase);
            }

            int x, y, a;
            int maximum = 0;
            int dx = Width/100;
            int dy = Height/100;
            int nx = 0;
            int ny = 0;

            // Die optimale Koordinate schätzen.
            for (x = 0; x < Width; x += dx) {
                for (y = 0; y < Height; y += dy) {
                    a = minimum(x, y);
                    if (a > maximum) {
                        maximum = a;
                        nx = x;
                        ny = y;
                    }
                }
            }

            // Noch ein wenig von der optimalen Koordiante abweichen.
            double radius = mapRandom.Next(0, (int) (Math.Sqrt(maximum)/3d));
            double winkel = mapRandom.Next(0, 359);
            nx += (int) (radius*Math.Cos(winkel*Math.PI/180d));
            ny += (int) (radius*Math.Sin(winkel*Math.PI/180d));

            return new Punkt(nx, ny, 0);
        }

        /// <summary>
        /// Erzeugt einen neuen Zuckerhaufen.
        /// </summary>
        public void NeuerZucker() {
            Punkt punkt = findePunkt();
            int wert = mapRandom.Next(SimulationSettings.Custom.SugarAmountMinimum, SimulationSettings.Custom.SugarAmountMaximum);
            SugarHills.Add(new CoreSugar(punkt.X, punkt.Y, wert));
        }

        /// <summary>
        /// Erzeugt ein neues Obsttück.
        /// </summary>
        public void NeuesObst() {
            Punkt punkt = findePunkt();
            int wert = mapRandom.Next(SimulationSettings.Custom.FruitAmountMinimum, SimulationSettings.Custom.FruitAmountMaximum);
            Fruits.Add(new CoreFruit(punkt.X, punkt.Y, wert));
        }

        /// <summary>
        /// Erzeugt einen neuen Bau.
        /// </summary>
        /// <param name="colony">Id of colony</param>
        /// <returns>Der neue Bau.</returns>
        public CoreAnthill NeuerBau(int colony) {
            Punkt punkt = findePunkt();
            CoreAnthill bau = new CoreAnthill(punkt.X, punkt.Y, SimulationSettings.Custom.AntHillRadius, colony);
            AntHills.Add(bau);
            return bau;
        }

        #region Nested type: Punkt

        private struct Punkt {
            /// <summary>
            /// Der Radius.
            /// </summary>
            public int Radius;

            /// <summary>
            /// Die X-Koordinate.
            /// </summary>
            public int X;

            /// <summary>
            /// Die Y-Koordinate.
            /// </summary>
            public int Y;

            /// <summary>
            /// Erzeugt eine neue Instant der Punkt-Struktur.
            /// </summary>
            /// <param name="x">Die X-Koordinate.</param>
            /// <param name="y">Die Y-Koordinate.</param>
            /// <param name="radius">Der Radius.</param>
            public Punkt(int x, int y, int radius) {
                X = x;
                Y = y;
                Radius = radius;
            }

            /// <summary>
            /// Erzeugt eine neue Instant der Punkt-Struktur aus einer Koordinate.
            /// </summary>
            /// <param name="koordinate">Die Koordinate.</param>
            public Punkt(CoreCoordinate koordinate) {
                X = koordinate.X/SimulationEnvironment.PLAYGROUND_UNIT;
                Y = koordinate.Y/SimulationEnvironment.PLAYGROUND_UNIT;
                Radius = koordinate.Radius/SimulationEnvironment.PLAYGROUND_UNIT;
            }
        }

        #endregion
    }
}