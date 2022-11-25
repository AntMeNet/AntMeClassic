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
        private TypeDefinition coreAnt;

        /// <summary>
        /// List of all Ant-BaseClasses in every known language.
        /// </summary>
        private Dictionary<PlayerLanguages, TypeDefinition> languageBases;

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
        /// Analyse given binary file for <see cref="PlayerInfo"/> and returns a list of found <see cref="PlayerInfo"/>
        /// </summary>
        /// <param name="file">AI file as binary file dump.</param>
        /// <param name="checkRules">true to check player rules</param>
        /// <returns>List of found <see cref="PlayerInfo"/> from given binary file</returns>
        public List<PlayerInfo> Analyse(byte[] file, bool checkRules)
        {
            // load base-class from Simulation-Core.#     
            ModuleDefinition simulation = ModuleDefinition.ReadModule(Assembly.GetExecutingAssembly().Modules.First().FullyQualifiedName);
            coreAnt = simulation.GetType("AntMe.Simulation.CoreAnt");
            //coreAnt = simulation.GetType("AntMe.Simulation.CoreAnt");

            // load all base-classes of different languages
            languageBases = new Dictionary<PlayerLanguages, TypeDefinition>();
            languageBases.Add(
                PlayerLanguages.Deutsch,
                simulation.GetType("AntMe.Deutsch.Basisameise"));
            languageBases.Add(
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
        /// <param name="checkRules">true for checking player rules with analyseAssembly</param>
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
                        byte[] coreKey = new AssemblyName(typeof(object).Assembly.FullName).GetPublicKeyToken();
                        byte[] referenceKey = reference.PublicKeyToken;
                        if (coreKey[0] != referenceKey[0] ||
                            coreKey[1] != referenceKey[1] ||
                            coreKey[2] != referenceKey[2] ||
                            coreKey[3] != referenceKey[3] ||
                            coreKey[4] != referenceKey[4] ||
                            coreKey[5] != referenceKey[5] ||
                            coreKey[6] != referenceKey[6] ||
                            coreKey[7] != referenceKey[7])
                        {
                            throw new RuleViolationException(Resource.SimulationCoreAnalysisCheatWithFxFake);
                        }
                        break;
                    case "Microsoft.VisualBasic":
                        // TODO: figure out how to handle this
                        break;
                    case "System":
                        // TODO: figure out how to handle this
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

            // TODO: find static attributes
            // TODO: find static constructor, still a problem?
            // TODO: nested types!

            foreach (var typeDefinition in module.Types)
            {
                // search for static attributes for determination
                // whether the player uses global memory for their ants
                staticVariables |= typeDefinition.Fields.Any(f => f.IsStatic);
                staticVariables |= typeDefinition.Properties.Any(p => (p.SetMethod?.IsStatic ?? false) || (p.GetMethod?.IsStatic ?? false));
            }

            // check AIs against rule set
            // consider all public types in library
            foreach (var exportedType in module.Types.Where(t => t.IsPublic && !t.IsInterface && !t.IsAbstract))
            {
                // check if the type inherts from class ant
                var exportedTypeDefinition = exportedType.Resolve();

                // antme version 1.6
                if (exportedTypeDefinition.IsSubclassOf(coreAnt))
                {
                    // player defintion is empty
                    int playerDefinitions = 0;

                    PlayerInfo player = new PlayerInfo();
                    player.SimulationVersion = PlayerSimulationVersions.Version_1_7;
                    player.ClassName = exportedType.FullName;

                    // determine language
                    foreach (PlayerLanguages language in languageBases.Keys)
                    {
                        if (exportedTypeDefinition.IsSubclassOf(languageBases[language]))
                        {
                            player.Language = language;
                            break;
                        }
                    }
                    // TODO: handling of unknown languages

                    // read attributes
                    foreach (var attribute in exportedTypeDefinition.CustomAttributes)
                    {
                        // read player attributes
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

                        // read caste attributes
                        CasteInfo caste = new CasteInfo();
                        switch (attribute.AttributeType.FullName)
                        {
                            case "AntMe.English.CasteAttribute":

                                // english Castes
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

                                // new castes of the player will be added to list
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

                                // new castes of the player will be added to list
                                if (!caste.IsEmpty())
                                {
                                    player.Castes.Add(caste);
                                }
                                break;
                        }

                        // Access-Attributes
                        bool isAccessAttribute = false;
                        // TODO: localize request information
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

                    // check class against rule set
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
                            // no player attribute found
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

                #region handling of old AIs

                // old AntMe version 
                else if (exportedTypeDefinition.BaseType.Name == "AntMe.Ameise")
                {
                    // no player definiton
                    int playerDefinitions = 0;

                    PlayerInfo player = new PlayerInfo();
                    player.SimulationVersion = PlayerSimulationVersions.Version_1_1;
                    player.ClassName = exportedTypeDefinition.FullName;

                    // old versions 1.1 to 1.5
                    // TODO: Check old versions 1.1 to 1.5
                    if (exportedTypeDefinition.Methods.Any(MethodDefinition => MethodDefinition.Name == "RiechtFreund"))
                    {
                        player.SimulationVersion = PlayerSimulationVersions.Version_1_5;
                    }

                    // read attributes
                    foreach (var attribute in exportedTypeDefinition.CustomAttributes)
                    {
                        // read player attribute
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

                        // read type attributes
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

                            // new castes of the player will be added to list
                            if (!caste.IsEmpty())
                            {
                                player.Castes.Add(caste);
                            }
                            break;
                        }
                    }

                    // check class against rule set
                    player.Static = staticVariables;
                    switch (playerDefinitions)
                    {
                        case 0:
                            // no player attribute found
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