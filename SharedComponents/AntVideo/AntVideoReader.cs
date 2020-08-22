using AntMe.SharedComponents.AntVideo.Block;
using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AntMe.SharedComponents.AntVideo
{
    /// <summary>
    /// Class, to read and decode ant-video-Streams.
    /// </summary>
    public sealed class AntVideoReader : IDisposable
    {
        #region local variables

        private readonly Dictionary<int, Anthill> anthillList;
        private readonly Dictionary<int, Ant> antList;
        private readonly Dictionary<int, Bug> bugList;
        private readonly Dictionary<int, Fruit> fruitList;
        private readonly Dictionary<int, Marker> markerList;
        private readonly Dictionary<int, Dictionary<int, Caste>> casteList;
        private readonly Serializer serializer;
        private readonly Dictionary<int, Sugar> sugarList;
        private readonly Dictionary<int, Team> teamList;
        private readonly Dictionary<int, Dictionary<int, Colony>> colonyList;
        private Frame frame;
        private bool complete;

        #endregion

        /// <summary>
        /// Creates a new instance of reader.
        /// </summary>
        /// <param name="inputStream">input-stream</param>
        public AntVideoReader(Stream inputStream)
        {

            // check for stream
            if (inputStream == null)
                throw new ArgumentNullException("inputStream", Resource.AntvideoReaderNoStreamException);

            // check for readable stream
            if (!inputStream.CanRead || !inputStream.CanSeek)
            {
                throw new InvalidOperationException(Resource.AntvideoReaderNoReadAccessException);
            }

            // init block-lists
            teamList = new Dictionary<int, Team>();
            colonyList = new Dictionary<int, Dictionary<int, Colony>>();
            casteList = new Dictionary<int, Dictionary<int, Caste>>();
            anthillList = new Dictionary<int, Anthill>();
            antList = new Dictionary<int, Ant>();
            markerList = new Dictionary<int, Marker>();
            bugList = new Dictionary<int, Bug>();
            sugarList = new Dictionary<int, Sugar>();
            fruitList = new Dictionary<int, Fruit>();

            // create serializer
            serializer = new Serializer(inputStream, true, false);

            // read hello-message
            serializer.ReadHello();
        }

        /// <summary>
        /// Reads a new simulation-state out of stream.
        /// </summary>
        /// <returns>New simulation-state or null, if stream is over</returns>
        public SimulationState Read()
        {

            // if stream is at his end, return null
            if (complete)
            {
                return null;
            }

            // first block have to be a frame-start
            ISerializable block;
            BlockType blockType = serializer.Read(out block);

            // detect stream-end
            if (blockType == BlockType.StreamEnd)
            {
                complete = true;
                return null;
            }

            // unexpected block-type
            if (blockType != BlockType.FrameStart)
            {
                throw new InvalidOperationException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.AntvideoReaderInvalidBlockType, blockType));
            }

            // block-loop
            while (blockType != BlockType.FrameEnd)
            {
                blockType = serializer.Read(out block);
                switch (blockType)
                {
                    case BlockType.Ant:
                        Ant ant = (Ant)block;
                        antList.Add(ant.Id, ant);
                        break;
                    case BlockType.Anthill:
                        Anthill anthill = (Anthill)block;
                        anthillList.Add(anthill.Id, anthill);
                        break;
                    case BlockType.AntLost:
                        Lost antLost = (Lost)block;
                        antList.Remove(antLost.Id);
                        break;
                    case BlockType.AntUpdate:
                        AntUpdate antUpdate = (AntUpdate)block;
                        antList[antUpdate.Id].Update(antUpdate);
                        break;
                    case BlockType.Bug:
                        Bug bug = (Bug)block;
                        bugList.Add(bug.Id, bug);
                        break;
                    case BlockType.BugLost:
                        Lost bugLost = (Lost)block;
                        bugList.Remove(bugLost.Id);
                        break;
                    case BlockType.BugUpdate:
                        BugUpdate bugUpdate = (BugUpdate)block;
                        bugList[bugUpdate.Id].Update(bugUpdate);
                        break;
                    case BlockType.Caste:
                        Caste caste = (Caste)block;
                        casteList[caste.ColonyId].Add(caste.Id, caste);
                        break;
                    case BlockType.Team:
                        Team team = (Team)block;
                        teamList.Add(team.Id, team);
                        colonyList.Add(team.Id, new Dictionary<int, Colony>());
                        break;
                    case BlockType.Colony:
                        Colony colony = (Colony)block;
                        colonyList[colony.TeamId].Add(colony.Id, colony);
                        casteList.Add(colony.Id, new Dictionary<int, Caste>());
                        break;
                    case BlockType.ColonyUpdate:
                        ColonyUpdate colonyUpdate = (ColonyUpdate)block;
                        colonyList[colonyUpdate.TeamId][colonyUpdate.Id].Update(colonyUpdate);
                        break;
                    case BlockType.Frame:
                        frame = (Frame)block;
                        break;
                    case BlockType.FrameUpdate:
                        FrameUpdate frameUpdate = (FrameUpdate)block;
                        frame.Update(frameUpdate);
                        break;
                    case BlockType.Fruit:
                        Fruit fruit = (Fruit)block;
                        fruitList.Add(fruit.Id, fruit);
                        break;
                    case BlockType.FruitLost:
                        Lost fruitLost = (Lost)block;
                        fruitList.Remove(fruitLost.Id);
                        break;
                    case BlockType.FruitUpdate:
                        FruitUpdate fruitUpdate = (FruitUpdate)block;
                        fruitList[fruitUpdate.Id].Update(fruitUpdate);
                        break;
                    case BlockType.Marker:
                        Marker marker = (Marker)block;
                        markerList.Add(marker.Id, marker);
                        break;
                    case BlockType.MarkerLost:
                        Lost markerLost = (Lost)block;
                        markerList.Remove(markerLost.Id);
                        break;
                    case BlockType.MarkerUpdate:
                        MarkerUpdate markerUpdate = (MarkerUpdate)block;
                        markerList[markerUpdate.Id].Update(markerUpdate);
                        break;
                    case BlockType.Sugar:
                        Sugar sugar = (Sugar)block;
                        sugarList.Add(sugar.Id, sugar);
                        break;
                    case BlockType.SugarLost:
                        Lost sugarLost = (Lost)block;
                        sugarList.Remove(sugarLost.Id);
                        break;
                    case BlockType.SugarUpdate:
                        SugarUpdate sugarUpdate = (SugarUpdate)block;
                        sugarList[sugarUpdate.Id].Update(sugarUpdate);
                        break;
                }
            }

            // Detect streamend
            if ((BlockType)serializer.Peek() == BlockType.StreamEnd)
            {
                complete = true;
            }

            // Interpolate all elements and buildup state
            frame.Interpolate();
            SimulationState state = frame.GenerateState();

            foreach (Bug bug in bugList.Values)
            {
                bug.Interpolate();
                state.BugStates.Add(bug.GenerateState());
            }
            foreach (Fruit fruit in fruitList.Values)
            {
                fruit.Interpolate();
                state.FruitStates.Add(fruit.GenerateState());
            }
            foreach (Sugar sugar in sugarList.Values)
            {
                sugar.Interpolate();
                state.SugarStates.Add(sugar.GenerateState());
            }

            foreach (Team team in teamList.Values)
            {
                TeamState teamState = team.GenerateState();
                state.TeamStates.Add(teamState);

                foreach (Colony colony in colonyList[team.Id].Values)
                {
                    colony.Interpolate();
                    ColonyState colonyState = colony.GenerateState();
                    teamState.ColonyStates.Add(colonyState);

                    foreach (Caste caste in casteList[colony.Id].Values)
                    {
                        colonyState.CasteStates.Add(caste.GenerateState());
                    }

                    foreach (Anthill anthill in anthillList.Values)
                    {
                        if (anthill.ColonyId == colony.Id)
                        {
                            colonyState.AnthillStates.Add(anthill.GenerateState());
                        }
                    }

                    foreach (Ant ant in antList.Values)
                    {
                        if (ant.ColonyId == colony.Id)
                        {
                            ant.Interpolate();
                            colonyState.AntStates.Add(ant.GenerateState());
                        }
                    }
                    foreach (Marker marker in markerList.Values)
                    {
                        if (marker.ColonyId == colony.Id)
                        {
                            marker.Interpolate();
                            colonyState.MarkerStates.Add(marker.GenerateState());
                        }
                    }
                }
            }

            // deliver
            return state;
        }

        /// <summary>
        /// Gives the current frame-position.
        /// </summary>
        public int CurrentFrame
        {
            get { return frame != null ? frame.CurrentRound : 0; }
        }

        /// <summary>
        /// Gives the number of total frames.
        /// </summary>
        public int TotalFrames
        {
            get { return frame != null ? frame.TotalRounds : 0; }
        }

        /// <summary>
        /// Gibt an, ob der Stream zu Ende gelesen wurde.
        /// </summary>
        public bool Complete
        {
            get { return complete; }
        }

        public void Dispose()
        {
            serializer.Dispose();
        }
    }
}