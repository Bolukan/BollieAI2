using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BollieAI2.Board;

namespace BollieAI2.Services
{
    /// <summary>
    /// Intelligence to pick the best starting regions
    /// </summary>
    public class PickStartingRegions
    {
        /// <summary>
        /// Number of pick_starting_region requests
        /// </summary>
        public static int PickingRound = 0;
        
        /// <summary>
        /// If opponent starts, we missed one picking region/superregion
        /// </summary>
        public static bool OpponentStarted;

        /// <summary>
        /// Pick the best region
        /// </summary>
        /// <param name="pickRegions">Available regions</param>
        public static Region PickFromRegions(List<Region> pickRegions)
        {
            PickingRound++;
            // first time
            if (PickingRound == 1)
            {
                OpponentStarted = (Map.Current.SuperRegions.Count > pickRegions.Count);
                // signal all regions to be opponent, just to be save
                pickRegions.ForEach(r =>
                    {
                        r.SuperRegion.PickStartingRegion = r;
                        r.CurrentPlayer = PlayerType.Opponent;
                    }
                );
            }

            // all times
            
            // TEMPORARY: PICK FIRST
            return pickRegions[0];

        }



    }
}
