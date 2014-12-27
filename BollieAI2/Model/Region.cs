using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Services;

namespace BollieAI2.Model
{
    /// <summary>
    /// Region
    /// </summary>
    public partial class Region
    {
        /// <summary>
        /// Initialise
        /// </summary>
        public Region()
        {
            Neighbours = new Regions();
            CurrentPlayer = PlayerType.Unknown;
            CurrentArmies = 2;
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
        public Regions Neighbours { get; set; }
        
        #region Turnbased

        /// <summary>
        /// Player in charge of region
        /// </summary>
        public PlayerType CurrentPlayer { get; set; }

        /// <summary>
        /// Armies in region
        /// </summary>
        public int CurrentArmies { get; set; }

        #region update_map

        /// <summary>
        /// Region is visible
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Player visible
        /// </summary>
        public PlayerType VisiblePlayer { get; set; }

        /// <summary>
        /// Armies visible
        /// </summary>
        public int VisibleArmies { get; set; }

        #endregion

        /// <summary>
        /// 95% or more success in attack of this region
        /// </summary>
        public int ArmiesToAttack
        {
            get
            {
                return Combat.AttackersNeeded(CurrentArmies);
            }
        }
        
        #endregion

    }
}
