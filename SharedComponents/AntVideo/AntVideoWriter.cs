using System.Collections.Generic;
using System.IO;

using AntMe.SharedComponents.AntVideo.Block;
using AntMe.SharedComponents.States;
using System.IO.Compression;
using System;

namespace AntMe.SharedComponents.AntVideo
{
    /// <summary>
    /// Class, to stream some simulation state as ant-video-stream.
    /// </summary>
    public sealed class AntVideoWriter : IDisposable
    {

        #region local variables

        private readonly Dictionary<int, Anthill> anthillList;
        private readonly Dictionary<int, Ant> antList;
        private readonly Dictionary<int, Bug> bugList;
        private readonly Dictionary<int, Fruit> fruitList;
        private readonly Dictionary<int, Marker> markerList;
        private readonly Dictionary<int, Dictionary<int, Caste>> casteList;
        private readonly Dictionary<int, Sugar> sugarList;
        private readonly Dictionary<int, Team> teamList;
        private readonly Dictionary<int, Colony> colonyList;

        private readonly Serializer serializer;
        private Frame frame;

        #endregion

        /// <summary>
        /// Creates a new instance of ant-video-writer.
        /// </summary>
        /// <param name="outputStream">output-stream</param>
        public AntVideoWriter(Stream outputStream)
        {

            // check for stream
            if (outputStream == null)
                throw new ArgumentNullException("outputStream");

            // check for readable stream
            if (!outputStream.CanWrite || !outputStream.CanSeek)
                throw new InvalidOperationException("Stream is not writeable");

            teamList = new Dictionary<int, Team>();
            colonyList = new Dictionary<int, Colony>();
            casteList = new Dictionary<int, Dictionary<int, Caste>>();
            anthillList = new Dictionary<int, Anthill>();
            antList = new Dictionary<int, Ant>();
            markerList = new Dictionary<int, Marker>();
            bugList = new Dictionary<int, Bug>();
            sugarList = new Dictionary<int, Sugar>();
            fruitList = new Dictionary<int, Fruit>();

            serializer = new Serializer(outputStream, false, true);
            serializer.WriteHello();
            serializer.Write(BlockType.StreamStart);
        }

        /// <summary>
        /// Writes a new state to the stream.
        /// </summary>
        /// <param name="state">New state</param>
        public void Write(SimulationState state)
        {
            serializer.Write(BlockType.FrameStart);
            int[] keys;

            #region Framestart

            // The first call creates the frame
            if (frame == null)
            {
                // Create new frame
                frame = new Frame(state);
                serializer.Write(BlockType.Frame, frame);
            }
            else
            {
                // Send frame-update
                FrameUpdate update = frame.GenerateUpdate(state);
                if (update != null)
                {
                    serializer.Write(BlockType.FrameUpdate, update);
                }
            }

            #endregion

            #region Teams and ColonyStates

            #region ant-reset

            // reset alive-flag
            foreach (Ant ant in antList.Values)
            {
                ant.IsAlive = false;
            }

            #endregion

            #region marker-reset

            // reset alive-flag
            foreach (Marker marker in markerList.Values)
            {
                marker.IsAlive = false;
            }

            #endregion

            // Teams are static and need no update

            // enumerate all teams
            foreach (TeamState teamState in state.TeamStates)
            {

                // Check, if team is known
                if (teamList.ContainsKey(teamState.Id))
                {
                    // No Teamupdate needed
                }
                else
                {
                    Team team = new Team(teamState);
                    serializer.Write(BlockType.Team, team);
                    teamList.Add(teamState.Id, team);
                }

                // ColonyStates are static and need no update

                // enumerate all colonies
                foreach (ColonyState colonyState in teamState.ColonyStates)
                {
                    // Check, if colony is known
                    if (colonyList.ContainsKey(colonyState.Id))
                    {
                        // colony-update
                        ColonyUpdate update = colonyList[colonyState.Id].GenerateUpdate(colonyState);
                        if (update != null)
                        {
                            serializer.Write(BlockType.ColonyUpdate, update);
                        }
                        colonyList[colonyState.Id].Interpolate();
                    }
                    else
                    {
                        // new colony
                        Colony colony = new Colony(colonyState, teamState.Id);
                        serializer.Write(BlockType.Colony, colony);
                        colonyList.Add(colonyState.Id, colony);
                        casteList.Add(colonyState.Id, new Dictionary<int, Caste>());

                        #region Castes

                        // Casts are static and need no update

                        Dictionary<int, Caste> castes = casteList[colonyState.Id];

                        // enumerate casts
                        for (ushort i = 0; i < colonyState.CasteStates.Count; i++)
                        {
                            // Check, if caste is known
                            if (!castes.ContainsKey(i))
                            {
                                // add caste
                                Caste caste = new Caste(colonyState.CasteStates[i]);
                                serializer.Write(BlockType.Caste, caste);
                            }
                        }

                        #endregion

                        #region Anthills

                        // Anthills are static and need no update

                        // enumerate anthills
                        foreach (AnthillState anthill in colonyState.AnthillStates)
                        {
                            if (!anthillList.ContainsKey(anthill.Id))
                            {
                                Anthill hill = new Anthill(anthill);
                                serializer.Write(BlockType.Anthill, hill);
                                anthillList.Add(anthill.Id, hill);
                            }
                        }

                        #endregion
                    }

                    #region Ants

                    // enumerate ants
                    foreach (AntState antState in colonyState.AntStates)
                    {
                        // Check, if ant is known
                        if (antList.ContainsKey(antState.Id))
                        {
                            // ant-update
                            AntUpdate update = antList[antState.Id].GenerateUpdate(antState);
                            if (update != null)
                            {
                                serializer.Write(BlockType.AntUpdate, update);
                            }
                            antList[antState.Id].Interpolate();
                        }
                        else
                        {
                            // create ant
                            Ant ant = new Ant(antState);
                            serializer.Write(BlockType.Ant, ant);
                            antList.Add(ant.Id, ant);
                        }

                        antList[antState.Id].IsAlive = true;
                    }

                    #endregion

                    #region Marker

                    // enumerate marker
                    foreach (MarkerState markerState in colonyState.MarkerStates)
                    {
                        // Check, if marker is known
                        if (markerList.ContainsKey(markerState.Id))
                        {
                            // marker-update
                            MarkerUpdate update = markerList[markerState.Id].GenerateUpdate(markerState);
                            if (update != null)
                            {
                                serializer.Write(BlockType.MarkerUpdate, update);
                            }
                            markerList[markerState.Id].Interpolate();
                        }
                        else
                        {
                            // create marker
                            Marker marker = new Marker(markerState);
                            serializer.Write(BlockType.Marker, marker);
                            markerList.Add(markerState.Id, marker);
                        }

                        markerList[markerState.Id].IsAlive = true;
                    }

                    #endregion
                }
            }

            #region Ant-Cleanup

            // remove dead ants
            keys = new int[antList.Keys.Count];
            antList.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!antList[keys[i]].IsAlive)
                {
                    serializer.Write(BlockType.AntLost, new Lost(keys[i]));
                    antList.Remove(keys[i]);
                }
            }

