using System;
using System.Collections.Generic;
using System.Linq;

namespace BollieAI2.Board
{
    /// <summary>
    /// Connection between 2 regions
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Source region
        /// </summary>
        public Region SourceRegion { get; set; }

        /// <summary>
        /// Target region
        /// </summary>
        public Region TargetRegion { get; set; }

        /// <summary>
        /// Intialise
        /// </summary>
        /// <param name="sourceRegion">Source region</param>
        /// <param name="targetRegion">Target region</param>
        public Connection(Region sourceRegion, Region targetRegion)
        {
            SourceRegion = sourceRegion;
            TargetRegion = targetRegion;
        }
    }
}
