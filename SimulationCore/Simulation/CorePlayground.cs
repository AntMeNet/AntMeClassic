using System;
using System.Collections.Generic;
using System.Threading;

// (Debuging) Referenzen Hinzufuegen bei Bedarf!!!
//using System.Windows.Forms;
//using System.Drawing;


namespace AntMe.Simulation
{
    /// <summary>
    /// Playground
    /// </summary>
    /// <author>Patrick Kirsch</author>
    internal class CorePlayground
    {
        // Debuging (in SimulatorProxy.cs den UIAccess auf True setzen!!!)
        private const bool ENABLE_DEBUGINGSCREEN = false;
        private Thread debugThread;

        public readonly int Width;
        public readonly int Height;

        private readonly Random mapRandom;

        private List<Vector2D> AntHillPoints;
        private Cell[,] CellArray;
        private List<Cell> SpawnCellsList;

        private float AntHillRandomdisplacement = SimulationSettings.Default.AntHillRandomDisplacement;
        private int SpawnCellSize = SimulationSettings.Default.SpawnCellSize;
        private int RestrictedZoneRadius = SimulationSettings.Default.RestrictedZoneRadius;
        private int FarZoneRadius = SimulationSettings.Default.FarZoneRadius;
        private float DecreasValue = SimulationSettings.Default.DecreaseValue;
        private float RegenerationValue = SimulationSettings.Default.RegenerationValue;

        // public readable lists 
        public readonly List<CoreFruit> FruitsList;
        public readonly List<CoreSugar> SugarHillsList;
        public readonly List<CoreAnthill> AntHillsList;

