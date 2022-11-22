using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// Ein Ameisenvolk. Kapselt alle Informationen die zur Laufzeit eines Spiels
    /// zu einem Spieler anfallen.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreColony
    {
        // Die Id des nächsten erzeugten Volkes.
        private static int nextId = 0;
        private readonly int[] antsInCaste;
        internal readonly List<CoreAnthill> AntHills = new List<CoreAnthill>();
        internal readonly CoreTeam Team;
        internal readonly List<CoreInsect> BeatenInsects = new List<CoreInsect>();
        internal readonly List<CoreInsect> EatenInsects = new List<CoreInsect>();

        /// <summary>
        /// Die Id die das Volk während eines laufenden Spiels idenzifiziert.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Die Guid, die das Volk eindeutig identifiziert.
        /// </summary>
        public Guid Guid { get; private set; }

        internal readonly List<CoreInsect> Insects = new List<CoreInsect>();
        private readonly Type Klasse;
        internal Grid<CoreMarker> Marker { get; private set; }
        internal readonly List<CoreMarker> NewMarker = new List<CoreMarker>();

        /// <summary>
        /// Der Spieler der das Verhalten des Volkes steuert.
        /// </summary>
        internal readonly PlayerInfo Player;

        internal readonly List<CoreInsect> StarvedInsects = new List<CoreInsect>();

        internal int WidthI;
        internal int WidthI2;
        internal int HightI;
        internal int HightI2;
        internal CorePlayground Playground;

        internal PlayerStatistics Statistic = new PlayerStatistics();
        internal PlayerStatistics StatistikDurchschnitt = new PlayerStatistics();

        /// <summary>
        /// Zählt die Anzahl an Runden herunter, bis wieder eine neus Insekt erzeugt
        /// werden kann.
        /// </summary>
        internal int insectDelay = 0;

        internal int insectCountDown;

        #region Fähigkeiten

        /// <summary>
        /// Die Angriffswerte aller Kasten des Volkes.
        /// </summary>
        internal readonly int[] AttackI;

        /// <summary>
        /// Gitter für die verschiedenen Sichtweiten.
        /// </summary>
        internal readonly Grid<CoreInsect>[] Grids;

        /// <summary>
        /// Die Drehgeschwindigkeiten aller Kasten des Volkes in Grad pro Runde.
        /// </summary>
        internal readonly int[] RotationSpeed;

        /// <summary>
        /// Die Lebenspunkte aller Kasten des Volkes.
        /// </summary>
        internal readonly int[] EnergyI;

        /// <summary>
        /// Die Geschwindigkeiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] SpeedI;

        /// <summary>
        /// Die maximalen Lastwerte aller Kasten des Volkes.
        /// </summary>
        internal readonly int[] Burden;

        /// <summary>
        /// Die Reichweiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] RangeI;

        /// <summary>
        /// Die Sichtweiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] ViewRangeI;

        #endregion

        /// <summary>
        /// Erzeugt eine neue Instanz der Volk-Klasse. Erzeugt ein Wanzen-Volk.
        /// </summary>
        /// <param name="spielfeld">Das Spielfeld.</param>
        internal CoreColony(CorePlayground spielfeld)
        {
            InitPlayground(spielfeld);

            // Die Wanzen werden vom Spiel gesteuert.
            Player = null;

            // Die Klasse ist in diesem Fall bereits bekannt.
            Klasse = typeof(CoreBug);

            //TODO: Werte überprüfen.
            SpeedI = new int[1] { SimulationSettings.Custom.BugSpeed * SimulationEnvironment.PLAYGROUND_UNIT };
            RotationSpeed = new int[1] { SimulationSettings.Custom.BugRotationSpeed };
            RangeI = new int[1] { int.MaxValue };
            ViewRangeI = new int[1] { 0 };
            Burden = new int[1] { 0 };
            EnergyI = new int[1] { SimulationSettings.Custom.BugEnergy };
            AttackI = new int[1] { SimulationSettings.Custom.BugAttack };

            Grids = new Grid<CoreInsect>[1];
            Grids[0] =
                Grid<CoreInsect>.Create
                    (
                    spielfeld.Width * SimulationEnvironment.PLAYGROUND_UNIT,
                    spielfeld.Height * SimulationEnvironment.PLAYGROUND_UNIT,
                    SimulationSettings.Custom.BattleRange * SimulationEnvironment.PLAYGROUND_UNIT);

            antsInCaste = new int[1];
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der Volk-Klasse. Erzeugt ein Ameisen-Volk.
        /// </summary>
        /// <param name="spielfeld">Das Spielfeld.</param>
        /// <param name="spieler">Das Spieler zu dem das Volk gehört.</param>
        /// <param name="team">Das dazugehörige Team.</param>
        internal CoreColony(CorePlayground spielfeld, PlayerInfo spieler, CoreTeam team)
        {
            InitPlayground(spielfeld);
            Team = team;

            Player = spieler;
            Klasse = spieler.assembly.GetType(spieler.ClassName, true, false);

            // Basisameisenkaste erstellen, falls keine Kasten definiert wurden
            if (spieler.Castes.Count == 0)
            {
                spieler.Castes.Add(new CasteInfo());
            }

            SpeedI = new int[spieler.Castes.Count];
            RotationSpeed = new int[spieler.Castes.Count];
            Burden = new int[spieler.Castes.Count];
            ViewRangeI = new int[spieler.Castes.Count];
            Grids = new Grid<CoreInsect>[spieler.Castes.Count];
            RangeI = new int[spieler.Castes.Count];
            EnergyI = new int[spieler.Castes.Count];
            AttackI = new int[spieler.Castes.Count];
            antsInCaste = new int[spieler.Castes.Count];

            int index = 0;
            foreach (CasteInfo caste in spieler.Castes)
            {
                SpeedI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                SpeedI[index] *= SimulationSettings.Custom.CasteSettings[caste.Speed].Speed;
                RotationSpeed[index] = SimulationSettings.Custom.CasteSettings[caste.RotationSpeed].RotationSpeed;
                Burden[index] = SimulationSettings.Custom.CasteSettings[caste.Load].Load;
                ViewRangeI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                ViewRangeI[index] *= SimulationSettings.Custom.CasteSettings[caste.ViewRange].ViewRange;
                RangeI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                RangeI[index] *= SimulationSettings.Custom.CasteSettings[caste.Range].Range;
                EnergyI[index] = SimulationSettings.Custom.CasteSettings[caste.Energy].Energy;
                AttackI[index] = SimulationSettings.Custom.CasteSettings[caste.Attack].Attack;

                Grids[index] =
                    Grid<CoreInsect>.Create
                        (
                        spielfeld.Width * SimulationEnvironment.PLAYGROUND_UNIT,
                        spielfeld.Height * SimulationEnvironment.PLAYGROUND_UNIT,
                        ViewRangeI[index]);

                index++;
            }
        }

        private void InitPlayground(CorePlayground spielfeld)
        {
            Playground = spielfeld;
            WidthI = spielfeld.Width * SimulationEnvironment.PLAYGROUND_UNIT;
            HightI = spielfeld.Height * SimulationEnvironment.PLAYGROUND_UNIT;
            WidthI2 = WidthI * 2;
            HightI2 = HightI * 2;



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
        /// Die Anzahl von Insektenkasten in diesem Volk.
        /// </summary>
        public int CasteCount
        {
            get { return Player.Castes.Count; }
        }

        /// <summary>
        /// Erstellt eine neues Insekt.
        /// </summary>
        internal void NeuesInsekt(Random random)
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
            CoreInsect insekt = (CoreInsect)Klasse.Assembly.CreateInstance(Klasse.FullName);
            insekt.Init(this, random, dictionary);

            // Merke das Insekt.
            Insects.Add(insekt);
            antsInCaste[insekt.CasteIndexBase]++;
        }

        /// <summary>
        /// Entfernt ein Insekt.
        /// </summary>
        /// <param name="insekt">Insekt</param>
        internal void EntferneInsekt(CoreInsect insekt)
        {
            if (Insects.Remove(insekt))
            {
                antsInCaste[insekt.CasteIndexBase]--;
            }
        }

        /// <summary>
        /// Erzeugt ein VolkZustand Objekt mit dem aktuellen Zustand des Volkes.
        /// </summary>
        /// <returns></returns>
        public ColonyState ErzeugeInfo()
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
                info.AnthillStates.Add(AntHills[index].ErzeugeInfo());
            }

            for (index = 1; index < Player.Castes.Count; index++)
            {
                info.CasteStates.Add(Player.Castes[index].CreateState(Id, index));
            }

            for (index = 0; index < Insects.Count; index++)
            {
                info.AntStates.Add(((CoreAnt)Insects[index]).GenerateInformation());
            }

            // Markierungen ist ein Bucket und die Bucket Klasse enthält keinen
            // Indexer. Daher können wir hier keine for Schleife verwenden, wie eben
            // bei den Bauten und den Ameisen. Daher benutzen wir eine foreach
            // Schleife für die wir eine extra Laufvariable benötigen.
            foreach (CoreMarker markierung in Marker)
            {
                info.MarkerStates.Add(markierung.ErzeugeInfo());
            }

            return info;
        }
    }
}