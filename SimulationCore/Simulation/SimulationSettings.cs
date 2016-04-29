using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace AntMe.Simulation {
    /// <summary>
    /// Simulation-Settings from application-configuration.
    /// </summary>
    [Serializable]
    public struct SimulationSettings {
        #region internal Varialbes

        /// <summary>
        /// Sets or gets the name of this settings-set.
        /// </summary>
        public string SettingsName;

        /// <summary>
        /// Gets or sets Guid of that settings-set.
        /// </summary>
        public Guid Guid;

        #region Playground

        /// <summary>
        /// Gets the size of the playground in SquareAntSteps.
        /// </summary>
        public int PlayGroundBaseSize;

        /// <summary>
        /// Gets the multiplier of additional playground-size per player.
        /// </summary>
        public float PlayGroundSizePlayerMultiplier;

        /// <summary>
        /// Gets the radius of anthills.
        /// </summary>
        public int AntHillRadius;

        /// <summary>
        /// Minimum Battle-Distance in steps between two insects.
        /// </summary>
        public int BattleRange;

        #endregion

        #region Livetime and Respawn

        /// <summary>
        /// Gets the maximum count of ants simultaneous on playground.
        /// </summary>
        public int AntSimultaneousCount;

        /// <summary>
        /// Gets the maximum count of bugs simultaneous on playground.
        /// </summary>
        public int BugSimultaneousCount;

        /// <summary>
        /// Gets the maximum count of sugar simultaneous on playground.
        /// </summary>
        public int SugarSimultaneousCount;

        /// <summary>
        /// Gets the maximum count of fruit simultaneous on playground.
        /// </summary>
        public int FruitSimultaneousCount;

        /// <summary>
        /// Gets the multiplier for maximum count of bugs simultaneous on playground per player.
        /// </summary>
        public float BugCountPlayerMultiplier;

        /// <summary>
        /// Gets the multiplier for maximum count of sugar simultaneous on playground per player.
        /// </summary>
        public float SugarCountPlayerMultiplier;

        /// <summary>
        /// Gets the multiplier for maximum count of fruit simultaneous on playground per player.
        /// </summary>
        public float FruitCountPlayerMultiplier;

        /// <summary>
        /// Gets the multiplier for maximum count of ants simultaneous on playground per player.
        /// </summary>
        public float AntCountPlayerMultiplier;

        /// <summary>
        /// Gets the maximum count of ants in the whole simulation.
        /// </summary>
        public int AntTotalCount;

        /// <summary>
        /// Gets the maximum count of bugs in the whole simulation.
        /// </summary>
        public int BugTotalCount;

        /// <summary>
        /// Gets the maximum count of sugar in the whole simulation.
        /// </summary>
        public int SugarTotalCount;

        /// <summary>
        /// Gets the maximum count of fruit in the whole simulation.
        /// </summary>
        public int FruitTotalCount;

        /// <summary>
        /// Gets the multiplier for maximum count of ants per player in the whole simulation.
        /// </summary>
        public float AntTotalCountPlayerMultiplier;

        /// <summary>
        /// Gets the multiplier for maximum count of bugs per player in the whole simulation.
        /// </summary>
        public float BugTotalCountPlayerMultiplier;

        /// <summary>
        /// Gets the multiplier for maximum count of sugar per player in the whole simulation.
        /// </summary>
        public float SugarTotalCountPlayerMultiplier;

        /// <summary>
        /// Gets the multiplier for maximum count of fruit per player in the whole simulation.
        /// </summary>
        public float FruitTotalCountPlayerMultiplier;

        /// <summary>
        /// Gets the delay for ant before next respawn in rounds. 
        /// </summary>
        public int AntRespawnDelay;

        /// <summary>
        /// Gets the delay for bugs before next respawn in rounds. 
        /// </summary>
        public int BugRespawnDelay;

        /// <summary>
        /// Gets the delay for sugar before next respawn in rounds. 
        /// </summary>
        public int SugarRespawnDelay;

        /// <summary>
        /// Gets the delay for fruits before next respawn in rounds. 
        /// </summary>
        public int FruitRespawnDelay;

        #endregion

        #region Bugsettings

        /// <summary>
        /// Gets the attack-value of bugs.
        /// </summary>
        public int BugAttack;

        /// <summary>
        /// Gets the rotation speed of bugs.
        /// </summary>
        public int BugRotationSpeed;

        /// <summary>
        /// Gets the energy of bugs.
        /// </summary>
        public int BugEnergy;

        /// <summary>
        /// Gets the speed of bugs.
        /// </summary>
        public int BugSpeed;

        /// <summary>
        /// Gets the attack-range of bugs.
        /// </summary>
        public int BugRadius;

        /// <summary>
        /// Gets the regeneration-value of bugs.
        /// </summary>
        public int BugRegenerationValue;

        /// <summary>
        /// Gets the delay in rounds between the regeneration-steps of bugs.
        /// </summary>
        public int BugRegenerationDelay;

        #endregion

        #region Foodstuff

        /// <summary>
        /// Gets the minimal amount of food in sugar-hills.
        /// </summary>
        public int SugarAmountMinimum;

        /// <summary>
        /// Gets the maximum amount of food in sugar-hills.
        /// </summary>
        public int SugarAmountMaximum;

        /// <summary>
        /// Gets the minimal amount of food in fruits.
        /// </summary>
        public int FruitAmountMinimum;

        /// <summary>
        /// Gets the maximum amount of food in fruits.
        /// </summary>
        public int FruitAmountMaximum;

        /// <summary>
        /// Gets the multiplier for fruits between load and amount of food.
        /// </summary>
        public float FruitLoadMultiplier;

        /// <summary>
        /// Gets the multiplier for fruits between radius and amount of food.
        /// </summary>
        public float FruitRadiusMultiplier;

        #endregion

        #region Marker

        /// <summary>
        /// Gets the minimal size of a marker.
        /// </summary>
        public int MarkerSizeMinimum;

        /// <summary>
        /// Gets the minimal allowed distance between to marker.
        /// </summary>
        public int MarkerDistance;

        /// <summary>
        /// Gets the maximum age in rounds of a marker.
        /// </summary>
        public int MarkerMaximumAge;

        #endregion

        #region Points

        /// <summary>
        /// Multiplier for the calculation from food to points.
        /// </summary>
        public float PointsForFoodMultiplier;

        /// <summary>
        /// Gets the amount of points for collected fruits.
        /// </summary>
        public int PointsForFruits;

        /// <summary>
        /// Gets the amount of points for killed bugs.
        /// </summary>
        public int PointsForBug;

        /// <summary>
        /// Gets the amount of points for killed foreign ants.
        /// </summary>
        public int PointsForForeignAnt;

        /// <summary>
        /// Gets the amount of points for own dead ants killed by bugs.
        /// </summary>
        public int PointsForEatenAnts;

        /// <summary>
        /// Gets the amount of points for own dead ants killed by foreign ants.
        /// </summary>
        public int PointsForBeatenAnts;

        /// <summary>
        /// Gets the amount of points for own dead starved ants.
        /// </summary>
        public int PointsForStarvedAnts;

        #endregion

        #region Castes

        /// <summary>
        /// Gives the caste-Settings.
        /// </summary>
        public SimulationCasteSettings CasteSettings;

        #endregion

        #endregion

        #region Singleton

        private static SimulationSettings defaultSettings = new SimulationSettings();
        private static SimulationSettings customSettings = new SimulationSettings();
        private static bool initDefault;
        private static bool initCustom;

        /// <summary>
        /// Gets the default settings.
        /// </summary>
        public static SimulationSettings Default
        {
            get {
                if (!initDefault) {
                    defaultSettings.SetDefaults();
                    initDefault = true;
                }
                return defaultSettings;
            }
        }

        /// <summary>
        /// Gives the current simulation-settings
        /// </summary>
        public static SimulationSettings Custom {
            get {
                if (!initCustom) {
                    return Default;
                }
                return customSettings;
            }
        }

        /// <summary>
        /// Sets a custom set of settings.
        /// </summary>
        /// <param name="settings">custom settings</param>
        public static void SetCustomSettings(SimulationSettings settings) {
            settings.RuleCheck();
            customSettings = settings;
            initCustom = true;
        }

        #endregion

        #region Default-Methods

        /// <summary>
        /// Resets the values to the default settings.
        /// </summary>
        public void SetDefaults() {
            SettingsName = Resource.SettingsDefaultName;

            // Guid
            Guid = new Guid("{C010EC26-0F4C-442c-8C36-0D6A71842A41}");

            // Playground
            PlayGroundBaseSize = 550000;
            PlayGroundSizePlayerMultiplier = 1;
            AntHillRadius = 32;
            BattleRange = 5;

            // Livetime and Respawn
            AntSimultaneousCount = 100;
            BugSimultaneousCount = 5;
            SugarSimultaneousCount = 1;
            FruitSimultaneousCount = 2;

            BugCountPlayerMultiplier = 1f;
            SugarCountPlayerMultiplier = 1f;
            FruitCountPlayerMultiplier = 1f;
            AntCountPlayerMultiplier = 0f;

            AntTotalCount = 999999;
            BugTotalCount = 999999;
            SugarTotalCount = 999999;
            FruitTotalCount = 999999;

            AntTotalCountPlayerMultiplier = 0f;
            BugTotalCountPlayerMultiplier = 0f;
            SugarTotalCountPlayerMultiplier = 0f;
            FruitTotalCountPlayerMultiplier = 0f;

            AntRespawnDelay = 15;
            BugRespawnDelay = 75;
            SugarRespawnDelay = 150;
            FruitRespawnDelay = 225;

            // Bugsettings
            BugAttack = 50;
            BugRotationSpeed = 3;
            BugEnergy = 1000;
            BugSpeed = 3;
            BugRadius = 4;
            BugRegenerationValue = 1;
            BugRegenerationDelay = 5;

            // Foodstuff
            SugarAmountMinimum = 1000;
            SugarAmountMaximum = 1000;
            FruitAmountMinimum = 250;
            FruitAmountMaximum = 250;
            FruitLoadMultiplier = 5;
            FruitRadiusMultiplier = 1;

            // Marker
            MarkerSizeMinimum = 20;           
            MarkerDistance = 13;
            MarkerMaximumAge = 150;

            // Points
            PointsForFoodMultiplier = 1;
            PointsForFruits = 0;
            PointsForBug = 150;
            PointsForForeignAnt = 5;
            PointsForEatenAnts = 0;
            PointsForBeatenAnts = -5;
            PointsForStarvedAnts = 0;

            // Castes
            CasteSettings = new SimulationCasteSettings();
            CasteSettings.Offset = -1;
            CasteSettings.Columns = new SimulationCasteSettingsColumn[4];

            CasteSettings.Columns[0].Attack = 0;
            CasteSettings.Columns[0].Energy = 50;
            CasteSettings.Columns[0].Load = 4;
            CasteSettings.Columns[0].Range = 1800;
            CasteSettings.Columns[0].RotationSpeed = 6;
            CasteSettings.Columns[0].Speed = 3;
            CasteSettings.Columns[0].ViewRange = 45;

            CasteSettings.Columns[1].Attack = 10;
            CasteSettings.Columns[1].Energy = 100;
            CasteSettings.Columns[1].Load = 5;
            CasteSettings.Columns[1].Range = 2250;
            CasteSettings.Columns[1].RotationSpeed = 8;
            CasteSettings.Columns[1].Speed = 4;
            CasteSettings.Columns[1].ViewRange = 60;

            CasteSettings.Columns[2].Attack = 20;
            CasteSettings.Columns[2].Energy = 175;
            CasteSettings.Columns[2].Load = 7;
            CasteSettings.Columns[2].Range = 3400;
            CasteSettings.Columns[2].RotationSpeed = 12;
            CasteSettings.Columns[2].Speed = 5;
            CasteSettings.Columns[2].ViewRange = 75;

            CasteSettings.Columns[3].Attack = 30;
            CasteSettings.Columns[3].Energy = 250;
            CasteSettings.Columns[3].Load = 10;
            CasteSettings.Columns[3].Range = 4500;
            CasteSettings.Columns[3].RotationSpeed = 16;
            CasteSettings.Columns[3].Speed = 6;
            CasteSettings.Columns[3].ViewRange = 90;
        }

        /// <summary>
        /// Checks the value-ranges of all properties.
        /// </summary>
        public void RuleCheck() {

            // TODO: Strings into res-files

            // Playground
            if (PlayGroundBaseSize < 100000) {
                throw new ConfigurationErrorsException("Grundgröße des Spielfeldes muss größer 100.000 sein");
            }

            if (PlayGroundSizePlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Playground Playermultiplikator darf nicht kleiner 0 sein");
            }

            if (AntHillRadius < 0) {
                throw new ConfigurationErrorsException("Ameisenbau braucht einen Radius >= 0");
            }
            
            if (BattleRange < 0) {
                throw new ConfigurationErrorsException("Angriffsradius der Wanze darf nicht kleiner 0 sein");
            }

            // Livetime and Respawn
            if (AntSimultaneousCount < 0) {
                throw new ConfigurationErrorsException("Weniger als 0 simultane Ameisen sind nicht möglich");
            }

            if (BugSimultaneousCount < 0) {
                throw new ConfigurationErrorsException("Weniger als 0 simultane Wanzen sind nicht möglich");
            }

            if (SugarSimultaneousCount < 0) {
                throw new ConfigurationErrorsException("Weniger als 0 simultane Zuckerberge sind nicht möglich");
            }

            if (FruitSimultaneousCount < 0) {
                throw new ConfigurationErrorsException("Weniger als 0 simultanes Obst sind nicht möglich");
            }

            if (BugCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Wanzen ist nicht zulässig");
            }
            
            if (SugarCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Zucker ist nicht zulässig");
            }

            if (FruitCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Obst ist nicht zulässig");
            }

            if (AntCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Ameisen ist nicht zulässig");
            }

            if (AntTotalCount < 0) {
                throw new ConfigurationErrorsException("Negative Gesamtmenge bei Ameisen ist nicht zulässig");
            }

            if (BugTotalCount < 0) {
                throw new ConfigurationErrorsException("Negative Gesamtmenge bei Wanzen ist nicht zulässig");
            }

            if (SugarTotalCount < 0) {
                throw new ConfigurationErrorsException("Negative Gesamtmenge bei Zucker ist nicht zulässig");
            }

            if (FruitTotalCount < 0) {
                throw new ConfigurationErrorsException("Negative Gesamtmenge bei Obst ist nicht zulässig");
            }

            if (AntTotalCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Ameisen ist nicht zulässig");
            }

            if (BugTotalCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Wanzen ist nicht zulässig");
            }

            if (SugarTotalCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Zucker ist nicht zulässig");
            }

            if (FruitTotalCountPlayerMultiplier < 0) {
                throw new ConfigurationErrorsException("Negative Spielermuliplikatoren bei Obst ist nicht zulässig");
            }

            if (AntRespawnDelay < 0) {
                throw new ConfigurationErrorsException("Negative Respawnzeit bei Ameisen ist nicht zulässig");
            }

            if (BugRespawnDelay < 0) {
                throw new ConfigurationErrorsException("Negative Respawnzeit bei Wanzen ist nicht zulässig");
            }

            if (SugarRespawnDelay < 0) {
                throw new ConfigurationErrorsException("Negative Respawnzeit bei Zucker ist nicht zulässig");
            }

            if (FruitRespawnDelay < 0) {
                throw new ConfigurationErrorsException("Negative Respawnzeit bei Obst ist nicht zulässig");
            }

            // Bugsettings
            if (BugAttack < 0) {
                throw new ConfigurationErrorsException("Negativer Angriffswert für Wanzen ist nicht zulässig");
            }

            if (BugRotationSpeed < 0) {
                throw new ConfigurationErrorsException("Negative Rotationsgeschwindigkeit für Wanzen ist nicht zulässig");
            }

            if (BugEnergy < 0) {
                throw new ConfigurationErrorsException("Negativer Energiewert für Wanzen ist nicht zulässig");
            }

            if (BugSpeed < 0) {
                throw new ConfigurationErrorsException("Negativer Geschwindigkeitswert für Wanzen ist nicht zulässig");
            }

            if (BugRadius < 0) {
                throw new ConfigurationErrorsException("Negativer Radius für Wanzen ist nicht zulässig");
            }

            if (BugRegenerationValue < 0) {
                throw new ConfigurationErrorsException("Negativer Regenerationswert für Wanzen ist nicht zulässig");
            }

            if (BugRegenerationDelay < 0) {
                throw new ConfigurationErrorsException("Negativer Regenerationsdelay für Wanzen ist nicht zulässig");
            }

            // Foodstuff
            if (SugarAmountMinimum < 0) {
                throw new ConfigurationErrorsException("Negativer Nahrungswert bei Zucker ist nicht zulässig");
            }

            if (SugarAmountMaximum < 0) {
                throw new ConfigurationErrorsException("Negativer Nahrungswert bei Zucker ist nicht zulässig");
            }

            if (FruitAmountMinimum < 0) {
                throw new ConfigurationErrorsException("Negativer Nahrungswert bei Obst ist nicht zulässig");
            }

            if (FruitAmountMaximum < 0) {
                throw new ConfigurationErrorsException("Negativer Nahrungswert bei Obst ist nicht zulässig");
            }

            if (FruitLoadMultiplier < 0) {
                throw new ConfigurationErrorsException("Negativer Loadmultiplikator bei Obst ist nicht zulässig");
            }

            if (FruitRadiusMultiplier < 0) {
                throw new ConfigurationErrorsException("Negativer Radiusmultiplikator bei Obst ist nicht zulässig");
            }

            // Marker

            if (MarkerSizeMinimum < 0) {
                throw new ConfigurationErrorsException("Negative Minimalgröße bei Markierung ist nicht zulässig");
            }

            if (MarkerDistance < 0) {
                throw new ConfigurationErrorsException("Negative Mindestdistanz bei Markierung ist nicht zulässig");
            }

            if (MarkerMaximumAge < 0) {
                throw new ConfigurationErrorsException("Negative maximallebensdauer bei Markierungen ist nicht zulässig");
            }

            // Castes
            CasteSettings.RuleCheck();           
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the maximal Speed of an insect.
        /// </summary>
        public int MaximumSpeed {
            get {
                int maxValue = BugSpeed;
                for (int i = 0; i < CasteSettings.Columns.Length; i++) {
                    maxValue = Math.Max(maxValue, CasteSettings.Columns[i].Speed);
                }
                return maxValue;
            }
        }

        /// <summary>
        /// Gets the maximal size of a marker.
        /// </summary>
        public int MarkerSizeMaximum {
            get {
                return MarkerSizeMinimum * MarkerMaximumAge;
            }
        }

        /// <summary>
        /// Gets the maximal size of a marker (without radius)
        /// </summary>
        public int MarkerRangeMaximum {
            get {
                return MarkerSizeMaximum - MarkerSizeMinimum;
            }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Saves the settings to a setting-file.
        /// </summary>
        /// <param name="settings">settings to save</param>
        /// <param name="filename">filename of target file</param>
        public static void SaveSettings(SimulationSettings settings, string filename) {
            // Open Filestream
            FileStream target = new FileStream(filename, FileMode.Create, FileAccess.Write);

            // Serialize
            try {
                XmlSerializer serializer = new XmlSerializer(typeof (SimulationSettings));
                serializer.Serialize(target, settings);
            }
            finally {
                target.Close();
            }
        }

        /// <summary>
        /// Loads settings from a setting-file.
        /// </summary>
        /// <param name="filename">filename of target file</param>
        /// <returns>Loaded settings</returns>
        public static SimulationSettings LoadSettings(string filename) {
            
            // Open File
            using (FileStream source = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return LoadSettings(source);
            }
        }

        public static SimulationSettings LoadSettings(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SimulationSettings));
            return (SimulationSettings)serializer.Deserialize(stream);
        }

        #endregion

        /// <summary>
        /// Gives the name of the settings.
        /// </summary>
        /// <returns>Name of Settings</returns>
        public override string ToString()
        {
            return SettingsName;
        }

        /// <summary>
        /// Checks, if two different simulation-sets are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SimulationSettings)) {
                return false;
            }

            return Guid.Equals(((SimulationSettings)obj).Guid);
        }

        /// <summary>
        /// Generates a Hash-Code for that object.
        /// </summary>
        /// <returns>Hash-Code</returns>
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}