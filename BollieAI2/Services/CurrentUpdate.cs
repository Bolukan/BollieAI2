using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BollieAI2.Board;

namespace BollieAI2.Services
{
    /// <summary>
    /// Calculate all Current fields
    /// </summary>
    class CurrentUpdate
    {
        public static void UpdateSuperRegion()
        {
            Map.Current.SuperRegions.ForEach(sr =>
                {
                    sr.CurrentBonus = sr.BonusArmiesAward * Configuration.ONE_DIVIDED_ROI;
                    sr.CurrentCostLoss = sr.Regions.
                        Where(r => r.CurrentPlayer != PlayerType.Me).
                        Sum(r => (int)(0.5 + r.CurrentArmies * 0.7));
                    sr.CurrentCostOwn = sr.Regions.
                        Count(r => r.CurrentPlayer != PlayerType.Me);
                }
            );
        }
    }
}
