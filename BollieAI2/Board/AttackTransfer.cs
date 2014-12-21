using System;
using System.Collections.Generic;
using System.Linq;

namespace BollieAI2.Board
{
    /// <summary>
    /// Command to Attack or Transfer armies
    /// </summary>
    public class AttackTransfer
    {
        /// <summary>
        /// Number of armies to attack/transfer
        /// </summary>
        public int Armies { get; set; }
        
        /// <summary>
        /// Source region 
        /// </summary>
        public Region SourceRegion { get; set; }

        /// <summary>
        /// Target region
        /// </summary>
        public Region TargetRegion { get; set; }

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="armies">Number of armies to attack/transfer</param>
        /// <param name="sourceRegion">Source region</param>
        /// <param name="targetRegion">Target region</param>
        public AttackTransfer(int armies, Region sourceRegion, Region targetRegion)
        {
            this.Armies = armies;
            this.SourceRegion = sourceRegion;
            this.TargetRegion = targetRegion;
        }

        /// <summary>
        /// Command to Attack or Transfer armies
        /// </summary>
        /// <returns>Command attack/transfer</returns>
        public override String ToString()
        {
            return String.Format("{0} attack/transfer {1} {2} {3}", Map.Current.YourBot, SourceRegion.Id, TargetRegion.Id, Armies);
        }
    }
}
