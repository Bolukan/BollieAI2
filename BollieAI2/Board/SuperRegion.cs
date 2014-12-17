using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


    }
}
