using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// the coreColony holds all information about the colony
    /// during the entire simulation
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreColony
    {
        // the nextId is the ID of the next colony created
        private static int nextId = 0;
        private readonly int[] antsInCaste;
        internal readonly List<CoreAnthill> AntHills = new List<CoreAnthill>();
        internal readonly CoreTeam Team;
        internal readonly List<CoreInsect> BeatenInsects = new List<CoreInsect>();
        internal readonly List<CoreInsect> EatenInsects = new List<CoreInsect>();

        /// <summary>
        /// the ID will be the identifier for the colony in the simulation
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// the GUID will be the identifier for the colony in the simulation
        /// </summary>
        public Guid Guid { get; private set; }

        internal readonly List<CoreInsect> InsectsList = new List<CoreInsect>();
        private readonly Type pClass;
        internal Grid<CoreMarker> Marker { get; private set; }
        internal readonly List<CoreMarker> NewMarker = new List<CoreMarker>();

        /// <summary>
        /// the player responisible for the performance of the colony
        /// </summary>
        internal readonly PlayerInfo Player;

        internal readonly List<CoreInsect> StarvedInsects = new List<CoreInsect>();

        internal int WidthI;
        internal int WidthI2;
        internal int HeightI;
        internal int HeightI2;
        internal CorePlayground Playground;

        internal PlayerStatistics Statistic = new PlayerStatistics();
        internal PlayerStatistics StatisticMeanAverage = new PlayerStatistics();

        /// <summary>
        /// delay for the creation of a new insect in number of rounds
        /// </summary>
        internal int insectDelay = 0;

        internal int insectCountDown;

        #region Capabilities

        /// <summary>
        /// attack value of all castes of the colony
        /// </summary>
        internal readonly int[] AttackI;

        /// <summary>
        /// grid for the different visibility ranges
        /// </summary>
        internal readonly Grid<CoreInsect>[] Grids;

        /// <summary>
        /// rotation speed of all castes of the colony
        /// in degrees per round
        /// </summary>
        internal readonly int[] RotationSpeedI;

        /// <summary>
        /// energy or health points of all castes of the colony
        /// </summary>
        internal readonly int[] EnergyI;

        /// <summary>
        /// movement speed of all castes of the colony
        /// in internal unit
        /// </summary>
        internal readonly int[] SpeedI;

        /// <summary>
        /// maximum load value of all castes of the colony
        /// </summary>
        internal readonly int[] LoadI;

        /// <summary>
        /// movement range of all castes of the colony
        /// in internal unit
        /// </summary>
        internal readonly int[] RangeI;

        /// <summary>
        /// view range of all castes of the colony
        /// in internal unit
        /// Die Sichtweiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] ViewRangeI;

        #endregion

        /// <summary>
        /// Bug constructor for a new instance of the colony class
        /// this is not an ant class but for the bugs
        /// </summary>
        /// <param name="playground">the playground</param>
        internal CoreColony(CorePlayground playground)
        {
            InitPlayground(playground);

            // the bugs will not under control of a player
            Player = null;

            // the class is CoreBug
            pClass = typeof(CoreBug);

            // TODO: check values of bugs
            SpeedI = new int[1] { SimulationSettings.Custom.BugSpeed * SimulationEnvironment.PLAYGROUND_UNIT };
            RotationSpeedI = new int[1] { SimulationSettings.Custom.BugRotationSpeed };
            RangeI = new int[1] { int.MaxValue };
            ViewRangeI = new int[1] { 0 };
            LoadI = new int[1] { 0 };
            EnergyI = new int[1] { SimulationSettings.Custom.BugEnergy };
            AttackI = new int[1] { SimulationSettings.Custom.BugAttack };

            Grids = new Grid<CoreInsect>[1];
            Grids[0] =
                Grid<CoreInsect>.Create
                    (
                    playground.Width * SimulationEnvironment.PLAYGROUND_UNIT,
                    playground.Height * SimulationEnvironment.PLAYGROUND_UNIT,
                    SimulationSettings.Custom.BattleRange * SimulationEnvironment.PLAYGROUND_UNIT);

            antsInCaste = new int[1];
        }

        /// <summary>
        /// Constructor of a new instance of a ant colony
        /// </summary>
        /// <param name="playground">the playground</param>
        /// <param name="player">player is owner of the colony</param>
        /// <param name="team">team the player belongs to</param>
        internal CoreColony(CorePlayground playground, PlayerInfo player, CoreTeam team)
        {
            InitPlayground(playground);
            Team = team;

            Player = player;
            pClass = player.assembly.GetType(player.ClassName, true, false);

            // if there is no defintion for the caste, create a new base ant caste
            if (player.Castes.Count == 0)
            {
                player.Castes.Add(new CasteInfo());
            }

            SpeedI = new int[player.Castes.Count];
            RotationSpeedI = new int[player.Castes.Count];
            LoadI = new int[player.Castes.Count];
            ViewRangeI = new int[player.Castes.Count];
            Grids = new Grid<CoreInsect>[player.Castes.Count];
            RangeI = new int[player.Castes.Count];
            EnergyI = new int[player.Castes.Count];
            AttackI = new int[player.Castes.Count];
            antsInCaste = new int[player.Castes.Count];

            int index = 0;
            foreach (CasteInfo caste in player.Castes)
            {
                SpeedI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                SpeedI[index] *= SimulationSettings.Custom.CasteSettings[caste.Speed].Speed;
                RotationSpeedI[index] = SimulationSettings.Custom.CasteSettings[caste.RotationSpeed].RotationSpeed;
                LoadI[index] = SimulationSettings.Custom.CasteSettings[caste.Load].Load;
                ViewRangeI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                ViewRangeI[index] *= SimulationSettings.Custom.CasteSettings[caste.ViewRange].ViewRange;
                RangeI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                RangeI[index] *= SimulationSettings.Custom.CasteSettings[caste.Range].Range;
                EnergyI[index] = SimulationSettings.Custom.CasteSettings[caste.Energy].Energy;
                AttackI[index] = SimulationSettings.Custom.CasteSettings[caste.Attack].Attack;

                Grids[index] =
                    Grid<CoreInsect>.Create
                        (
                        playground.Width * SimulationEnvironment.PLAYGROUND_UNIT,
                        playground.Height * SimulationEnvironment.PLAYGROUND_UNIT,
                        ViewRangeI[index]);

                index++;
            }
        }

        private void InitPlayground(CorePlayground spielfeld)
        {
            Playground = spielfeld;
            WidthI = spielfeld.Width * SimulationEnvironment.PLAYGROUND_UNIT;
            HeightI = spielfeld.Height * SimulationEnvironment.PLAYGROUND_UNIT;
            WidthI2 = WidthI * 2;
            HeightI2 = HeightI * 2;



            Marker =
                new Grid<CoreMarker>
                    (
                    spielfeld.Width * SimulationEnvironment.PLAYGROUND_UNIT,
                    spielfeld.Height * SimulationEnvironment.PLAYGROUND_UNIT,
                    SimulationSettings.Custom.MaximumMarkerSize * SimulationEnvironment.PLAYGROUND_UNIT);

            Id = nextId++;
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// colonies caste count
        /// </summary>
        public int CasteCount
        {
            get { return Player.Castes.Count; }
        }

        /// <summary>
        /// Constructor for a new insect instance
        /// </summary>
        internal void NewInsect(Random random)
        {
            Dictionary<string, int> dictionary = null;

            if (Player != null)
            {
                dictionary = new Dictionary<string, int>();
                foreach (CasteInfo caste in Player.Castes)
                {
                    if (!dictionary.ContainsKey(caste.Name))
                    {
                        dictionary.Add(caste.Name, antsInCaste[dictionary.Count]);
                    }
                }
            }

            //Insekt insekt =
            //    (Insekt)
            //    Klasse.Assembly.CreateInstance
            //        (Klasse.FullName, false, BindingFlags.Default, null, new object[] {this, dictionary}, null,
            //         new object[] {});
            CoreInsect insect = (CoreInsect)pClass.Assembly.CreateInstance(pClass.FullName);
            insect.Init(this, random, dictionary);

            // add insect to the insects list
            InsectsList.Add(insect);
            antsInCaste[insect.CasteIndexBase]++;
        }

        /// <summary>
        /// remove insect
        /// </summary>
        /// <param name="insect">insect</param>
        internal void RemoveInsect(CoreInsect insect)
        {
            if (InsectsList.Remove(insect))
            {
                antsInCaste[insect.CasteIndexBase]--;
            }
        }

        /// <summary>
        /// Constructor of a colony state information instance
        /// holds the current information about the colony
        /// </summary>
        /// <returns></returns>
        public ColonyState GenerateColonyStateInfo()
        {
            ColonyState info = new ColonyState(Id, Player.Guid, Player.ColonyName, Player.FirstName + " " + Player.LastName);

            info.CollectedFood = Statistic.CollectedFood;
            info.CollectedFruits = Statistic.CollectedFruits;
            info.StarvedAnts = Statistic.StarvedAnts;
            info.EatenAnts = Statistic.EatenAnts;
            info.BeatenAnts = Statistic.BeatenAnts;
            info.KilledBugs = Statistic.KilledBugs;
            info.KilledEnemies = Statistic.KilledAnts;
            info.Points = Statistic.Points;

            int index;

            for (index = 0; index < AntHills.Count; index++)
            {
                info.AnthillStates.Add(AntHills[index].GenerateAnthillStateInfo());
            }

            for (index = 1; index < Player.Castes.Count; index++)
            {
                info.CasteStates.Add(Player.Castes[index].CreateCasteStateInfo(Id, index));
            }

            for (index = 0; index < InsectsList.Count; index++)
            {
                info.AntStates.Add(((CoreAnt)InsectsList[index]).GenerateAntStateInfo());
            }

            // Markierungen ist ein Bucket und die Bucket Klasse enthält keinen
            // Indexer. Daher können wir hier keine for Schleife verwenden, wie eben
            // bei den Bauten und den Ameisen. Daher benutzen wir eine foreach
            // Schleife für die wir eine extra Laufvariable benötigen.
            foreach (CoreMarker markierung in Marker)
            {
                info.MarkerStates.Add(markierung.PopulateMarkerState());
            }

            return info;
        }
    }
}