            #endregion

            #region Marker-Cleanup

            // remove dead marker
            keys = new int[markerList.Keys.Count];
            markerList.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!markerList[keys[i]].IsAlive)
                {
                    serializer.Write(BlockType.MarkerLost, new Lost(keys[i]));
                    markerList.Remove(keys[i]);
                }
            }

            #endregion

            #endregion

            #region Fruit

            // reset alive-flag
            foreach (Fruit fruit in fruitList.Values)
            {
                fruit.IsAlive = false;
            }

            // enumerate fruit
            foreach (FruitState fruitState in state.FruitStates)
            {

                // Check, if fruit is known
                if (fruitList.ContainsKey(fruitState.Id))
                {

                    // fruit-update
                    FruitUpdate update = fruitList[fruitState.Id].GenerateUpdate(fruitState);
                    if (update != null)
                    {
                        serializer.Write(BlockType.FruitUpdate, update);
                    }
                    fruitList[fruitState.Id].Interpolate();
                }
                else
                {

                    // create fruit
                    Fruit fruit = new Fruit(fruitState);
                    serializer.Write(BlockType.Fruit, fruit);
                    fruitList.Add(fruitState.Id, fruit);
                }

                fruitList[fruitState.Id].IsAlive = true;
            }

            // remove dead fruits
            keys = new int[fruitList.Keys.Count];
            fruitList.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!fruitList[keys[i]].IsAlive)
                {
                    serializer.Write(BlockType.FruitLost, new Lost(keys[i]));
                    fruitList.Remove(keys[i]);
                }
            }

            #endregion

            #region Sugar

            // reset alive-flag
            foreach (Sugar sugar in sugarList.Values)
            {
                sugar.IsAlive = false;
            }

            // enumerate sugar
            foreach (SugarState sugarState in state.SugarStates)
            {

                // Check, if sugar is known
                if (sugarList.ContainsKey(sugarState.Id))
                {

                    // sugar-update
                    SugarUpdate update = sugarList[sugarState.Id].GenerateUpdate(sugarState);
                    if (update != null)
                    {
                        serializer.Write(BlockType.SugarUpdate, update);
                    }
                    sugarList[sugarState.Id].Interpolate();
                }
                else
                {

                    // create sugar
                    Sugar sugar = new Sugar(sugarState);
                    serializer.Write(BlockType.Sugar, sugar);
                    sugarList.Add(sugarState.Id, sugar);
                }

                sugarList[sugarState.Id].IsAlive = true;
            }

            // remove dead sugar
            keys = new int[sugarList.Keys.Count];
            sugarList.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!sugarList[keys[i]].IsAlive)
                {
                    serializer.Write(BlockType.SugarLost, new Lost(keys[i]));
                    sugarList.Remove(keys[i]);
                }
            }

            #endregion

            #region Bugs

            // reset alive-flag
            foreach (Bug bug in bugList.Values)
            {
                bug.IsAlive = false;
            }

            // enumerate bugs
            foreach (BugState bugState in state.BugStates)
            {

                // Check, if bug is known
                if (bugList.ContainsKey(bugState.Id))
                {

                    // bug-update
                    BugUpdate update = bugList[bugState.Id].GenerateUpdate(bugState);
                    if (update != null)
                    {
                        serializer.Write(BlockType.BugUpdate, update);
                    }
                    bugList[bugState.Id].Interpolate();
                }
                else
                {
                    // create bug
                    Bug bug = new Bug(bugState);
                    serializer.Write(BlockType.Bug, bug);
                    bugList.Add(bugState.Id, bug);
                }

                bugList[bugState.Id].IsAlive = true;
            }

            // remove dead bugs
            keys = new int[bugList.Keys.Count];
            bugList.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!bugList[keys[i]].IsAlive)
                {
                    serializer.Write(BlockType.BugLost, new Lost(keys[i]));
                    bugList.Remove(keys[i]);
                }
            }

            #endregion

            serializer.Write(BlockType.FrameEnd);
        }

        /// <summary>
        /// Close the writer.
        /// </summary>
        public void Close()
        {
            serializer.Write(BlockType.StreamEnd);
            serializer.Flush();
        }

        public void Dispose()
        {
            serializer.Dispose();
        }
    }
}