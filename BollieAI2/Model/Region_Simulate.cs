using System;
using System.Collections.Generic;
using System.Linq;
using BollieAI2.Services;

namespace BollieAI2.Model
{
    /// <summary>
    /// Subclass for Analysing SuperRegion
    /// </summary>
    public partial class Region
    {
        /// <summary>
        /// Simulate #armies
        /// </summary>
        public int SimulateArmies { get; set; }

        /// <summary>
        /// Simulate #armies
        /// </summary>
        public PlayerType SimulatePlayer { get; set; }
    }

}
