using System;
using System.Collections.Generic;

using AntMe.SharedComponents.States;

namespace AntMe.Simulation {
    /// <summary>
    /// Ein Ameisenvolk. Kapselt alle Informationen die zur Laufzeit eines Spiels
    /// zu einem Spieler anfallen.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreColony {
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
        public readonly int Id;

        /// <summary>
        /// Die Guid, die das Volk eindeutig identifiziert.
        /// </summary>
        public readonly Guid Guid;

        internal readonly List<CoreInsect> Insects = new List<CoreInsect>();
        private readonly Type Klasse;
        internal readonly Grid<CoreMarker> Marker;
        internal readonly List<CoreMarker> NewMarker = new List<CoreMarker>();

        /// <summary>
        /// Der Spieler der das Verhalten des Volkes steuert.
        /// </summary>
        internal readonly PlayerInfo Player;

        internal readonly List<CoreInsect> VerhungerteInsekten = new List<CoreInsect>();

        internal int BreiteI;
        internal int BreiteI2;
        internal int HöheI;
        internal int HöheI2;
        internal CorePlayground Playground;

        internal PlayerStatistics Statistik = new PlayerStatistics();
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
        internal readonly int[] Angriff;

        /// <summary>
        /// Gitter für die verschiedenen Sichtweiten.
        /// </summary>
        internal readonly Grid<CoreInsect>[] Grids;

        /// <summary>
        /// Die Drehgeschwindigkeiten aller Kasten des Volkes in Grad pro Runde.
        /// </summary>
        internal readonly int[] Drehgeschwindigkeit;

        /// <summary>
        /// Die Lebenspunkte aller Kasten des Volkes.
        /// </summary>
        internal readonly int[] Energie;

        /// <summary>
        /// Die Geschwindigkeiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] GeschwindigkeitI;

        /// <summary>
        /// Die maximalen Lastwerte aller Kasten des Volkes.
        /// </summary>
        internal readonly int[] Last;

        /// <summary>
        /// Die Reichweiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] ReichweiteI;

        /// <summary>
        /// Die Sichtweiten aller Kasten des Volkes in der internen Einheit.
        /// </summary>
        internal readonly int[] SichtweiteI;

        #endregion

