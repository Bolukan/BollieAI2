using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Strategy;

namespace BollieAI2.Model
{
    /// <summary>
    /// Superregion is one or more regions with a bonus armies reward
    /// </summary>
    public class SuperRegion
    {
        /// <summary>
        /// Initialise
        /// </summary>
        public SuperRegion()
        {
            Regions = new Regions();
        }
        
        /// <summary>
        /// setup_map super_regions [-i -i ...]
        /// Odd numbers are superregion ids, even numbers are rewards.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// setup_map super_regions [-i -i ...]
        /// Odd numbers are superregion ids, even numbers are rewards.
        /// </summary>
        public int BonusArmiesAward { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        public Regions Regions { get; set; }

        /// <summary>
        /// Pick starting region in this SuperRegion
        /// </summary>
        public Region StartingRegion { get; set; }

        #region Assess

        /// <summary>
        /// Cost of owning: number of regions 
        /// </summary>
        public int CurrentCostOwn { get; set; }

        /// <summary>
        /// Cost of combat: number of armies * 0.7
        /// </summary>
        public int CurrentCostLoss { get; set; }

        /// <summary>
        /// Worth of SuperRegion: BonusArmies / ROI%
        /// </summary>
        public int CurrentBonus { get; set; }

        /// <summary>
        /// Value of SuperRegion (can be negative)
        /// </summary>
        public int CurrentValue
        {
            get
            {
                return CurrentBonus - (CurrentCostLoss + CurrentCostOwn);
            }
        }

        public Strategy.StrategySuperRegionType StrategySuperRegion { get; set; }
        
        #endregion
    }
}
