using System;
using System.Collections.Generic;
using System.Linq;

namespace BollieAI2.Model
{
    class Configuration
    {
        /// <summary>
        /// Name for neutral
        /// </summary>
        public const String PLAYER_NEUTRAL = "neutral";
        
        /// <summary>
        /// Armies on wasteland
        /// </summary>
        public const int WASTELAND_ARMIES = 10;

        /// <summary>
        /// Multiplier for eternal ROI on Bonus Armies =  1/10% = 10
        /// </summary>
        public const int ONE_DIVIDED_ROI = 10;

        /// <summary>
        /// Minimum Starting Armies each round
        /// </summary>
        public const int STARTING_ARMIES_MINIMUM = 5;

    }
}
