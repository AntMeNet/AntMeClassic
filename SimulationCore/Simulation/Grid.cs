using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace AntMe.Simulation
{

    /// <summary>
    /// Implementiert ein Zellrasterverfahren zum schnellen Auffinden von
    /// Spielelementen auf dem Spielfeld.
    /// </summary>
    /// <remarks>
    /// Ein Spielelement wird unabhängig von seinem Radius in genau eine Zelle
    /// einsortiert. Spielelemente "zwei Zellen weiter" werden daher auch dann
    /// nicht gefunden, wenn sie (durch ihren Radius) eigentlich nahe genug
    /// am Referenzelement liegen.
    /// </remarks>
    /// <typeparam name="T">Typ der Spielelemente.</typeparam>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal class Grid<T> : ICollection<T>
        where T : ICoordinate
    {

        #region Statischer Teil, Fabrikmethode

        #region Struktur GridSize

        /// <summary>
        /// Speichert die Abmessungen eines Gitters.
        /// </summary>
        private struct GridSize
        {
            /// <summary>
            /// Die Breite des Gitters.
            /// </summary>
            public readonly int Width;

            /// <summary>
            /// Die Höhe des Gitters.
            /// </summary>
            public readonly int Height;

            /// <summary>
            /// Die Seitenlänge einer Gitterzelle.
            /// </summary>
            public readonly int SideLength;

            /// <summary>
            /// Erzeugt eine neue Instanz der Struktur.
            /// </summary>
            /// <param name="width">Die Breite des Gitters.</param>
            /// <param name="height">Die Höhe des Gitters.</param>
            /// <param name="sideLength">Die Seitenlänge einer Gitterzelle.</param>
            public GridSize(int width, int height, int sideLength)
            {
                Width = width;
                Height = height;
                SideLength = sideLength;
            }
        }

        #endregion

        // Liste aller bereits erzeugten Gitter.
        private static readonly Dictionary<GridSize, Grid<T>> grids =
            new Dictionary<GridSize, Grid<T>>();

        /// <summary>
        /// Erzeugt ein Gitter mit den angegebenen Maßen oder gibt ein vorhandenes
        /// Gitter mit diesem Maßen zurück.
        /// </summary>
        /// <param name="width">Breite des Gitters.</param>
        /// <param name="height">Höhe des Gitters.</param>
        /// <param name="sideLength">Seitenlänge einer Gitterzelle. Bestimmt die
        /// maximale Entfernung bis zu der ein Spielelement ein anderes sehen kann.</param>
        public static Grid<T> Create(int width, int height, int sideLength)
        {
            GridSize size = new GridSize(width, height, sideLength);
            if (!grids.ContainsKey(size))
                grids.Add(size, new Grid<T>(size));
            return grids[size];
        }

        #endregion

        // Die Abmessungen des Gitters.
        private readonly int columns;
        private readonly int rows;
        private readonly int sideLength;

        // Die einzelnen Gitterzellen.
        private readonly List<T>[,] cells;

        #region Konstruktoren

        /// <summary>
        /// Erzeugt eine neue Instanz der Klasse.
        /// </summary>
        /// <param name="size">Die Maße des Gitters.</param>
        private Grid(GridSize size)
        {
            sideLength = size.SideLength;
            columns = size.Width / size.SideLength + 1;
            rows = size.Height / size.SideLength + 1;

            cells = new List<T>[columns, rows];
            for (int c = 0; c < columns; c++)
                for (int r = 0; r < rows; r++)
                    cells[c, r] = new List<T>();
        }

        /// <summary>
        /// Erzeugt einen neue Instanz der Klasse.
        /// </summary>
        /// <param name="width">Breite des Gitters.</param>
        /// <param name="height">Höhe des Gitters.</param>
        /// <param name="sideLength">Seitenlänge einer Gitterzelle. Bestimmt die
        /// maximale Entfernung bis zu der ein Spielelement ein anderes sehen kann.</param>
        public Grid(int width, int height, int sideLength)
        {
            this.sideLength = sideLength;
            this.columns = width / sideLength + 1;
            this.rows = height / sideLength + 1;

            cells = new List<T>[this.columns, this.rows];
            for (int c = 0; c < this.columns; c++)
                for (int r = 0; r < this.rows; r++)
                    cells[c, r] = new List<T>();
        }

        #endregion

        #region Schnittstelle IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int c = 0; c < columns; c++)
                for (int r = 0; r < rows; r++)
                    for (int i = 0; i < cells[c, r].Count; i++)
                        yield return cells[c, r][i];
        }

        #endregion

        #region Schnittstelle ICollection

        private int count = 0;

        public int Count
        {
            get { return count; }
        }

        public void Clear()
        {
            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                    cells[x, y].Clear();
            count = 0;
        }

        public void Add(T element)
        {
            int c = element.CoordinateBase.X / sideLength;
            int r = element.CoordinateBase.Y / sideLength;
            if (c < 0 || c >= columns || r < 0 || r >= rows)
                return;

            cells[c, r].Add(element);
            count++;
        }

        /// <summary>
        /// Fügt dem Gitter mehrere Spielelemente hinzu.
        /// </summary>
        /// <param name="elemente">Eine Liste von Spielelementen.</param>
        public void Add(List<T> elemente)
        {
            for (int i = 0; i < elemente.Count; i++)
                Add(elemente[i]);
        }

        public bool Remove(T element)
        {
            int c = element.CoordinateBase.X / sideLength;
            int r = element.CoordinateBase.Y / sideLength;
            if (c < 0 || c >= columns || r < 0 || r >= rows)
                return false;

            bool success = cells[c, r].Remove(element);
            if (success)
                count--;
            return success;
        }

        /// <summary>
        /// Entfernt mehrere Elemente aus dem Gitter.
        /// </summary>
        /// <param name="elemente">Eine Liste von Spielelementen.</param>
        public void Remove(List<T> elemente)
        {
            for (int i = 0; i < elemente.Count; i++)
                Remove(elemente[i]);
        }

        public bool Contains(T element)
        {
            int c = element.CoordinateBase.X / sideLength;
            int r = element.CoordinateBase.Y / sideLength;
            if (c < 0 || c >= columns || r < 0 || r >= rows)
                return false;
            return cells[c, r].Contains(element);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int c = 0; c < columns; c++)
                for (int r = 0; c < rows; c++)
                {
                    cells[c, r].CopyTo(array, arrayIndex);
                    arrayIndex += cells[c, r].Count;
                }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Finden vom Spielelementen

        #region Klassen Tupel und TupelComparer, Sortieren von Spielelementen nach Entfernung

        private readonly TupelComparer<Tupel> comparer = new TupelComparer<Tupel>();

        private class Tupel
        {
            public readonly ICoordinate Element;
            public readonly int Distance;

            public Tupel(ICoordinate element, int distance)
            {
                Element = element;
                Distance = distance;
            }
        }

        private class TupelComparer<T> : IComparer<T> where T : Tupel
        {
            public int Compare(T t1, T t2)
            {
                return t1.Distance.CompareTo(t2.Distance);
            }
        }

        #endregion

        /// <summary>
        /// Findet alle Spielelemente innerhalb des gegebenen Sichtkreis des gegebenen Spielelements.
        /// </summary>
        /// <param name="coordinate">Das Referenzspielelement.</param>
        /// <param name="maximumDistance">Die maximale Entfernung.</param>
        /// <returns>Eine nach Entfernung sortierte Liste von Spielelementen.</returns>
        public List<T> FindSorted(ICoordinate coordinate, int maximumDistance)
        {
            // Speichert alle gefundenen Tupel (Spielelement, Entfernung).
            List<Tupel> tupels = new List<Tupel>();

            // Bestimme die Zelle in der das übergebene Spielelement sich befindet.
            int col = coordinate.CoordinateBase.X / sideLength;
            int row = coordinate.CoordinateBase.Y / sideLength;

            // Betrachte die Zelle und die acht Zellen daneben.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {
                            List<T> cell = cells[col + c, row + r];
                            for (int i = 0; i < cell.Count; i++)
                            {
                                if (cell[i].Equals(coordinate))
                                    continue;

                                int distance = CoreCoordinate.BestimmeEntfernungI(coordinate.CoordinateBase, cell[i].CoordinateBase);
                                if (distance <= maximumDistance)
                                    tupels.Add(new Tupel(cell[i], distance));
                            }
                        }

            // Sortiere die Tupel und gib die Spielelemente zurück.
            tupels.Sort(comparer);
            List<T> elements = new List<T>(tupels.Count);
            for (int i = 0; i < tupels.Count; i++)
                elements.Add((T)tupels[i].Element);
            return elements;
        }

        /// <summary>
        /// Findet alle Spielelemente innerhalb des Sichtkreis der gegebenen Wanze.
        /// </summary>
        /// <remarks>
        /// Die Simulation legt ein Gitter mit der maximalen Sichtweite der Wanzen als
        /// Seitenlänge an und benutzt diese Methode auf dieser Instanz zum Finden von
        /// Ameisen. In dieses Gitter werden nur Ameisen einsortiert.
        /// </remarks>
        /// <param name="bug">Die Referenzwanze.</param>
        /// <returns>Eine Liste von Ameisen.</returns>
        public List<T> FindAnts(CoreBug bug)
        {
            // Speichert alle gefundenen Ameisen.
            List<T> ants = new List<T>();

            // Bestimme die Zelle in der die übergebene Wanze sich befindet.
            int col = bug.CoordinateBase.X / sideLength;
            int row = bug.CoordinateBase.Y / sideLength;

            // Betrachte die Zelle und die acht Zellen daneben.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {
                            List<T> cell = cells[col + c, row + r];
                            for (int i = 0; i < cell.Count; i++)
                            {
                                int distance = CoreCoordinate.BestimmeEntfernungI(bug.CoordinateBase, cell[i].CoordinateBase);
                                if (distance <= sideLength)
                                    ants.Add(cell[i]);
                            }
                        }

            return ants;
        }

        /// <summary>
        /// Findet die Markierung, die die gegebene Ameise noch nicht gerochen hat
        /// und die der Ameise am nächsten liegt.
        /// </summary>
        /// <remarks>
        /// Die Simulation legt ein Gitter mit dem maximalen Radius einer Markierung als
        /// Seitenlänge an und benutzt diese Methode auf dieser Instanz zum Finden von
        /// Markierungen. In dieses Gitter werden nur Markierungen einsortiert.
        /// </remarks>
        /// <param name="ant">Die Referenzameise.</param>
        /// <returns>Eine Markierung.</returns>
        public CoreMarker FindMarker(CoreAnt ant)
        {
            CoreMarker nearestMarker = null;
            int nearestMarkerDistance = int.MaxValue;

            // Bestimme die Zelle in der die übergebene Ameise sich befindet.
            int col = ant.CoordinateBase.X / sideLength;
            int row = ant.CoordinateBase.Y / sideLength;

            // Betrachte die Zelle und die acht Zellen daneben.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {
                            List<T> cell = cells[col + c, row + r];

                            // Betrachte alle Markierungen in der aktuellen Zelle.
                            for (int i = 0; i < cell.Count; i++)
                            {
                                CoreMarker marker = cell[i] as CoreMarker;
                                Debug.Assert(marker != null);

                                // Bestimme die Entfernung der Mittelpunkte und der Kreise.
                                int distance = CoreCoordinate.BestimmeEntfernungDerMittelpunkteI(ant.CoordinateBase, marker.CoordinateBase);
                                int circleDistance = distance - ant.CoordinateBase.Radius - marker.CoordinateBase.Radius;

                                // Die neue Markierung wurde noch nicht gerochen und
                                // liegt näher als die gemerkte.
                                if (circleDistance <= 0 && distance < nearestMarkerDistance &&
                                    !ant.SmelledMarker.Contains(marker))
                                {
                                    nearestMarkerDistance = distance;
                                    nearestMarker = marker;
                                }
                            }
                        }

            return nearestMarker;
        }

        /// <summary>
        /// Findet die Wanze, die feindliche Ameise und die befreundete Ameise mit der
        /// geringsten Entfernung innerhalb des Sichtkreis der gegebenen Ameise und
        /// zählt die Anzahl an Wanzen, feindlichen und befreundeten Ameisen im Sichtkreis.
        /// </summary>
        /// <remarks>
        /// Wird für Ameisen verwendet. Die Simulation legt für jeden vorkommenden Sichtradius
        /// eine eigenes Gitter an und benutzt diese Methode auf der passenden Instanz zum Finden
        /// von Insekten. Die Seitenlänge dieses Gitters ist also der Sichradius der Ameise.
        /// In diese Gitter werden Wanzen und Ameisen einsortiert.
        /// </remarks>
        /// <param name="ant">Die Referenzameise.</param>
        /// <param name="nearestBug">Eine Wanze.</param>
        /// <param name="bugCount">Die Anzahl an Wanzen.</param>
        /// <param name="nearestEnemyAnt">Eine feindliche Ameise.</param>
        /// <param name="enemyAntCount">Die Anzahl an feindlichen Ameisen.</param>
        /// <param name="nearestColonyAnt">Eine befreundete Ameise.</param>
        /// <param name="colonyAntCount">Die Anzahl an befreundeten Ameisen.</param>
        /// <param name="casteAntCount">Die Anzahl an befreundeten Ameisen der selben Kaste.</param>
        public void FindAndCountInsects(CoreAnt ant, out CoreBug nearestBug, out int bugCount,
            out CoreAnt nearestEnemyAnt, out int enemyAntCount, out CoreAnt nearestColonyAnt,
            out int colonyAntCount, out int casteAntCount, out CoreAnt nearestTeamAnt,
            out int teamAntCount)
        {
            // Die nächstliegenden gefundenen Wanzen und Ameisen.
            nearestBug = null;
            nearestEnemyAnt = null;
            nearestColonyAnt = null;
            nearestTeamAnt = null;

            // Die Entfernungen zu den nächstliegenden gefundenen Wanzen und Ameisen.
            int nearestBugDistance = int.MaxValue;
            int nearestEnemyAntDistance = int.MaxValue;
            int nearestColonyAntDistance = int.MaxValue;
            int nearestTeamAntDistance = int.MaxValue;

            // Die Anzahlen der gefundenen Wanzen und Ameisen.
            bugCount = 0;
            enemyAntCount = 0;
            colonyAntCount = 0;
            casteAntCount = 0;
            teamAntCount = 0;

            // Bestimme die Zelle in der die übergebene Ameise sich befindet.
            int col = ant.CoordinateBase.X / sideLength;
            int row = ant.CoordinateBase.Y / sideLength;

            // Betrachte die Zelle und die acht Zellen daneben.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {

                            // Betrachte alle Insekten in der aktuellen Zelle.
                            List<T> cell = cells[col + c, row + r];
                            for (int i = 0; i < cell.Count; i++)
                            {
                                CoreInsect insect = cell[i] as CoreInsect;
                                Debug.Assert(insect != null);

                                if (insect == ant)
                                    continue;

                                // Vergleiche die Entfernung zum aktuellen Insekt mit der
                                // Sichtweite der Ameise bzw. der Seitenlänge des Gitters.
                                int distance = CoreCoordinate.BestimmeEntfernungI(ant.CoordinateBase, insect.CoordinateBase);
                                if (distance > sideLength)
                                    continue;

                                // Selbes Volk. Die Abfrage "insect is CoreAnt" ist unnötig.
                                if (insect.colony == ant.colony)
                                {
                                    colonyAntCount++;
                                    if (insect.CasteIndexBase == ant.CasteIndexBase)
                                        casteAntCount++;

                                    // Die neue Ameise liegt näher als die gemerkte.
                                    if (distance < nearestColonyAntDistance)
                                    {
                                        nearestColonyAntDistance = distance;
                                        nearestColonyAnt = (CoreAnt)insect;
                                    }
                                }

                                // Selbes Team.
                                else if (insect.colony.Team == ant.colony.Team)
                                {
                                    teamAntCount++;

                                    // Die neue Ameise liegt näher als die gemerkte.
                                    if (distance < nearestTeamAntDistance)
                                    {
                                        nearestTeamAntDistance = distance;
                                        nearestTeamAnt = (CoreAnt)insect;
                                    }
                                }

                                // Wanze.
                                else if (insect is CoreBug)
                                {
                                    bugCount++;

                                    // Die neue Wanze liegt näher als die gemerkte.
                                    if (distance < nearestBugDistance)
                                    {
                                        nearestBugDistance = distance;
                                        nearestBug = (CoreBug)insect;
                                    }
                                }

                                // Feindliche Ameise.
                                else
                                {
                                    enemyAntCount++;

                                    // Die neue Ameise liegt näher als die gemerkte.
                                    if (distance < nearestEnemyAntDistance)
                                    {
                                        nearestEnemyAntDistance = distance;
                                        nearestEnemyAnt = (CoreAnt)insect;
                                    }
                                }
                            }
                        }
        }

        #endregion

    }
}