        /// <summary>
        /// Constructor for instance of playground
        /// </summary>
        /// <param name="width">width in steps</param>
        /// <param name="height">height in steps</param>
        /// <param name="random">initial value for the random generator</param>
        /// <param name="playercount">number of players</param>
        public CorePlayground(int width, int height, Random random, int playercount)
        {
            Width = width;
            Height = height;
            mapRandom = random;

            // checking the simulation settings
            if (SimulationSettings.Custom.AntHillRandomDisplacement != 0)
                AntHillRandomdisplacement = SimulationSettings.Custom.AntHillRandomDisplacement;
            if (SimulationSettings.Custom.SpawnCellSize != 0)
                SpawnCellSize = SimulationSettings.Custom.SpawnCellSize;
            if (SimulationSettings.Custom.RestrictedZoneRadius != 0)
                RestrictedZoneRadius = SimulationSettings.Custom.RestrictedZoneRadius;
            if (SimulationSettings.Custom.FarZoneRadius != 0)
                FarZoneRadius = SimulationSettings.Custom.FarZoneRadius;
            if (SimulationSettings.Custom.DecreaseValue != 0)
                DecreasValue = SimulationSettings.Custom.DecreaseValue;
            if (SimulationSettings.Custom.RegenerationValue != 0)
                RegenerationValue = SimulationSettings.Custom.RegenerationValue;


            // Initialization of public readable lists
            FruitsList = new List<CoreFruit>();
            SugarHillsList = new List<CoreSugar>();
            AntHillsList = new List<CoreAnthill>();

            AntHillPoints = new List<Vector2D>(playercount);
            CellArray = new Cell[(int)Math.Ceiling((float)width / SpawnCellSize), (int)Math.Ceiling((float)height / SpawnCellSize)];
            SpawnCellsList = new List<Cell>(CellArray.Length);

            //if (ENABLE_DEBUGINGSCREEN)
            //{
            //    debugThread = new Thread(new ThreadStart(debug));
            //    debugThread.Start();
            //}

            #region AntHills

            // the start angle gives the direction which the first newly spawned ant is heading to
            int startAngle = mapRandom.Next(359);

            // the list of possible anthill spawning positions is populated 
            if (playercount == 1)
            {
                AntHillPoints.Add(new Vector2D(width / 2, height / 2));
            }
            else
            {

                float angle = 360f / playercount;
                int radius = Math.Min(height / 3, width / 3);

                for (int i = 0; i < playercount; i++)
                {
                    int nx = (int)(radius * Math.Cos(((angle * i) + startAngle) * Math.PI / 180d));
                    int ny = (int)(radius * Math.Sin(((angle * i) + startAngle) * Math.PI / 180d));

                    AntHillPoints.Add(new Vector2D(nx + (width / 2), ny + (height / 2)));
                }
            }

            // spawning positions for the players
            for (int i = 0; i < playercount; i++)
            {
                int attempts = 5;
                Again:
                Vector2D punkt = new Vector2D(mapRandom.Next(width), mapRandom.Next(height));
                Vector2D targetPoint = new Vector2D((int)((punkt.X * AntHillRandomdisplacement) + (AntHillPoints[i].X * (1 - AntHillRandomdisplacement))), (int)(((punkt.Y * AntHillRandomdisplacement) + (AntHillPoints[i].Y * (1 - AntHillRandomdisplacement)))));

                for (int p = 0; p < i; p++)
                {
                    if ((AntHillPoints[p] - targetPoint).GetLenght() < RestrictedZoneRadius && attempts > 0)
                    {
                        attempts--;
                        goto Again;
                    }
                }

                AntHillPoints[i] = targetPoint;
            }
            #endregion

            #region Cells



            for (int cellX = 0; cellX < CellArray.GetLength(0); cellX++)
            {
                for (int cellY = 0; cellY < CellArray.GetLength(1); cellY++)
                {
                    // position of the spawn cells upper left corner
                    Vector2D pos = new Vector2D(cellX * SpawnCellSize, cellY * SpawnCellSize);

                    // spawn cells touching the edge must shrink
                    int cellWidth = Math.Min(SpawnCellSize, Width - (cellX * SpawnCellSize));
                    int cellHeight = Math.Min(SpawnCellSize, Height - (cellY * SpawnCellSize));

                    // instantiate a new cell
                    Cell cell = CellArray[cellX, cellY] = new Cell(pos, cellWidth, cellHeight);


                    Vector2D totalDisplacementVector = new Vector2D();

                    foreach (Vector2D antHill in AntHillPoints)
                    {
                        // determination of the cells center point
                        int cellMidX = (cellX * SpawnCellSize) + (cellWidth / 2);
                        int cellMidY = (cellY * SpawnCellSize) + (cellHeight / 2);

                        // calculation of the displacement vector
                        Vector2D displacmentVector = antHill - cell.Position;

                        int distance = (int)displacmentVector.GetLenght();

                        if (distance < RestrictedZoneRadius)
                        {
                            // spawn cell is within one or several anthills restricted zone
                            totalDisplacementVector = new Vector2D(0, 0);
                            break;
                        }

                        // adding up displacement vectors
                        totalDisplacementVector += displacmentVector;
                    }

                    // 
                    // calculation of the average displacement vector value
                    totalDisplacementVector /= playercount;

                    // restrict cell if within restricted zone or at the edge of the playground
                    if (totalDisplacementVector.GetLenght() < RestrictedZoneRadius || totalDisplacementVector.GetLenght() > FarZoneRadius || cellX == 0 || cellY == 0 || cellX == CellArray.GetLength(0) - 1 || cellY == CellArray.GetLength(1) - 1)
                    {
                        cell.restricted = true;
                    }
                }
            }

            // unrestricted cells populate the spawn list
            foreach (Cell cell in CellArray)
            {
                if (!cell.restricted)
                    SpawnCellsList.Add(cell);
            }

            #endregion

        }

        /// <summary>
        /// find a random cell with high spawn value
        /// </summary>
        /// <returns>returns the cell for the food to spawn</returns>
        private Cell FindFoodSpawnCell()
        {
            RegenerateCellValues();

            // sort cell spawn list descending by spawn value 
            SpawnCellsList.Sort((x, y) => y.SpawnValue.CompareTo(x.SpawnValue));

            // RandomList of cells with hightest and those -0.1 below highest spawn value
            List<Cell> RandomList = SpawnCellsList.FindAll((x) => (x.SpawnValue >= SpawnCellsList[0].SpawnValue - 0.1f) && (x.SpawnedFood == null));

            // if RandomList is empty and there are no unused spawn cells, the list will be populated with already used spawn cells
            if (RandomList.Count == 0)
                RandomList = SpawnCellsList.FindAll((x) => (x.SpawnValue >= SpawnCellsList[0].SpawnValue - 0.1f));

            // random cell for spawning is chosen
            Cell cell = RandomList[mapRandom.Next(RandomList.Count - 1)];

            cell.SpawnValue = 0f;

            DecreaseCells(cell);

            return cell;
        }

