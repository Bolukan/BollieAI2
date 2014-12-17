using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BollieAI2.Board;

namespace BollieAI2.Services
{
    /// <summary>
    /// Command to Place armies
    /// </summary>
    public class PlaceArmies
    {
        /// <summary>
        /// Number of armies to place on map
        /// </summary>
        public int Armies { get; set; }
        /// <summary>
        /// Region to place armies
        /// </summary>
        public Region Region { get; set; }
        
        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="armies">Number of armies to place on map</param>
        /// <param name="region">Region to place armies</param>
        public PlaceArmies(int armies, Region region)
        {
            Armies = armies;
            Region = region;
        }
        
        /// <summary>
        /// Command to place armies
        /// </summary>
        /// <returns>command place_armies</returns>
        public override String ToString()
        {
            return String.Format("{0} place_armies {1} {2}", Map.Current.YourBot, Region.Id, Armies);
        }

    }
}
