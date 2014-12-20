using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BollieAI2.Board;
using System.IO;

namespace BollieAI2.Services
{
    /// <summary>
    /// Intelligence to pick the best starting regions
    /// </summary>
    public class StartingRegions
    {
        /// <summary>
        /// Number of pick_starting_region requests
        /// </summary>
        public static int PickingRound = 0;
        
        /// <summary>
        /// Process all possible Starting Regions
        /// </summary>
        /// <param name="pickRegions"></param>
        public static void SetStartingRegions(List<Region> startingRegions)
        {
            // save them all
            Map.Current.StartingRegions = startingRegions;
            // and in each superregion
            startingRegions.ForEach(r =>
                {
                    r.SuperRegion.StartingRegion = r;
                }
            );

            // Update values
            CurrentUpdate.UpdateRegion();
            CurrentUpdate.UpdateSuperRegion();
            CurrentUpdate.UpdateMap();

        }

        /// <summary>
        /// Pick the best region
        /// </summary>
        /// <param name="pickRegions">Available regions</param>
        public static Region PickFromRegions(List<Region> pickRegions)
        {
            return pickRegions.OrderByDescending(r => r.SuperRegion.CurrentValue).FirstOrDefault();
        }

    }
}
