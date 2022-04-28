using AntMe.SharedComponents.Tools;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AntMe.Simulation
{
    /// <summary>
    /// Player-Analysis-Class 
    /// </summary>
    internal sealed class PlayerAnalysis
    {
        #region local Variables

        /// <summary>
        /// Type of baseAnt from core.
        /// </summary>
        private TypeDefinition _coreAnt;

        /// <summary>
        /// List of all Ant-BaseClasses in every known language.
        /// </summary>
        private Dictionary<PlayerLanguages, TypeDefinition> _languageBases;

        #endregion

        #region public Methods

        /// <summary>
        /// Sets special simulation-settings.
        /// </summary>
        /// <param name="settings">special settings</param>
        public void InitSettings(SimulationSettings settings)
        {
            SimulationSettings.SetCustomSettings(settings);
        }

        /// <summary>
        /// Searches in the given file for possible PlayerInfos and delivers a list of found Players.
        /// </summary>
        /// <param name="file">Ai-File as binary file-dump.</param>
        /// <param name="checkRules">True, if Analyzer should also check player-rules</param>
        /// <returns>List of found players.</returns>
        public List<PlayerInfo> Analyse(byte[] file, bool checkRules)
        {
            // load base-class from Simulation-Core.#     
            ModuleDefinition simulation = ModuleDefinition.ReadModule(Assembly.GetExecutingAssembly().Modules.First().FullyQualifiedName);
            _coreAnt = simulation.GetType("AntMe.Simulation.CoreAnt");
            //coreAnt = simulation.GetType("AntMe.Simulation.CoreAnt");

            // load all base-classes of different languages
            _languageBases = new Dictionary<PlayerLanguages, TypeDefinition>();
            _languageBases.Add(
                PlayerLanguages.Deutsch,
                simulation.GetType("AntMe.Deutsch.Basisameise"));
            _languageBases.Add(
                PlayerLanguages.English,
                simulation.GetType("AntMe.English.BaseAnt"));

            // open Ai-File
            using (var filestream = new MemoryStream(file))
            {
                return analyseAssembly(ModuleDefinition.ReadModule(filestream), checkRules);
            }
        }

        #endregion

        #region Helpermethods

        /// <summary>
        /// Analyzes the given for any kind of valid player-information.
        /// </summary>
        /// <param name="module">given module.</param>
        /// <param name="checkRules">true, if the Analyser should also check player-rules</param>
        /// <returns>List of valid players.</returns>
        private List<PlayerInfo> analyseAssembly(ModuleDefinition module, bool checkRules)
        {
            // All included players would be marked as static, if one static play is found.
            bool staticVariables = false;

            bool foreignReferences = false;
            string referenceInformation = string.Empty;

            List<PlayerInfo> players = new List<PlayerInfo>();

            // filter valid references
            foreach (var reference in module.AssemblyReferences)
            {
                switch (reference.Name)
                {
                    case "mscorlib":
                        // Framework-Core
                        byte[] coreschlüssel = new AssemblyName(typeof(object).Assembly.FullName).GetPublicKeyToken();
                        byte[] referenzschlüssel = reference.PublicKeyToken;
                        if (coreschlüssel[0] != referenzschlüssel[0] ||
                            coreschlüssel[1] != referenzschlüssel[1] ||
                            coreschlüssel[2] != referenzschlüssel[2] ||
                            coreschlüssel[3] != referenzschlüssel[3] ||
                            coreschlüssel[4] != referenzschlüssel[4] ||
                            coreschlüssel[5] != referenzschlüssel[5] ||
                            coreschlüssel[6] != referenzschlüssel[6] ||
                            coreschlüssel[7] != referenzschlüssel[7])
                        {
                            throw new RuleViolationException(Resource.SimulationCoreAnalysisCheatWithFxFake);
                        }
                        break;
                    case "Microsoft.VisualBasic":
                        // TODO: Prüfen, wie wir damit umgehen
                        break;
                    case "System":
                        // TODO: Prüfen, wie wir damit umgehen
                        break;
                    case "AntMe.Simulation":
                        if (Assembly.GetCallingAssembly().FullName != reference.FullName)
                            throw new RuleViolationException(Resource.SimulationCoreAnalysisNotSupportedAntVersion);

                        //if (reference.Version.Major == 1) {
                        //    switch (reference.Version.Minor) {
                        //        case 1:
                        //            // Version 1.1
                        //            throw new NotSupportedException(
                        //                Resource.SimulationCoreAnalysisNotSupportedAntVersion);
                        //        case 5:
                        //            // Version 1.5
                        //            try {
                        //                Assembly.ReflectionOnlyLoad("AntMe.Simulation.Ameise");
                        //            }
                        //            catch (Exception ex) {
                        //                throw new DllNotFoundException(
                        //                    string.Format(
                        //                        Resource.SimulationCoreAnalysisOldAssemblyLoadFailed,
                        //                        reference.Version.ToString(2)),
                        //                    ex);
                        //            }
                        //            break;
                        //        case 6:
                        //            if (Assembly.GetCallingAssembly().FullName != reference.FullName) {
                        //                throw new RuleViolationException(Resource.SimulationCoreAnalysisNewerVersion);
                        //            }
                        //            break;
                        //        default:
                        //            // unknown Version
                        //            throw new NotSupportedException(
                        //                string.Format(
                        //                    Resource.SimulationCoreAnalysisUnknownVersion, reference.Version.ToString(2)));
                        //    }
                        //}
                        //else {
                        //    // unknown Version
                        //    throw new NotSupportedException(
                        //        string.Format(
                        //            Resource.SimulationCoreAnalysisUnknownVersion, reference.Version.ToString(2)));
                        //}
                        break;
                    default:
                        // load unknown refereneces and add to list
                        try
                        {
                            //Assembly.ReflectionOnlyLoad(reference.FullName);
                        }
                        catch (Exception ex)
                        {
                            throw new DllNotFoundException(
                                string.Format(
                                    Resource.SimulationCoreAnalysisUnknownReferenceLoadFailed, reference.FullName),
                                ex);
                        }
                        foreignReferences = true;
                        referenceInformation += reference.FullName + Environment.NewLine;
                        break;
                }
            }

            // TODO: Statische Variablen finden
            // TODO: Statische Konstruktoren finden - immernoch ein Problem?
            // TODO: Nested Types!

            foreach (var typeDefinition in module.Types)
            {
                // Suche nach statischen Variablen und bestimme so, ob der Spieler
                // ein globales Gedächtnis für seine Ameisen benutzt.
                staticVariables |= typeDefinition.Fields.Any(f => f.IsStatic);
                staticVariables |= typeDefinition.Properties.Any(p => (p.SetMethod?.IsStatic ?? false) || (p.GetMethod?.IsStatic ?? false));
            }

            // Gefundene KIs auf Regeln prüfen
            // Betrachte alle öffentlichen Typen in der Bibliothek.
            foreach (var exportedType in module.Types.Where(t => t.IsPublic && !t.IsInterface && !t.IsAbstract))
            {
                // Prüfe ob der Typ von der Klasse Ameise erbt.
                var exportedTypeDefinition = exportedType.Resolve();

                // Ameisenversion 1.6
                if (exportedTypeDefinition.IsSubclassOf(_coreAnt))
                {
                    // Leerer Spieler-Rumpf
                    int playerDefinitions = 0;

                    PlayerInfo player = new PlayerInfo();
                    player.SimulationVersion = PlayerSimulationVersions.Version_1_7;
                    player.ClassName = exportedType.FullName;

                    // Sprache ermitteln
                    foreach (PlayerLanguages sprache in _languageBases.Keys)
                    {
                        if (exportedTypeDefinition.IsSubclassOf(_languageBases[sprache]))
                        {
                            player.Language = sprache;
                            break;
                        }
                    }
                    // TODO: Vorgehensweise bei unbekannten Sprachen

                    // Attribute auslesen
                    foreach (var attribute in exportedTypeDefinition.CustomAttributes)
                    {
                        // Player-Attribut auslesen
                        switch (attribute.AttributeType.FullName)
                        {
                            case "AntMe.Deutsch.SpielerAttribute":
                                playerDefinitions++;
                                foreach (var property in attribute.Properties)
                                {
                                    switch (property.Name)
                                    {
                                        case "Volkname":
                                            player.ColonyName = (string)property.Argument.Value;
                                            break;
                                        case "Vorname":
                                            player.FirstName = (string)property.Argument.Value;
                                            break;
                                        case "Nachname":
                                            player.LastName = (string)property.Argument.Value;
                                            break;
                                    }
                                }
                                break;
                            case "AntMe.English.PlayerAttribute":
                                playerDefinitions++;
                                foreach (var property in attribute.Properties)
                                {
                                    switch (property.Name)
                                    {
                                        case "ColonyName":
                                            player.ColonyName = (string)property.Argument.Value;
                                            break;
                                        case "FirstName":
                                            player.FirstName = (string)property.Argument.Value;
                                            break;
                                        case "LastName":
                                            player.LastName = (string)property.Argument.Value;
                                            break;
                                    }
                                }
                                break;
                        }

                        // Caste-Attribut auslesen
                        CasteInfo caste = new CasteInfo();
                        switch (attribute.AttributeType.FullName)
                        {
                            case "AntMe.English.CasteAttribute":

                                // englische Kasten
                                foreach (var field in attribute.Fields)
                                {
                                    switch (field.Name)
                                    {
                                        case "Name":
                                            caste.Name = (string)field.Argument.Value;
                                            break;
                                        case "SpeedModifier":
                                            caste.Speed = (int)field.Argument.Value;
                                            break;
                                        case "RotationSpeedModifier":
                                            caste.RotationSpeed = (int)field.Argument.Value;
                                            break;
                                        case "LoadModifier":
                                            caste.Load = (int)field.Argument.Value;
                                            break;
                                        case "RangeModifier":
                                            caste.Range = (int)field.Argument.Value;
                                            break;
                                        case "ViewRangeModifier":
                                            caste.ViewRange = (int)field.Argument.Value;
                                            break;
                                        case "EnergyModifier":
                                            caste.Energy = (int)field.Argument.Value;
                                            break;
                                        case "AttackModifier":
                                            caste.Attack = (int)field.Argument.Value;
                                            break;
                                    }
                                }

                                // Bei Individualkasten zur Liste 
                                if (!caste.IsEmpty())
                                {
                                    player.Castes.Add(caste);
                                }
                                break;
                            case "AntMe.Deutsch.KasteAttribute":

                                // deutsche Kasten
                                foreach (var field in attribute.Fields)
                                {
                                    switch (field.Name)
                                    {
                                        case "Name":
                                            caste.Name = (string)field.Argument.Value;
                                            break;
                                        case "GeschwindigkeitModifikator":
                                            caste.Speed = (int)field.Argument.Value;
                                            break;
                                        case "DrehgeschwindigkeitModifikator":
                                            caste.RotationSpeed = (int)field.Argument.Value;
                                            break;
                                        case "LastModifikator":
                                            caste.Load = (int)field.Argument.Value;
                                            break;
                                        case "ReichweiteModifikator":
                                            caste.Range = (int)field.Argument.Value;
                                            break;
                                        case "SichtweiteModifikator":
                                            caste.ViewRange = (int)field.Argument.Value;
                                            break;
                                        case "EnergieModifikator":
                                            caste.Energy = (int)field.Argument.Value;
                                            break;
                                        case "AngriffModifikator":
                                            caste.Attack = (int)field.Argument.Value;
                                            break;
                                    }
                                }

                                // Bei Individualkasten zur Liste 
                                if (!caste.IsEmpty())
                                {
                                    player.Castes.Add(caste);
                                }
                                break;
                        }

                        // Access-Attributes
                        bool isAccessAttribute = false;
                        // TODO: Request-Infos lokalisieren
                        switch (attribute.AttributeType.FullName)
                        {
                            case "AntMe.Deutsch.DateizugriffAttribute":
                                player.RequestFileAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Dateizugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.Deutsch.DatenbankzugriffAttribute":
                                player.RequestDatabaseAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Datenbankzugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.Deutsch.FensterzugriffAttribute":
                                player.RequestUserInterfaceAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Fensterzugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.Deutsch.NetzwerkzugriffAttribute":
                                player.RequestNetworkAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Netzwerkzugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.English.FileAccessAttribute":
                                player.RequestFileAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Dateizugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.English.DatabaseAccessAttribute":
                                player.RequestDatabaseAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Datenbankzugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.English.UserinterfaceAccessAttribute":
                                player.RequestUserInterfaceAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Fensterzugriff:" + Environment.NewLine;
                                break;
                            case "AntMe.English.NetworkAccessAttribute":
                                player.RequestNetworkAccess = true;
                                isAccessAttribute = true;
                                player.RequestInformation += "Netzwerkzugriff:" + Environment.NewLine;
                                break;
                        }

                        if (isAccessAttribute)
                        {
                            foreach (var field in attribute.Fields)
                            {
                                switch (field.Name)
                                {
                                    case "Beschreibung":
                                        player.RequestInformation += field.Argument.Value +
                                            Environment.NewLine + Environment.NewLine;
                                        break;
                                    case "Description":
                                        player.RequestInformation += field.Argument.Value +
                                            Environment.NewLine + Environment.NewLine;
                                        break;
                                }
                            }
                        }
                    }

                    // Prüfe ob die Klasse regelkonform ist
                    player.Static = staticVariables;
                    player.RequestReferences = foreignReferences;
                    if (foreignReferences)
                    {
                        player.RequestInformation += Resource.SimulationCoreAnalysisForeignRefInfotext +
                                                     Environment.NewLine + referenceInformation + Environment.NewLine +
                                                     Environment.NewLine;
                    }

                    switch (playerDefinitions)
                    {
                        case 0:
                            // Kein Spielerattribut gefunden
                            throw new RuleViolationException(
                                string.Format(
                                    Resource.SimulationCoreAnalysisNoPlayerAttribute,
                                    player.ClassName));
                        case 1:
                            if (checkRules)
                            {
                                player.RuleCheck();
                            }
                            players.Add(player);
                            break;
                        default:
                            throw new RuleViolationException(
                                string.Format(
                                    Resource.SimulationCoreAnalysisTooManyPlayerAttributes,
                                    player.ClassName));
                    }
                }

                #region Erkennung älterer KI-Versionen

                // Ältere Versionen
                else if (exportedTypeDefinition.BaseType.Name == "AntMe.Ameise")
                {
                    // Leerer Spieler-Rumpf
                    int playerDefinitions = 0;

                    PlayerInfo player = new PlayerInfo();
                    player.SimulationVersion = PlayerSimulationVersions.Version_1_1;
                    player.ClassName = exportedTypeDefinition.FullName;

                    // Veraltete Version (1.1 oder 1.5)
                    // TODO: Prüfen
                    if (exportedTypeDefinition.Methods.Any(MethodDefinition => MethodDefinition.Name == "RiechtFreund"))
                    {
                        player.SimulationVersion = PlayerSimulationVersions.Version_1_5;
                    }

                    // Attribute auslesen
                    foreach (var attribute in exportedTypeDefinition.CustomAttributes)
                    {
                        // Spieler-Attribut auslesen
                        if (attribute.AttributeType.FullName == "AntMe.SpielerAttribute")
                        {
                            playerDefinitions++;
                            foreach (var property in attribute.Properties)
                            {
                                switch (property.Name)
                                {
                                    case "Name":
                                        player.ColonyName = (string)property.Argument.Value;
                                        break;
                                    case "Vorname":
                                        player.FirstName = (string)property.Argument.Value;
                                        break;
                                    case "Nachname":
                                        player.LastName = (string)property.Argument.Value;
                                        break;
                                }
                            }
                            break;
                        }

                        // Typ-Attribut auslesen
                        if (attribute.AttributeType.FullName == "AntMe.TypAttribute")
                        {
                            CasteInfo caste = new CasteInfo();
                            foreach (var field in attribute.Fields)
                            {
                                switch (field.Name)
                                {
                                    case "Name":
                                        caste.Name = (string)field.Argument.Value;
                                        break;
                                    case "GeschwindigkeitModifikator":
                                        caste.Speed = (int)field.Argument.Value;
                                        break;
                                    case "DrehgeschwindigkeitModifikator":
                                        caste.RotationSpeed = (int)field.Argument.Value;
                                        break;
                                    case "LastModifikator":
                                        caste.Load = (int)field.Argument.Value;
                                        break;
                                    case "ReichweiteModifikator":
                                        caste.Range = (int)field.Argument.Value;
                                        break;
                                    case "SichtweiteModifikator":
                                        caste.ViewRange = (int)field.Argument.Value;
                                        break;
                                    case "EnergieModifikator":
                                        caste.Energy = (int)field.Argument.Value;
                                        break;
                                    case "AngriffModifikator":
                                        caste.Attack = (int)field.Argument.Value;
                                        break;
                                }
                            }

                            // Bei Individualtypen zur Liste 
                            if (!caste.IsEmpty())
                            {
                                player.Castes.Add(caste);
                            }
                            break;
                        }
                    }

                    // Prüfe ob die Klasse regelkonform ist
                    player.Static = staticVariables;
                    switch (playerDefinitions)
                    {
                        case 0:
                            // Kein Spielerattribut gefunden
                            throw new RuleViolationException(
                                string.Format(
                                    Resource.SimulationCoreAnalysisNoPlayerAttribute,
                                    player.ClassName));
                        case 1:
                            if (checkRules)
                            {
                                player.RuleCheck();
                            }
                            players.Add(player);
                            break;
                        default:
                            throw new RuleViolationException(
                                string.Format(
                                    Resource.SimulationCoreAnalysisTooManyPlayerAttributes,
                                    player.ClassName));
                    }
                }

                #endregion
            }

            return players;
        }

        #endregion
    }
}