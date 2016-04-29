using System;
using System.Collections.Generic;
using AntMe.SharedComponents.States;

namespace AntMe.Simulation
{
    /// <summary>
    /// Represents a complete encapsulated simulation-core.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    public sealed class Simulator : IDisposable
    {
        #region private variables

        private readonly SimulatorConfiguration configuration;

        private int currentLoop;
        private int currentRound;

        private long loopTime;
        private long totalTime;
        private SimulatorHostState lastHostState;
        private readonly Dictionary<Guid, long> totalPlayerTime;
        private SimulatorProxy proxy;
        private long roundTime;

        private SimulatorState state;
        private SimulationState lastSimulationState;

        #endregion

        #region Constructor and Init

        /// <summary>
        /// Creates an Instance of simulator. Should be called only from factory.
        /// </summary>
        /// <param name="configuration">configuration</param>
        public Simulator(SimulatorConfiguration configuration)
        {
            // Leere Konfiguration prüfen
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration", Resource.SimulationCoreFactoryConfigIsNull);
            }

            // Copy config
            this.configuration = (SimulatorConfiguration)configuration.Clone();

            // Reload PlayerInfo
            if (this.configuration.Teams != null)
            {
                foreach (TeamInfo team in this.configuration.Teams)
                {
                    if (team.Player != null)
                    {
                        for (int i = 0; i < team.Player.Count; i++)
                        {
                            PlayerInfo player = team.Player[i];
                            if (player is PlayerInfoFiledump)
                            {
                                team.Player[i] =
                                    AiAnalysis.FindPlayerInformation(
                                        ((PlayerInfoFiledump)player).File, player.ClassName);
                            }
                            else if (player is PlayerInfoFilename)
                            {
                                team.Player[i] =
                                    AiAnalysis.FindPlayerInformation(
                                        ((PlayerInfoFilename)player).File, player.ClassName);
                            }
                            else
                            {
                                // TODO: Throw unknown type...
                            }
                            team.Player[i].Guid = player.Guid;
                        }
                    }
                }
            }

            // Regelprüfung der Konfig anwerfen
            configuration.Rulecheck();


            // Init values
            currentLoop = 0;
            currentRound = 0;
            roundTime = 0;
            loopTime = 0;
            totalTime = 0;

            // Reset
            totalPlayerTime = new Dictionary<Guid, long>();
            for (int i = 0; i < configuration.Teams.Count; i++)
            {
                for (int j = 0; j < configuration.Teams[i].Player.Count; j++)
                {
                    Guid guid = configuration.Teams[i].Player[j].Guid;
                    if (!totalPlayerTime.ContainsKey(guid))
                        totalPlayerTime.Add(guid, 0);
                }
            }

            // Set initial state
            state = SimulatorState.Ready;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Calculates the next step and deliver through parameter.
        /// </summary>
        /// <param name="simulationState">empty <see cref="SimulationState"/></param>
        public void Step(ref SimulationState simulationState)
        {
            lastSimulationState = simulationState;
            switch (state)
            {
                case SimulatorState.Ready:
                case SimulatorState.Simulating:

                    // Create proxy
                    if (proxy == null)
                    {
                        proxy = new SimulatorProxy();
                        proxy.Init(configuration);
                        currentLoop++;
                        currentRound = 0;
                        loopTime = 0;
                        for (int i = 0; i < configuration.Teams.Count; i++)
                        {
                            for (int j = 0; j < configuration.Teams[i].Player.Count; j++)
                            {
                                totalPlayerTime[configuration.Teams[i].Player[j].Guid] = 0;
                            }
                        }
                        state = SimulatorState.Simulating;
                    }

                    // Calculate step
                    currentRound++;
                    lastHostState = proxy.Step(ref lastSimulationState);
                    simulationState = lastSimulationState;

                    // Calculate times
                    roundTime = lastHostState.ElapsedRoundTime;
                    loopTime += lastHostState.ElapsedRoundTime;
                    totalTime += lastHostState.ElapsedRoundTime;
                    for (int i = 0; i < configuration.Teams.Count; i++)
                    {
                        for (int j = 0; j < configuration.Teams[i].Player.Count; j++)
                        {
                            // TODO: Fix Dictionary-Problem with time-list
                            Guid guid = configuration.Teams[i].Player[j].Guid;
                            totalPlayerTime[guid] += lastHostState.ElapsedPlayerTimes[guid];
                        }
                    }

                    // After one loop, unload appdomain
                    if (currentRound >= configuration.RoundCount)
                    {
                        proxy.Unload();
                        proxy = null;
                        GC.Collect();
                    }

                    // Mark Simulator as finished after all loops
                    if (currentRound >= configuration.RoundCount &&
                        currentLoop >= configuration.LoopCount)
                    {
                        state = SimulatorState.Finished;
                    }
                    break;

                case SimulatorState.Finished:

                    // Throw exception, if step was called on a finished simulator
                    throw new InvalidOperationException(
                        Resource.SimulationCoreSimulatorRestartFailed);
            }
            lastSimulationState = null;
        }

        /// <summary>
        /// Disposes open resources.
        /// </summary>
        ~Simulator()
        {
            Unload();
        }

        /// <summary>
        /// Disposes open resources.
        /// </summary>
        public void Dispose()
        {
            Unload();
        }

        /// <summary>
        /// Unloads simulator.
        /// </summary>
        public void Unload()
        {
            if (proxy != null)
            {
                proxy.Unload();
                proxy = null;
                GC.Collect();
            }
            state = SimulatorState.Finished;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current simulator-state.
        /// </summary>
        public SimulatorState State
        {
            get { return state; }
        }

        /// <summary>
        /// Gets the current round of simulation.
        /// </summary>
        public int CurrentRound
        {
            get { return currentRound; }
        }

        /// <summary>
        /// Gets the current loop of simulation.
        /// </summary>
        public int CurrentLoop
        {
            get { return currentLoop; }
        }

        /// <summary>
        /// Gets the number of ticks the last round was running.
        /// </summary>
        public long RoundTime
        {
            get
            {
                // TODO: Deliver a bether timeformat.
                return roundTime;
            }
        }

        /// <summary>
        /// Gets the total number of ticks the current Loop is running.
        /// </summary>
        public long LoopTime { get { return loopTime; } }

        /// <summary>
        /// Gets the total number of ticks the current simulator is working.
        /// </summary>
        public long TotalTime { get { return totalTime; } }

        /// <summary>
        /// Gets a the total time of ticks a player needed in the last round.
        /// </summary>
        public Dictionary<Guid, long> PlayerRoundTimes
        {
            get
            {
                if (lastHostState != null)
                {
                    return lastHostState.ElapsedPlayerTimes;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the total number of ticks a player needed in the whole loop till now.
        /// </summary>
        public Dictionary<Guid, long> PlayerLoopTimes
        {
            // TODO: Deliver a bether format.
            get { return totalPlayerTime; }
        }

        #endregion
    }
}