        /// <summary>
        /// regenerate values of all cells on the playground
        /// </summary>
        private void RegenerateCellValues()
        {
            foreach (Cell cell in CellArray)
            {
                if (cell.SpawnedFood != null)
                    continue;

                cell.SpawnValue += RegenerationValue;
                if (cell.SpawnValue > 1f)
                    cell.SpawnValue = 1f;
            }
        }

        /// <summary>
        /// Reduce the spawn value of all cells depending an the distance from the start cell
        /// </summary>
        /// <param name="startCell">start cell</param>
        private void DecreaseCells(Cell startCell)
        {
            foreach (Cell cell in CellArray)
            {
                if (cell == startCell)
                    continue;

                float distance = (cell.Position - startCell.Position).GetLenght() / SpawnCellSize;

                cell.SpawnValue -= DecreasValue / (distance * distance);

                if (cell.SpawnValue < 0)
                    cell.SpawnValue = 0;
            }
        }



        /// <summary>
        /// Create a new sugar hill
        /// </summary>
        public void NewSugar()
        {
            Cell cell = FindFoodSpawnCell();
            int value = mapRandom.Next(SimulationSettings.Custom.SugarAmountMinimum, SimulationSettings.Custom.SugarAmountMaximum);
            Vector2D vector2D = cell.Position + new Vector2D(mapRandom.Next(cell.Width), mapRandom.Next(cell.Height));
            CoreSugar sugar = new CoreSugar(vector2D.X, vector2D.Y, value);
            SugarHillsList.Add(sugar);
            cell.SpawnedFood = sugar;
        }

        /// <summary>
        /// Remove sugar and spawn cell from the corresponding lists
        /// SugarHillList and SpawnCellsList
        /// </summary>
        /// <param name="sugar">sugar to be removed</param>
        public void RemoveSugar(CoreSugar sugar)
        {
            SugarHillsList.Remove(sugar);
            Cell cell = SpawnCellsList.Find((x) => x.SpawnedFood == sugar);
            cell.SpawnedFood = null;
        }

        /// <summary>
        /// create a new piece of fruit
        /// </summary>
        public void NewFruit()
        {
            Cell cell = FindFoodSpawnCell();
            int value = mapRandom.Next(SimulationSettings.Custom.FruitAmountMinimum, SimulationSettings.Custom.FruitAmountMaximum);
            Vector2D vector2D = cell.Position + new Vector2D(mapRandom.Next(cell.Width), mapRandom.Next(cell.Height));
            CoreFruit Fruit = new CoreFruit(vector2D.X, vector2D.Y, value);
            FruitsList.Add(Fruit);
            cell.SpawnedFood = Fruit;
        }

        /// <summary>
        /// Removes the fruits from the corresponding lists
        /// FruitsList and SpawnCellsList
        /// </summary>
        /// <param name="fruit">fruit to be removed</param>
        public void RemoveFruit(CoreFruit fruit)
        {
            FruitsList.Remove(fruit);
            Cell cell = SpawnCellsList.Find((x) => x.SpawnedFood == fruit);
            cell.SpawnedFood = null;
        }

        /// <summary>
        /// create a new anthill
        /// </summary>
        /// <param name="colony">ID of the colony</param>
        /// <returns>new anthill</returns>
        public CoreAnthill NewAnthill(int colony)
        {
            Vector2D vector2D = AntHillPoints[mapRandom.Next(AntHillPoints.Count - 1)];
            AntHillPoints.Remove(vector2D);
            CoreAnthill anthill = new CoreAnthill(vector2D.X, vector2D.Y, SimulationSettings.Custom.AntHillRadius, colony);
            AntHillsList.Add(anthill);
            return anthill;
        }

        #region Vector2D and Cells


        /// <summary>
        /// two-dimensional vector in two-dimensional space
        /// </summary>
        internal struct Vector2D
        {

            public int X;
            public int Y;

            /// <summary>
            /// Constructor of two-dimensional vector instance
            /// </summary>
            /// <param name="x">X-Coordinate of vector</param>
            /// <param name="y">Y-Coordinate of vector</param>
            public Vector2D(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Calculate length of vector
            /// </summary>
            /// <returns>length of vector as float</returns>
            public float GetLenght()
            {
                return (float)Math.Sqrt((float)((X * X) + (Y * Y)));
            }

            public static Vector2D operator +(Vector2D V1, Vector2D V2)
            {
                return new Vector2D(V1.X + V2.X, V1.Y + V2.Y);
            }

            public static Vector2D operator -(Vector2D V1, Vector2D V2)
            {
                return new Vector2D(V1.X - V2.X, V1.Y - V2.Y);
            }

            public static Vector2D operator /(Vector2D V1, int x)
            {
                return new Vector2D(V1.X / x, V1.Y / x);
            }

        }

