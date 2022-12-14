using AntMe.SharedComponents.States;
using System;
using System.Configuration;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace AntMe.Simulation
{
    /// <summary>
    /// Proxy class to host an AppDomain for the encapsulated simulation.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    internal sealed class SimulatorProxy
    {
        #region internal Variables

        /// <summary>
        /// Appdomain to host the simulation in.
        /// </summary>
        private AppDomain appDomain;

        /// <summary>
        /// Proxy for the simulationHost inside the appDomain.
        /// </summary>
        private SimulatorHost host;

        #endregion

        #region public Methods

        /// <summary>
        /// Initialization of the simulation
        /// </summary>
        /// <param name="configuration">configuration of the simulation</param>
        public void Init(SimulatorConfiguration configuration)
        {
            // unload possible appdomains.
            Unload();

            // load involved ai's
            for (int team = 0; team < configuration.Teams.Count; team++)
            {
                for (int i = 0; i < configuration.Teams[team].Player.Count; i++)
                {

                    // save external Guid information
                    Guid guid = configuration.Teams[team].Player[i].Guid;

                    if (configuration.Teams[team].Player[i] is PlayerInfoFiledump)
                    {
                        // use filedump
                        configuration.Teams[team].Player[i] =
                            AiAnalysis.FindPlayerInformation(
                                ((PlayerInfoFiledump)configuration.Teams[team].Player[i]).File,
                                configuration.Teams[team].Player[i].ClassName);
                    }
                    else if (configuration.Teams[team].Player[i] is PlayerInfoFilename)
                    {
                        // use filename
                        configuration.Teams[team].Player[i] =
                            AiAnalysis.FindPlayerInformation(
                                ((PlayerInfoFilename)configuration.Teams[team].Player[i]).File,
                                configuration.Teams[team].Player[i].ClassName);
                    }
                    else
                    {
                        // not supported PlayerInfo-type
                        throw new InvalidOperationException(
                            Resource.SimulationCoreProxyWrongPlayerInfo);
                    }

                    // save back external Guid
                    configuration.Teams[team].Player[i].Guid = guid;
                }
            }

            // setup appDomain
            AppDomainSetup setup = new AppDomainSetup();

            // Base Path for references
            string applicationBase = AppDomain.CurrentDomain.RelativeSearchPath;
            if (string.IsNullOrEmpty(applicationBase))
                applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            setup.ApplicationBase = applicationBase;
            setup.ShadowCopyDirectories = AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories;
            setup.ShadowCopyFiles = "false";
            setup.PrivateBinPath = "";

            // setup access-rights for appdomain
            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));

            bool IoAccess = false;
            bool UiAccess = false;
            bool DbAccess = false;
            bool NetAccess = false;

            // grant access to the needed ai-files
            foreach (TeamInfo team in configuration.Teams)
            {
                foreach (PlayerInfo info in team.Player)
                {
                    if (info is PlayerInfoFilename)
                    {
                        permissions.AddPermission(
                            new FileIOPermission(
                                FileIOPermissionAccess.Read |
                                FileIOPermissionAccess.PathDiscovery,
                                ((PlayerInfoFilename)info).File));
                    }
                    if (info.RequestDatabaseAccess) DbAccess = true;
                    if (info.RequestFileAccess) IoAccess = true;
                    if (info.RequestNetworkAccess) NetAccess = true;
                    if (info.RequestUserInterfaceAccess) UiAccess = true;
                }
            }

            // grant special rights
            if (IoAccess)
            {
                if (configuration.AllowFileAccess)
                {
                    permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                }
                else
                {
                    throw new ConfigurationErrorsException(Resource.SimulationCoreRightsConflictIo);
                }
            }

            if (UiAccess)
            {
                if (configuration.AllowUserinterfaceAccess)
                {
                    permissions.AddPermission(new UIPermission(PermissionState.Unrestricted));
                }
                else
                {
                    throw new ConfigurationErrorsException(Resource.SimulationCoreRightsConflictUi);
                }
            }

            if (DbAccess)
            {
                if (configuration.AllowDatabaseAccess)
                {
                    // TODO: grant rights
                }
                else
                {
                    throw new ConfigurationErrorsException(Resource.SimulationCoreRightsConflictDb);
                }
            }

            if (NetAccess)
            {
                if (configuration.AllowNetworkAccess)
                {
                    permissions.AddPermission(new System.Net.WebPermission(PermissionState.Unrestricted));
                    permissions.AddPermission(new System.Net.SocketPermission(PermissionState.Unrestricted));
                    permissions.AddPermission(new System.Net.DnsPermission(PermissionState.Unrestricted));
                }
                else
                {
                    throw new ConfigurationErrorsException(Resource.SimulationCoreRightsConflictNet);
                }
            }

            // create appdomain and load simulation-host
            appDomain = AppDomain.CreateDomain("Simulation", AppDomain.CurrentDomain.Evidence, setup, permissions);

            host =
                (SimulatorHost)
                appDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName, "AntMe.Simulation.SimulatorHost");

            // initialize host
            if (!host.Init(configuration))
            {
                // got some exceptions - unload appdomain und throw exception
                Exception ex = host.Exception;
                Unload();
                throw ex;
            }
        }

        /// <summary>
        /// Executes one single step in simulation and returns summary of the executed simulation step 
        /// </summary>
        /// <param name="simulationState">simulation state</param>
        /// <returns>Summery of the executed simulationStep.</returns>
        public SimulatorHostState Step(ref SimulationState simulationState)
        {
            // check, of proxy is still usable
            if (host == null)
            {
                throw new InvalidOperationException(Resource.SimulationCoreProxyUnloaded);
            }

            // execute step inside the appdomain via host
            SimulatorHostState state = host.Step(ref simulationState);

            // in case of an exception
            if (state == null)
            {
                throw host.Exception;
            }

            // return success
            return state;
        }

        /// <summary>
        /// Unloads the appdomain, if needed and reset the proxy.
        /// </summary>
        public void Unload()
        {
            if (appDomain != null)
            {
                AppDomain.Unload(appDomain);
                appDomain = null;
                host = null;
            }
        }

        #endregion
    }
}