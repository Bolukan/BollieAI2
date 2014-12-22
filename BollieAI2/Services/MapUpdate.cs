using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Board;
using BollieAI2.Helpers;

namespace BollieAI2.Services
{
    /// <summary>
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

        /// <summary>
        /// Update map with opponent moves
        /// </summary>
        public static void UpdateOpponent()
        {
            Map.Current.OpponentLastPlaceArmies.ForEach(pa =>
                {
                    pa.Region.CurrentArmies += pa.Armies;
                }
            );

            Map.Current.OpponentLastAttackTransfer.ForEach(at =>
                {
                    at.SourceRegion.CurrentArmies -= at.Armies;
                    at.TargetRegion.CurrentArmies += at.Armies;
                }
            );

        }

        /// <summary>
        /// Process update
        /// </summary>
        /// <param name="mapUpdates"></param>
        public static void UpdateMap()
        {
            Map.Current.Regions.ForEach(r =>
                {
                    // reset Visible (map_update)
                    r.IsVisible = false;

                    // lower expected armies for invisible opponent regions
                    // -20% each turn, but not lower than 1 or 2 
                    if (r.CurrentPlayer == PlayerType.Opponent)
                    {
                        r.CurrentArmies = (int)((2 + r.CurrentArmies * 4) / 5);
                    }
                    
                    // change playerType
                    if (r.CurrentPlayer.In(PlayerType.Me | PlayerType.Neutral))
                        r.CurrentPlayer = PlayerType.Unknown;
                }
            );

            // update map
            Map.Current.MapUpdates.ForEach(mu =>
                {
                    mu.Region.IsVisible = true;
                    mu.Region.CurrentPlayer = mu.Player;
                    mu.Region.CurrentArmies = mu.Armies;
                }
            );

            // Flood border
            FloodsCalculation.CalcDangerousBorderDistance();

        }
   }
}