        internal class Cell
        {

            public readonly Vector2D Position;
            public readonly int Width;
            public readonly int Height;

            public CoreFood SpawnedFood;
            public bool restricted = false;
            public float SpawnValue = 1f;

            /// <summary>
            /// Constructor of a cell instance
            /// </summary>
            /// <param name="position">two-dimensional position of the cells upper left corner</param>
            /// <param name="width">width of the cell</param>
            /// <param name="height">height of the cell</param>
            public Cell(Vector2D position, int width, int height)
            {
                this.Position = position;
                this.Width = width;
                this.Height = height;
            }
        }
        #endregion

        //#region Debugin

        //private void debug()
        //{
        //    DebugViewer DebugForm = new DebugViewer(CellArray);

        //    Application.Run(DebugForm);
        //    DebugForm.Show();
        //}

        //private class DebugViewer : Form
        //{

        //    private ViewCell[,] ViewCells;
        //    private System.Windows.Forms.Timer updateTimer;

        //    public DebugViewer(Cell[,] cells)
        //    {
        //        this.Text = "DebugViewer - Spawnzellen";

        //        ViewCells = new ViewCell[cells.GetLength(0), cells.GetLength(1)];
        //        updateTimer = new System.Windows.Forms.Timer();


        //        for (int x = 0; x < cells.GetLength(0); x++)
        //        {
        //            for (int y = 0; y < cells.GetLength(1); y++)
        //            {
        //                ViewCells[x, y] = new ViewCell(cells[x, y]);
        //                ViewCells[x, y].Location = new Point(x * 52, y * 52);
        //                this.Controls.Add(ViewCells[x, y]);
        //            }
        //        }

        //        updateTimer.Enabled = true;
        //        updateTimer.Start();
        //        updateTimer.Tick += updateTimer_Tick;

        //    }

        //    private void updateTimer_Tick(object sender, EventArgs e)
        //    {
        //        foreach (ViewCell viewCell in ViewCells)
        //        {
        //            viewCell.update();
        //        }
        //        this.Refresh();
        //    }
        //}

        //private class ViewCell : UserControl
        //{
        //    private CorePlayground.Cell cellData;
        //    private Label label_SpawnValue;

        //    public ViewCell(CorePlayground.Cell cell)
        //    {
        //        InitializeComponent();
        //        this.cellData = cell;
        //    }

        //    public void update()
        //    {
        //        if (cellData == null)
        //        {
        //            label_SpawnValue.Text = "Null";
        //            this.BackColor = Color.White;

        //        }
        //        else
        //        {
        //            if (cellData.restricted)
        //                this.BackColor = Color.Red;
        //            else if (cellData.SpawnedFood != null)
        //                this.BackColor = Color.OrangeRed;
        //            else
        //                this.BackColor = Color.LightGreen;

        //            label_SpawnValue.Text = cellData.SpawnValue.ToString("0.000");
        //        }
        //    }

        //    private void InitializeComponent()
        //    {
        //        this.label_SpawnValue = new System.Windows.Forms.Label();
        //        this.SuspendLayout();
        //        // 
        //        // label_SpawnValue
        //        // 
        //        this.label_SpawnValue.Dock = System.Windows.Forms.DockStyle.Fill;
        //        this.label_SpawnValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        this.label_SpawnValue.Location = new System.Drawing.Point(0, 0);
        //        this.label_SpawnValue.Name = "label_SpawnValue";
        //        this.label_SpawnValue.Size = new System.Drawing.Size(50, 50);
        //        this.label_SpawnValue.TabIndex = 0;
        //        this.label_SpawnValue.Text = "SpawnValue";
        //        this.label_SpawnValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //        // 
        //        // ViewCell
        //        // 
        //        this.Controls.Add(this.label_SpawnValue);
        //        this.Name = "ViewCell";
        //        this.Size = new System.Drawing.Size(50, 50);
        //        this.ResumeLayout(false);

        //    }
        //}

        //#endregion

    }
}