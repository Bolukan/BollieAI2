using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BollieAI2.Strategy;

namespace BollieAI2.Board
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
            Regions = new List<Region>();
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
        public List<Region> Regions { get; set; }

        /// <summary>
        /// Regions of a particular player
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Regions</returns>
        public IEnumerable<Region> RegionsPlayer(PlayerType player) 
        {
            return Regions.Where(R => R.CurrentPlayer == player);
        }

        /// <summary>
        /// Pick starting region in this SuperRegion
        /// </summary>
        public Region PickStartingRegion { get; set; }

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