        /// <summary>
        /// Erzeugt eine neue Instanz der Volk-Klasse. Erzeugt ein Wanzen-Volk.
        /// </summary>
        /// <param name="spielfeld">Das Spielfeld.</param>
        internal CoreColony(CorePlayground spielfeld) {
            Playground = spielfeld;
            BreiteI = spielfeld.Width*SimulationEnvironment.PLAYGROUND_UNIT;
            HöheI = spielfeld.Height*SimulationEnvironment.PLAYGROUND_UNIT;
            BreiteI2 = BreiteI*2;
            HöheI2 = HöheI*2;

            Marker =
                new Grid<CoreMarker>
                    (
                    spielfeld.Width*SimulationEnvironment.PLAYGROUND_UNIT,
                    spielfeld.Height*SimulationEnvironment.PLAYGROUND_UNIT,
                    SimulationSettings.Custom.MarkerSizeMaximum *SimulationEnvironment.PLAYGROUND_UNIT);

            Id = nextId++;
            Guid = Guid.NewGuid();

            // Die Wanzen werden vom Spiel gesteuert.
            Player = null;

            // Die Klasse ist in diesem Fall bereits bekannt.
            Klasse = typeof (CoreBug);

            //TODO: Werte überprüfen.
            GeschwindigkeitI = new int[1] {SimulationSettings.Custom.BugSpeed*SimulationEnvironment.PLAYGROUND_UNIT};
            Drehgeschwindigkeit = new int[1] { SimulationSettings.Custom.BugRotationSpeed };
            ReichweiteI = new int[1] {int.MaxValue};
            SichtweiteI = new int[1] {0};
            Last = new int[1] {0};
            Energie = new int[1] { SimulationSettings.Custom.BugEnergy };
            Angriff = new int[1] { SimulationSettings.Custom.BugAttack };

            Grids = new Grid<CoreInsect>[1];
            Grids[0] =
                Grid<CoreInsect>.Create
                    (
                    spielfeld.Width*SimulationEnvironment.PLAYGROUND_UNIT,
                    spielfeld.Height*SimulationEnvironment.PLAYGROUND_UNIT,
                    SimulationSettings.Custom.BattleRange*SimulationEnvironment.PLAYGROUND_UNIT);

            antsInCaste = new int[1];
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der Volk-Klasse. Erzeugt ein Ameisen-Volk.
        /// </summary>
        /// <param name="spielfeld">Das Spielfeld.</param>
        /// <param name="spieler">Das Spieler zu dem das Volk gehört.</param>
        /// <param name="team">Das dazugehörige Team.</param>
        internal CoreColony(CorePlayground spielfeld, PlayerInfo spieler, CoreTeam team) {
            Playground = spielfeld;
            BreiteI = spielfeld.Width*SimulationEnvironment.PLAYGROUND_UNIT;
            HöheI = spielfeld.Height*SimulationEnvironment.PLAYGROUND_UNIT;
            BreiteI2 = BreiteI*2;
            HöheI2 = HöheI*2;

            Marker =
                new Grid<CoreMarker>
                    (
                    spielfeld.Width*SimulationEnvironment.PLAYGROUND_UNIT,
                    spielfeld.Height*SimulationEnvironment.PLAYGROUND_UNIT,
                    SimulationSettings.Custom.MarkerSizeMaximum *SimulationEnvironment.PLAYGROUND_UNIT);

            Id = nextId++;
            Guid = spieler.Guid;
            Team = team;

            Player = spieler;
            Klasse = spieler.assembly.GetType(spieler.ClassName, true, false);

            // Basisameisenkaste erstellen, falls keine Kasten definiert wurden
            if (spieler.Castes.Count == 0) {
                spieler.Castes.Add(new CasteInfo());
            }

            GeschwindigkeitI = new int[spieler.Castes.Count];
            Drehgeschwindigkeit = new int[spieler.Castes.Count];
            Last = new int[spieler.Castes.Count];
            SichtweiteI = new int[spieler.Castes.Count];
            Grids = new Grid<CoreInsect>[spieler.Castes.Count];
            ReichweiteI = new int[spieler.Castes.Count];
            Energie = new int[spieler.Castes.Count];
            Angriff = new int[spieler.Castes.Count];
            antsInCaste = new int[spieler.Castes.Count];

            int index = 0;
            foreach (CasteInfo caste in spieler.Castes) {
                GeschwindigkeitI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                GeschwindigkeitI[index] *= SimulationSettings.Custom.CasteSettings[caste.Speed].Speed;
                Drehgeschwindigkeit[index] = SimulationSettings.Custom.CasteSettings[caste.RotationSpeed].RotationSpeed;
                Last[index] = SimulationSettings.Custom.CasteSettings[caste.Load].Load;
                SichtweiteI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                SichtweiteI[index] *= SimulationSettings.Custom.CasteSettings[caste.ViewRange].ViewRange;
                ReichweiteI[index] = SimulationEnvironment.PLAYGROUND_UNIT;
                ReichweiteI[index] *= SimulationSettings.Custom.CasteSettings[caste.Range].Range;
                Energie[index] = SimulationSettings.Custom.CasteSettings[caste.Energy].Energy;
                Angriff[index] = SimulationSettings.Custom.CasteSettings[caste.Attack].Attack;

                Grids[index] =
                    Grid<CoreInsect>.Create
                        (
                        spielfeld.Width * SimulationEnvironment.PLAYGROUND_UNIT,
                        spielfeld.Height * SimulationEnvironment.PLAYGROUND_UNIT,
                        SichtweiteI[index]);

                index++;
            }
        }

        /// <summary>
        /// Die Anzahl von Insektenkasten in diesem Volk.
        /// </summary>
        public int AnzahlKasten {
            get { return Player.Castes.Count; }
        }

        /// <summary>
        /// Erstellt eine neues Insekt.
        /// </summary>
        internal void NeuesInsekt() {
            Dictionary<string, int> dictionary = null;

            if (Player != null) {
                dictionary = new Dictionary<string, int>();
                foreach (CasteInfo caste in Player.Castes) {
                    if (!dictionary.ContainsKey(caste.Name)) {
                        dictionary.Add(caste.Name, antsInCaste[dictionary.Count]);
                    }
                }
            }

            //Insekt insekt =
            //    (Insekt)
            //    Klasse.Assembly.CreateInstance
            //        (Klasse.FullName, false, BindingFlags.Default, null, new object[] {this, dictionary}, null,
            //         new object[] {});
            CoreInsect insekt = (CoreInsect) Klasse.Assembly.CreateInstance(Klasse.FullName);
            insekt.Init(this, dictionary);

            // Merke das Insekt.
            Insects.Add(insekt);
            antsInCaste[insekt.CasteIndexBase]++;
        }

        /// <summary>
        /// Entfernt ein Insekt.
        /// </summary>
        /// <param name="insekt">Insekt</param>
        internal void EntferneInsekt(CoreInsect insekt) {
            if (Insects.Remove(insekt)) {
                antsInCaste[insekt.CasteIndexBase]--;
            }
        }

        /// <summary>
        /// Erzeugt ein VolkZustand Objekt mit dem aktuellen Zustand des Volkes.
        /// </summary>
        /// <returns></returns>
        public ColonyState ErzeugeInfo() {
            ColonyState info = new ColonyState(Id, Player.Guid, Player.ColonyName, Player.FirstName + " " + Player.LastName);

            info.CollectedFood = Statistik.CollectedFood;
            info.CollectedFruits = Statistik.CollectedFruits;
            info.StarvedAnts = Statistik.StarvedAnts;
            info.EatenAnts = Statistik.EatenAnts;
            info.BeatenAnts = Statistik.BeatenAnts;
            info.KilledBugs = Statistik.KilledBugs;
            info.KilledEnemies = Statistik.KilledAnts;
            info.Points = Statistik.Points;

            int index;

            for (index = 0; index < AntHills.Count; index++) {
                info.AnthillStates.Add(AntHills[index].ErzeugeInfo());
            }

            for (index = 1; index < Player.Castes.Count; index++) {
                info.CasteStates.Add(Player.Castes[index].CreateState(Id, index));
            }

            for (index = 0; index < Insects.Count; index++) {
                info.AntStates.Add(((CoreAnt) Insects[index]).ErzeugeInfo());
            }

            // Markierungen ist ein Bucket und die Bucket Klasse enthält keinen
            // Indexer. Daher können wir hier keine for Schleife verwenden, wie eben
            // bei den Bauten und den Ameisen. Daher benutzen wir eine foreach
            // Schleife für die wir eine extra Laufvariable benötigen.
            foreach (CoreMarker markierung in Marker) {
                info.MarkerStates.Add(markierung.ErzeugeInfo());
            }

            return info;
        }
    }
}