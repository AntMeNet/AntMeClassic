using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace AntMe.Simulation
{

    /// <summary>
    /// Implements a cell grid method for quickly finding game elements on the playground.
    /// </summary>
    /// <remarks>
    /// A game element is sorted into exactly one cell regardless of its radius.
    /// Therefore, game elements "two cells further" will not be found even if
    /// they are actually close enough to the reference element (due to their radius).
    /// </remarks>
    /// <typeparam name="T">Game element type.</typeparam>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal class Grid<T> : ICollection<T>
        where T : ICoordinate
    {

        #region static part, fabric method

        #region structur GridSize

        /// <summary>
        /// Saves the dimensions of a grid.
        /// </summary>
        private struct GridSize
        {
            /// <summary>
            /// The width of the grid.
            /// </summary>
            public readonly int Width;

            /// <summary>
            /// The height of the grid.
            /// </summary>
            public readonly int Height;

            /// <summary>
            /// The side length of a grid cell.
            /// </summary>
            public readonly int SideLength;

            /// <summary>
            /// Creates a new instance of the structure GridSize
            /// </summary>
            /// <param name="width">The width of the grid.</param>
            /// <param name="height">The height of the grid.</param>
            /// <param name="sideLength">The side length of a grid cell.</param>
            public GridSize(int width, int height, int sideLength)
            {
                Width = width;
                Height = height;
                SideLength = sideLength;
            }
        }

        #endregion

        // List of all already created grids.
        private static readonly Dictionary<GridSize, Grid<T>> grids =
            new Dictionary<GridSize, Grid<T>>();

        /// <summary>
        /// Creates a grid with the specified dimensions or returns an existing grid with this dimensions.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="sideLength">The side length of a grid cell.
        /// Determines the Maximum distance up to which a game element can see another one.</param>
        public static Grid<T> Create(int width, int height, int sideLength)
        {
            GridSize size = new GridSize(width, height, sideLength);
            if (!grids.ContainsKey(size))
                grids.Add(size, new Grid<T>(size));
            return grids[size];
        }

        #endregion

        // The dimensions of the grid.
        private readonly int columns;
        private readonly int rows;
        private readonly int sideLength;

        // The individual grid cells.
        private readonly List<T>[,] cells;

        #region constructors

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="size">The dimensions of the grid.</param>
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
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="sideLength">The side length of a grid cell.
        /// Determines the Maximum distance up to which a game element can see another one.</param>
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

        #region Interface IEnumerable

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

        #region Interface ICollection

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
        /// Adds multiple game elements to the grid.
        /// </summary>
        /// <param name="elements">List of game elements.</param>
        public void Add(List<T> elements)
        {
            for (int i = 0; i < elements.Count; i++)
                Add(elements[i]);
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
        /// Removes multiple elements from the grid.
        /// </summary>
        /// <param name="elements">List of game elements.</param>
        public void Remove(List<T> elements)
        {
            for (int i = 0; i < elements.Count; i++)
                Remove(elements[i]);
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

        #region Find game element

        #region Tuple and TupleComparer classes, sorting game elements by distance

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
        /// Finds all game elements within the given visual circle of the given game element.
        /// </summary>
        /// <param name="coordinate">Reference game element.</param>
        /// <param name="maximumDistance">The maximum distance.</param>
        /// <returns>A list of game elements sorted by distance.</returns>
        public List<T> FindSorted(ICoordinate coordinate, int maximumDistance)
        {
            // Saves all found tuples (game element, distance).
            List<Tupel> tupels = new List<Tupel>();

            // Determine the cell in which the given game element is located.
            int col = coordinate.CoordinateBase.X / sideLength;
            int row = coordinate.CoordinateBase.Y / sideLength;

            // Consider the cell and the eight cells next to it.
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

                                int distance = CoreCoordinate.DetermineDistanceI(coordinate.CoordinateBase, cell[i].CoordinateBase);
                                if (distance <= maximumDistance)
                                    tupels.Add(new Tupel(cell[i], distance));
                            }
                        }

            // Sort the tuples and return the game elements.
            tupels.Sort(comparer);
            List<T> elements = new List<T>(tupels.Count);
            for (int i = 0; i < tupels.Count; i++)
                elements.Add((T)tupels[i].Element);
            return elements;
        }

        /// <summary>
        /// Finds all game elements within the given bug's field of view.
        /// </summary>
        /// <remarks>
        /// The simulation creates a grid with the maximum visibility of the bugs
        /// as side length and uses this method on this instance to find ants.
        /// Only ants are sorted into this grid.
        /// </remarks>
        /// <param name="bug">Reference bug.</param>
        /// <returns>List of ants.</returns>
        public List<T> FindAnts(CoreBug bug)
        {
            // Saves all found ants.
            List<T> ants = new List<T>();

            // Determine the cell in which the given bug is located.
            int col = bug.CoordinateBase.X / sideLength;
            int row = bug.CoordinateBase.Y / sideLength;

            // Consider the cell and the eight cells next to it.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {
                            List<T> cell = cells[col + c, row + r];
                            for (int i = 0; i < cell.Count; i++)
                            {
                                int distance = CoreCoordinate.DetermineDistanceI(bug.CoordinateBase, cell[i].CoordinateBase);
                                if (distance <= sideLength)
                                    ants.Add(cell[i]);
                            }
                        }

            return ants;
        }

        /// <summary>
        /// Finds the marker that the given ant has not smelled yet. and which is closest to the ant.
        /// </summary>
        /// <remarks>
        /// The simulation creates a grid with the maximum radius of a marker as the side length
        /// and uses this method on this instance to find markers. Only marks are sorted into this grid.
        /// </remarks>
        /// <param name="ant">Reference ant.</param>
        /// <returns>Marker.</returns>
        public CoreMarker FindMarker(CoreAnt ant)
        {
            CoreMarker nearestMarker = null;
            int nearestMarkerDistance = int.MaxValue;

            // Determine the cell in which the given ant is located.
            int col = ant.CoordinateBase.X / sideLength;
            int row = ant.CoordinateBase.Y / sideLength;

            // Consider the cell and the eight cells next to it.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {
                            List<T> cell = cells[col + c, row + r];

                            // Consider all markers in the current cell.
                            for (int i = 0; i < cell.Count; i++)
                            {
                                CoreMarker marker = cell[i] as CoreMarker;
                                Debug.Assert(marker != null);

                                // Determine the distance between the centers and the circles.
                                int distance = CoreCoordinate.DetermineDistanceToCenter(ant.CoordinateBase, marker.CoordinateBase);
                                int circleDistance = distance - ant.CoordinateBase.Radius - marker.CoordinateBase.Radius;

                                // The new marker has not yet been smelled and is closer than the remembered one.
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
        /// Finds the bug, enemy ant and friendly ant with the smallest distance within the given
        /// ant's field of view and counts the number of bugs, enemy ants and friendly ants in the
        /// field of view.
        /// </summary>
        /// <remarks>
        /// Used for ants. The simulation creates a separate grid for each sight radius and uses
        /// this method on the appropriate instance to find insects. The side length of this grid
        /// is the radius of view of the ant. Bugs and ants are sorted into these grids.
        /// </remarks>
        /// <param name="ant">reference ant.</param>
        /// <param name="nearestBug">nearest bug.</param>
        /// <param name="bugCount">bug counter.</param>
        /// <param name="nearestEnemyAnt">nearest enemy ant.</param>
        /// <param name="enemyAntCount">enemy ant counter.</param>
        /// <param name="nearestColonyAnt">nearest colony ant.</param>
        /// <param name="colonyAntCount">colony ant counter.</param>
        /// <param name="casteAntCount">The number of friendly ants of the same caste.</param>
        public void FindAndCountInsects(CoreAnt ant, out CoreBug nearestBug, out int bugCount,
            out CoreAnt nearestEnemyAnt, out int enemyAntCount, out CoreAnt nearestColonyAnt,
            out int colonyAntCount, out int casteAntCount, out CoreAnt nearestTeamAnt,
            out int teamAntCount)
        {
            // The nearest found bugs and ants.
            nearestBug = null;
            nearestEnemyAnt = null;
            nearestColonyAnt = null;
            nearestTeamAnt = null;

            // The distances to the nearest bugs and ants found.
            int nearestBugDistance = int.MaxValue;
            int nearestEnemyAntDistance = int.MaxValue;
            int nearestColonyAntDistance = int.MaxValue;
            int nearestTeamAntDistance = int.MaxValue;

            // The numbers of bugs and ants found.
            bugCount = 0;
            enemyAntCount = 0;
            colonyAntCount = 0;
            casteAntCount = 0;
            teamAntCount = 0;

            // Determine the cell in which the given ant is located.
            int col = ant.CoordinateBase.X / sideLength;
            int row = ant.CoordinateBase.Y / sideLength;

            // Consider the cell and the eight cells next to it.
            for (int c = -1; c <= 1; c++)
                if (col + c >= 0 && col + c < columns)
                    for (int r = -1; r <= 1; r++)
                        if (row + r >= 0 && row + r < rows)
                        {

                            // Consider all insects in the current cell.
                            List<T> cell = cells[col + c, row + r];
                            for (int i = 0; i < cell.Count; i++)
                            {
                                CoreInsect insect = cell[i] as CoreInsect;
                                Debug.Assert(insect != null);

                                if (insect == ant)
                                    continue;

                                // Compare the distance to the current insect with the ant's visibility
                                // or the side length of the grid.
                                int distance = CoreCoordinate.DetermineDistanceI(ant.CoordinateBase, insect.CoordinateBase);
                                if (distance > sideLength)
                                    continue;

                                // Same colony. The query "insect is CoreAnt" is unnecessary.
                                if (insect.colony == ant.colony)
                                {
                                    colonyAntCount++;
                                    if (insect.CasteIndexBase == ant.CasteIndexBase)
                                        casteAntCount++;

                                    // The new ant is closer than the remembered one.
                                    if (distance < nearestColonyAntDistance)
                                    {
                                        nearestColonyAntDistance = distance;
                                        nearestColonyAnt = (CoreAnt)insect;
                                    }
                                }

                                // Same team.
                                else if (insect.colony.Team == ant.colony.Team)
                                {
                                    teamAntCount++;

                                    // The new ant is closer than the remembered one.
                                    if (distance < nearestTeamAntDistance)
                                    {
                                        nearestTeamAntDistance = distance;
                                        nearestTeamAnt = (CoreAnt)insect;
                                    }
                                }

                                // Bug.
                                else if (insect is CoreBug)
                                {
                                    bugCount++;

                                    // The new bug is closer than the remembered one.
                                    if (distance < nearestBugDistance)
                                    {
                                        nearestBugDistance = distance;
                                        nearestBug = (CoreBug)insect;
                                    }
                                }

                                // Enemy ant.
                                else
                                {
                                    enemyAntCount++;

                                    // The new ant is closer than the remembered one.
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