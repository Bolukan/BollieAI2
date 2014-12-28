using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;

namespace BollieAI2.State
{
    /// <summary>
    /// R02
    /// Update map info
    /// </summary>
    public class MapUpdate
    {
        /// <summary>
        /// Region with updated info
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// Player owning region
        /// </summary>
        public PlayerType Player { get; set; }

        /// <summary>
        /// Number of armies on region
        /// </summary>
        public int Armies { get; set; }

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="player">PlayerType</param>
        /// <param name="armies"># armies</param>
        public MapUpdate(Region region, PlayerType player, int armies)
        {
            Region = region;
            Player = player;
            Armies = armies;
        }

    }
}
