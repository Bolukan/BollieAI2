using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using BollieAI2.Services;
using BollieAI2.Strategy;
using BollieAI2.State;

namespace BollieAI2.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Initialise
        /// </summary>
        public Map()
        {
            Regions = new Regions();
            SuperRegions = new List<SuperRegion>();
            Wastelands = new Regions();
            Connections = new List<Connection>();
        }

        /// <summary>
        /// Map instance
        /// </summary>
        private static Map _instance;

        /// <summary>
        /// Roundspecific state of map
        /// </summary>
        private static MapState mapState;

        /// <summary>
        /// Map instance: "Map.Current"
        /// </summary>
        public static Map Current
        {
            [DebuggerStepThrough]
            get
            {
                if (_instance == null)
                {
                    _instance = new Map();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Get the round specific state of the Map
        /// </summary>
        public static MapState MapState
        {
            [DebuggerStepThrough]
            get
            {
                if (mapState == null)
                {
                    mapState = new MapState();
                }
                return mapState;
            }
        }

        #region Static

        /// <summary>
        /// Superregions are one or more regions with a bonus armies reward
        /// </summary>
        public List<SuperRegion> SuperRegions { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        public Regions Regions { get; set; }

        /// <summary>
        /// Wastelands are neutral regions with more than 2 armies on them. 
        /// </summary>
        public Regions Wastelands { get; set; }

        /// <summary>
        /// Connections are define 2 Regions as neighbours
        /// </summary>
        public List<Connection> Connections { get; set; }

        /// <summary>
        /// At the start of the game, the engine will make a list of regions by picking one random region from each super region.
        /// Then the bots are asked in turn what region they would like to start on and they will get the region they return. 
        /// The ordering is in an ABBAAB fashion to keep things fair.
        /// If there is an uneven amount of regions to be picked from, the last region remaining will be a neutral, 
        /// so both players have an equal amount of starting regions.
        /// </summary>
        public Regions StartingRegions { get; set; }

        /// <summary>
        /// The maximum (and initial) amount of time in the timebank is given in ms
        /// </summary>
        public int TimebankInitial { get; set; }

        /// <summary>
        /// The amount of time that is added to your timebank each time a move is requested in ms.
        /// </summary>
        public int TimePerMove { get; set; }

        /// <summary>
        /// The maximum amount of rounds in this game. When this number is reached it's a draw. 
        /// </summary>
        public int MaxRounds { get; set; }

        /// <summary>
        /// The name of your bot is given.
        /// </summary>
        public String YourBot { get; set; }

        /// <summary>
        /// The name of your opponent bot is given.
        /// </summary>
        public String OpponentBot { get; set; }

        #endregion

        public int MapBonusArmiesAward()
        {
            return SuperRegions.Sum(SR => SR.BonusArmiesAward);
        }

        #region Current

        /// <summary>
        /// ms available for current operation
        /// </summary>
        public int CurrentTimebank;
        
        #endregion
    }
}
