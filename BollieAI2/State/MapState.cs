using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;
using BollieAI2.Services;

namespace BollieAI2.State
{
    /// <summary>
    /// Round specifics of Map
    /// </summary>
    public class MapState
    {
        /// <summary>
        /// Initialise
        /// </summary>
        public MapState()
        {
        }
        
        /// <summary>
        /// Round of <see cref="MaxRounds"/>
        /// </summary>
        public int Round { get; set; }
        
        /// <summary>
        /// R01
        /// Armies to place on Map this turn
        /// </summary>
        public int StartingArmies { get; set; }

        /// <summary>
        /// Reset to 0 when StartingArmies is set
        /// </summary>
        public int PlacedArmies { get; set; }
        
        /// <summary>
        /// Available armies
        /// </summary>
        /// <returns>armies</returns>
        public int ArmiesAvailable()
        {
            return StartingArmies - PlacedArmies;
        }

        /// <summary>
        /// Reserve armies
        /// </summary>
        /// <param name="armiesWanted"># armies</param>
        /// <returns># armies</returns>
        public int ReserveArmies(int armiesWanted)
        {
            if (armiesWanted > ArmiesAvailable())
            {
                armiesWanted = ArmiesAvailable();
            }
            PlacedArmies += armiesWanted;
            return armiesWanted;
        }
        
        /// <summary>
        /// R02
        /// Map updates
        /// </summary>
        public List<MapUpdate> MapUpdates { get; set; }

        /// <summary>
        /// R03
        /// all the visible moves the opponent has done are given in consecutive order.
        /// </summary>
        public List<PlaceArmies> OpponentLastPlaceArmies { get; set; }

        /// <summary>
        /// R03
        /// all the visible moves the opponent has done are given in consecutive order.
        /// </summary>
        public List<AttackTransfer> OpponentLastAttackTransfer { get; set; }
        
    }
}
