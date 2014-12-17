using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BollieAI2.Board
{
    /// <summary>
    /// 
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Initialise
        /// </summary>
        public Region()
        {
            Neighbours = new List<Region>();
        }
        
        /// <summary>
        /// setup_map regions [-i -i ...]
        /// Odd numbers are the region ids, even numbers are the superregion ids.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// setup_map regions [-i -i ...]
        /// Odd numbers are the region ids, even numbers are the superregion ids.
        /// </summary>
        public SuperRegion SuperRegion { get; set; }

        /// <summary>
        /// setup_map neighbors [-i [-i,...] ...] 
        /// The connectivity of the regions are given
        /// </summary>
        public List<Region> Neighbours { get; set; }

    }
}
