using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace AntMe.Simulation
{
    /// <summary>
    /// Class, to host an simulation-Environment inside an <see cref="AppDomain"/>.
    /// </summary>
    internal sealed class SimulatorHost : MarshalByRefObject
    {
        #region internal Variables

        private Area currentArea;
        private PlayerInfo currentPlayer;
        private Exception exception;
        private SimulatorConfiguration configuration;
        private Dictionary<Guid, long> playerTimes;
        private SimulatorHostState lastHostState;
        private SimulationState lastSimulationState;

        private readonly Stopwatch playerWatch = new Stopwatch();
        private readonly Stopwatch stepWatch = new Stopwatch();
        private SimulationEnvironment environment;

        #endregion

        #region Constructor and Init

        /// <summary>
        /// Initialize the simulation-environment.
        /// </summary>
        /// <param name="config">configuration</param>
        public bool Init(SimulatorConfiguration config)
        {
            // Prepare values
            configuration = config;
            SimulationSettings.SetCustomSettings(config.Settings);
            environment = null;

            // Prepare time-counting
            playerTimes = new Dictionary<Guid, long>();
            currentPlayer = null;
            currentArea = Area.Unknown;

            // Load Playerfiles
            foreach (TeamInfo team in configuration.Teams)
            {
                foreach (PlayerInfo spieler in team.Player)
                {
                    if (spieler is PlayerInfoFiledump)
                    {
                        // Try, to load filedump
                        try
                        {
                            spieler.assembly = Assembly.Load(((PlayerInfoFiledump)spieler).File);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                            return false;
                        }
                    }
                    else if (spieler is PlayerInfoFilename)
                    {
                        // Try, to load filename
                        try
                        {
                            spieler.assembly = Assembly.LoadFile(((PlayerInfoFilename)spieler).File);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                            return false;
                        }
                    }
                    else
                    {
                        exception =
                            new InvalidOperationException(
                                Resource.SimulationCoreHostWrongPlayerInfo);
                        return false;
                    }

                    // Add player to counter-list
                    // TODO: Need another key for times
                    // playerTimes.Add(spieler, 0);
                }
            }

            // Init environment
            try
            {
                environment = new SimulationEnvironment();
                environment.AreaChange += umgebung_Verantwortungswechsel;
                environment.Init(configuration);
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }

            // Everything nice...
            return true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes one single step in simulation and returns hostState.
        /// </summary>
        /// <returns>Summery of the executed simulationStep</returns>
        public SimulatorHostState Step(ref SimulationState simulationState)
        {
            if (environment == null)
            {
                throw new InvalidOperationException(Resource.SimulationCoreHostEnvironmentNotInit);
            }

            // Reset of times
            stepWatch.Reset();
            foreach (TeamInfo team in configuration.Teams)
                foreach (PlayerInfo spieler in team.Player)
                    playerTimes[spieler.Guid] = 0;

            // Init Step-Thread
            exception = null;
            lastSimulationState = simulationState;
            Thread stepThread = new Thread(step);
            stepWatch.Start();
            stepThread.Start();
            if (configuration.IgnoreTimeouts)
            {
                // Wait without any timeout
                stepThread.Join();
            }
            else
            {
                // Wait for thread with timeout
                if (!stepThread.Join(configuration.RoundTimeout))
                {
                    throw new TimeoutException(Resource.SimulationCoreHostRoundTimeout);
                }
            }
            stepWatch.Stop();

            // Bei Exceptions null zurück liefern, um Fehler zu signalisieren
            if (exception != null)
            {
                return null;
            }

            // Add player-times
            lastHostState.ElapsedRoundTime = stepWatch.ElapsedTicks;
            for (int i = 0; i < configuration.Teams.Count; i++)
            {
                for (int j = 0; j < configuration.Teams[i].Player.Count; j++)
                {
                    Guid guid = configuration.Teams[i].Player[j].Guid;
                    if (!lastHostState.ElapsedPlayerTimes.ContainsKey(guid))
                        lastHostState.ElapsedPlayerTimes.Add(guid, playerTimes[guid]);
                }
            }

            // deliver host-state
            return lastHostState;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the last thrown exception.
        /// </summary>
        public Exception Exception
        {
            get { return exception; }
        }

        #endregion

        #region Helper-Methods

        /// <summary>
        /// Internal step-Method to calculate the next step
        /// </summary>
        private void step()
        {
            lastHostState = new SimulatorHostState();
            try
            {
                environment.Step(lastSimulationState);
                lastSimulationState.TotalRounds = (ushort)configuration.RoundCount;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        #endregion

        #region Eventhandler

        private void umgebung_Verantwortungswechsel(object sender, AreaChangeEventArgs e)
        {
            // Aktuellen Timer stoppen
            if (currentPlayer != e.Player && currentPlayer != null)
            {
                playerWatch.Stop();
                // TODO: need another key
                if (!playerTimes.ContainsKey(currentPlayer.Guid))
                    playerTimes.Add(currentPlayer.Guid, 0);
                playerTimes[currentPlayer.Guid] += playerWatch.ElapsedTicks;
                currentPlayer = null;
                currentArea = Area.Unknown;
            }

            // Neuen Timer starten
            if (e.Player != null)
            {
                currentPlayer = e.Player;
                currentArea = e.Area;
                playerWatch.Reset();
                playerWatch.Start();
            }
        }

        #endregion
    }
}