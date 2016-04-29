using System;
using System.Collections.Generic;
using AntMe.Deutsch;
using System.Threading;

// (Debuging) Referenzen Hinzufügen bei Bedarf!!!
//using System.Windows.Forms;
//using System.Drawing;


namespace AntMe.Simulation
{
    /// <summary>
    /// Das Spielfeld.
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
        private List<Cell> CellSpawnList;

        private float AntHillRandomdisplacement = SimulationSettings.Default.AntHillRandomDisplacement;
        private int SpawnCellSize= SimulationSettings.Default.SpawnCellSize;
        private int RestrictedZoneRadius = SimulationSettings.Default.RestrictedZoneRadius;
        private int FarZoneRadius= SimulationSettings.Default.FarZoneRadius;
        private float DecreasValue = SimulationSettings.Default.DecreaseValue;
        private float RegenerationValue = SimulationSettings.Default.RegenerationValue;

        // öffentliche Bestandslisten(werden von Außen gelesen)
        public readonly List<CoreFruit> Fruits;
        public readonly List<CoreSugar> SugarHills;
        public readonly List<CoreAnthill> AntHills;

        /// <summary>
        /// Erzeugt eine neue Instanz der Spielfeld-Klasse.
        /// </summary>
        /// <param name="width">Die Breite in Schritten.</param>
        /// <param name="height">Die Höhe in Schritten.</param>
        /// <param name="random">Initialwert für Zufallsgenerator</param>
        /// /// <param name="playercount">Anzahl der Spieler</param>
        public CorePlayground(int width, int height, Random random, int playercount)
        {
            Width = width;
            Height = height;
            mapRandom = random;

            //Überprüfen der Settings
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


            // Initialisierungen
            Fruits = new List<CoreFruit>();
            SugarHills = new List<CoreSugar>();
            AntHills = new List<CoreAnthill>();

            AntHillPoints = new List<Vector2D>(playercount);
            CellArray = new Cell[(int)Math.Ceiling((float)width / SpawnCellSize), (int)Math.Ceiling((float)height / SpawnCellSize)];
            CellSpawnList = new List<Cell>(CellArray.Length);

            //if (ENABLE_DEBUGINGSCREEN)
            //{
            //    debugThread = new Thread(new ThreadStart(debug));
            //    debugThread.Start();
            //}

            #region AntHills

            // Bestimme zufälligen Rotationswinkel des Spawnkreises.
            int startAngle = mapRandom.Next(359);

            // Fülle die Liste der möglichen Spawnpositionen.
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

            // Weise jedem Spieler eine zufällige Spawnposition zu.
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
                    // Position der oberen linken Ecke der Zelle
                    Vector2D pos = new Vector2D(cellX * SpawnCellSize, cellY * SpawnCellSize);

                    // Ermittlung ob die Zelle kleiner sein muss, da sie an den Rand stößt.
                    int cellWidth = Math.Min(SpawnCellSize, Width - (cellX * SpawnCellSize));
                    int cellHeight = Math.Min(SpawnCellSize, Height - (cellY * SpawnCellSize));

                    // Neue Zelle erstellen.
                    Cell cell = CellArray[cellX, cellY] = new Cell(pos, cellWidth, cellHeight);


                    Vector2D totalDisplacementVector = new Vector2D();

                    foreach (Vector2D antHill in AntHillPoints)
                    {
                        // Ermittlung des Zell Mittelpunktes.
                        int cellMidX = (cellX * SpawnCellSize) + (cellWidth / 2);
                        int cellMidY = (cellY * SpawnCellSize) + (cellHeight / 2);

                        // Berechnung des Abstands Vectors.
                        Vector2D displacmentVector = antHill - cell.Position;

                        int distance = (int)displacmentVector.GetLenght();

                        if (distance < RestrictedZoneRadius)
                        {
                            // Zelle liegt im  gesperrten Bereich mindestens eines Ameisenhügels.
                            totalDisplacementVector = new Vector2D(0, 0);
                            break;
                        }

                        // Abstands Vectoren aufaddieren.
                        totalDisplacementVector += displacmentVector;
                    }

                    // Durchschnittswert berechnen.
                    totalDisplacementVector /= playercount;

                    // Sperrung der Zelle, wenn sie in einem gesperrten Bereich sich befindet, oder sich am Spielfeldrand befindet.
                    if (totalDisplacementVector.GetLenght() < RestrictedZoneRadius || totalDisplacementVector.GetLenght() > FarZoneRadius || cellX == 0 || cellY == 0 || cellX == CellArray.GetLength(0) - 1 || cellY == CellArray.GetLength(1) - 1)
                    {
                        cell.restricted = true;
                    }
                }
            }

            // Nicht gesperrte Zellen der Spawnliste hinzufügen.
            foreach (Cell cell in CellArray)
            {
                if (!cell.restricted)
                    CellSpawnList.Add(cell);
            }

            #endregion

        }

        /// <summary>
        /// Wählt eine zufällige Zelle mit einem hohen Spawnwert aus.
        /// </summary>
        /// <returns>Gibt die Zielzelle zurück.</returns>
        private Cell findeRohstoffZelle()
        {
            RegeneriereZellen();

            // Sortiere die Spawnliste absteigend nach dem SpawnValue.
            CellSpawnList.Sort((x, y) => y.SpawnValue.CompareTo(x.SpawnValue));

            // Wählt alle Zellen aus, welche dem "höchseten Spawnvalue - 0,1" entsprechen.
            List<Cell> RandomList = CellSpawnList.FindAll((x) => (x.SpawnValue >= CellSpawnList[0].SpawnValue - 0.1f) && (x.SpawnedFood == null));

            //Sollte es keine Zelle geben, wir auch in schon bespawnten Zellen gesucht
            if (RandomList.Count == 0)
                RandomList = CellSpawnList.FindAll((x) => (x.SpawnValue >= CellSpawnList[0].SpawnValue - 0.1f));

            // Wählt ein zufällige Zelle aus dem Bereich aus.
            Cell cell = RandomList[mapRandom.Next(RandomList.Count - 1)];

            cell.SpawnValue = 0f;

            DecreaseCells(cell);

            return cell;
        }

        /// <summary>
        /// Regeneriert alle Zellen auf dem Spielfeld
        /// </summary>
        private void RegeneriereZellen()
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
        /// Verringert den Spawnwert aller Zellen, auf basis der entfernugn von der AusgangsZelle. 
        /// </summary>
        /// <param name="startCell">Die Ausgangszelle.</param>
        private void DecreaseCells(Cell startCell)
        {
            foreach (Cell cell in CellArray)
            {
                if (cell == startCell)
                    continue;

                float abstand = (cell.Position - startCell.Position).GetLenght() / SpawnCellSize;

                cell.SpawnValue -= DecreasValue / (abstand * abstand);

                if (cell.SpawnValue < 0)
                    cell.SpawnValue = 0;
            }
        }



        /// <summary>
        /// Erzeugt einen neuen Zuckerhaufen.
        /// </summary>
        public void NeuerZucker()
        {
            Cell cell = findeRohstoffZelle();
            int wert = mapRandom.Next(SimulationSettings.Custom.SugarAmountMinimum, SimulationSettings.Custom.SugarAmountMaximum);
            Vector2D punkt = cell.Position + new Vector2D(mapRandom.Next(cell.Width), mapRandom.Next(cell.Height));
            CoreSugar zucker = new CoreSugar(punkt.X, punkt.Y, wert);
            SugarHills.Add(zucker);
            cell.SpawnedFood = zucker;
        }

        /// <summary>
        /// Entfernt Zucker und gibt die Zelle wieder frei.
        /// </summary>
        /// <param name="nahrung">Den zu entfernenden Zucker</param>
        public void EntferneZucker(CoreSugar sugar)
        {
            SugarHills.Remove(sugar);
            Cell cell = CellSpawnList.Find((x) => x.SpawnedFood == sugar);
            cell.SpawnedFood = null;
        }

        /// <summary>
        /// Erzeugt ein neues Obsttück.
        /// </summary>
        public void NeuesObst()
        {
            Cell cell = findeRohstoffZelle();
            int wert = mapRandom.Next(SimulationSettings.Custom.FruitAmountMinimum, SimulationSettings.Custom.FruitAmountMaximum);
            Vector2D punkt = cell.Position + new Vector2D(mapRandom.Next(cell.Width), mapRandom.Next(cell.Height));
            CoreFruit Fruit = new CoreFruit(punkt.X, punkt.Y, wert);
            Fruits.Add(Fruit);
            cell.SpawnedFood = Fruit;
        }

        /// <summary>
        /// Entfernt Obst und gibt die Zelle wieder frei.
        /// </summary>
        /// <param name="nahrung">Das zu entfernenden Obst</param>
        public void EntferneObst(CoreFruit fruit)
        {
            Fruits.Remove(fruit);
            Cell cell = CellSpawnList.Find((x) => x.SpawnedFood == fruit);
            cell.SpawnedFood = null;
        }

        /// <summary>
        /// Erzeugt einen neuen Bau.
        /// </summary>
        /// <param name="colony">ID der Kolonie</param>
        /// <returns>Der neue Bau.</returns>
        public CoreAnthill NeuerBau(int colony)
        {
            Vector2D punkt = AntHillPoints[mapRandom.Next(AntHillPoints.Count - 1)];
            AntHillPoints.Remove(punkt);
            CoreAnthill bau = new CoreAnthill(punkt.X, punkt.Y, SimulationSettings.Custom.AntHillRadius, colony);
            AntHills.Add(bau);
            return bau;
        }

        #region "Vector2D und Cells"


        /// <summary>
        /// Vector im 2D Raum
        /// </summary>
        internal struct Vector2D
        {

            public int X;
            public int Y;

            /// <summary>
            /// Erzeugt einen neuen 2D Vector.
            /// </summary>
            /// <param name="x">X-Koordinate des Vectors</param>
            /// <param name="y">Y-Koordinate des Vectors</param>
            public Vector2D(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Berechnet die Länge des Vectors.
            /// </summary>
            /// <returns>Gibt dei Länge des Vectors als Float zurück.</returns>
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
            /// Erstellt eine Neue Zelle.
            /// </summary>
            /// <param name="position">Die Position der oberen linken Ecke der Zelle.</param>
            /// <param name="width">Die Breite der Zelle</param>
            /// <param name="height">Die Höhe der Zelle</param>
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