using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BollieAI2.Services;

namespace BollieAI2.Board
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
            SuperRegions = new List<SuperRegion>();
            Regions = new List<Region>();
            Wastelands = new List<Region>();
        }

        /// <summary>
        /// Map instance
        /// </summary>
        private static Map _instance;

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
        /// Superregions are one or more regions with a bonus armies reward
        /// </summary>
        public List<SuperRegion> SuperRegions { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        public List<Region> Regions { get; set; }

        /// <summary>
        /// Wastelands are neutral regions with more than 2 armies on them. 
        /// </summary>
        public List<Region> Wastelands { get; set; }

        /// <summary>
        /// At the start of the game, the engine will make a list of regions by picking one random region from each super region.
        /// Then the bots are asked in turn what region they would like to start on and they will get the region they return. 
        /// The ordering is in an ABBAAB fashion to keep things fair.
        /// If there is an uneven amount of regions to be picked from, the last region remaining will be a neutral, 
        /// so both players have an equal amount of starting regions.
        /// </summary>
        public List<Region> StartingRegions { get; set; }

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

        #region Current

        /// <summary>
        /// ms available for current operation
        /// </summary>
        public int CurrentTimebank;

        /// <summary>
        /// Armies to place on Map this turn
        /// </summary>
        public int StartingArmies { get; set; }

        public int GetArmies()
        {
            return GetArmies(StartingArmies);
        }

        public int GetArmies(int armies)
        {
            if (armies > StartingArmies)
            {
                armies = StartingArmies;
            }
            StartingArmies -= StartingArmies;
            return armies;
        }

        /// <summary>
        /// Number of Regions for each player
        /// </summary>
        public Dictionary<PlayerType, int> CurrentRegionsCount;

        /// <summary>
        /// all the visible moves the opponent has done are given in consecutive order.
        /// </summary>
        public List<PlaceArmies> OpponentLastPlaceArmies { get; set; }

        /// <summary>
        /// all the visible moves the opponent has done are given in consecutive order.
        /// </summary>
        public List<AttackTransfer> OpponentLastAttackTransfer { get; set; }

        /// <summary>
        /// Map updates
        /// </summary>
        public List<MapUpdate> MapUpdates { get; set; }

        #endregion
    }
}
