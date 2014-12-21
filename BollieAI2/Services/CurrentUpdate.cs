using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Board;

namespace BollieAI2.Services
{
    /// <summary>
    /// Calculate all Current fields
    /// </summary>
    class CurrentUpdate
    {
        /// <summary>
        /// Update Current values for SuperRegion
        /// </summary>
        public static void UpdateRegion()
        {
        }

        /// <summary>
        /// Update Current values for SuperRegion
        /// </summary>
        public static void UpdateSuperRegion()
        {
            Map.Current.SuperRegions.ForEach(sr =>
                {
                    // Valuation of BonusArmies for SuperRegion for a certain timehorizon, p.e. 10
                    sr.CurrentBonus = sr.BonusArmiesAward * Configuration.ONE_DIVIDED_ROI;
                    
                    // Losses occurred while combatting
                    sr.CurrentCostLoss = sr.Regions.
                        Where(r => r.CurrentPlayer != PlayerType.Me).
                        Sum(r => (int)(0.5 + r.CurrentArmies * 0.7));
                    
                    // 1 army for each region to keep it occupied
                    sr.CurrentCostOwn = sr.Regions.
                        Count(r => r.CurrentPlayer != PlayerType.Me);
                }
            );
        }

        /// <summary>
        /// Update Current values for Map
        /// </summary>
        public static void UpdateMap()
        {
            // Count Regions for each PlayerType
            Map.Current.CurrentRegionsCount =
                Map.Current.Regions.GroupBy(r => r.CurrentPlayer).ToDictionary(g => g.Key, g => g.Count());
        }


    }
}
