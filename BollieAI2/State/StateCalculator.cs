using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;
using BollieAI2.Services;

namespace BollieAI2.State
{
    public class StateCalculator
    {
        /// <summary>
        /// Should be called after OpponentMoves and before PlacingArmies
        /// </summary>
        public static void CalculateState()
        {
            UpdateOpponent();
            UpdateMap();
        }

        /// <summary>
        /// Update map with opponent moves
        /// </summary>
        public static void UpdateOpponent()
        {
            Map.MapState.OpponentLastPlaceArmies.ForEach(pa =>
            {
                pa.Region.CurrentArmies += pa.Armies;
            }
            );

            Map.MapState.OpponentLastAttackTransfer.ForEach(at =>
            {
                at.TargetRegion.CurrentArmies += at.Armies - Combat.AttackersLosses(at.TargetRegion.CurrentArmies);
                at.SourceRegion.CurrentArmies -= at.Armies;
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
            Map.MapState.MapUpdates.ForEach(mu =>
                {
                    mu.Region.IsVisible = true;
                    mu.Region.CurrentPlayer = mu.Player;
                    mu.Region.CurrentArmies = mu.Armies;
                }
            );

            // Flood border
            FloodsCalculation.CalculateFlood(Map.Current.Regions.Player(PlayerType.Dangerous));
            Map.Current.Regions.ForEach(R => R.DangerousBorderDistance = R.Flood);
        }

        // IS NOT IN USE

        /// <summary>
        /// Process MapUpdate into Region
        /// </summary>
        public static void ProcessMapUpdate()
        {
            List<MapUpdate> mapUpdates = Map.MapState.MapUpdates;

            // reset all to invisible
            Map.Current.Regions.ForEach(R =>
            {
                R.IsVisible = false;
                R.VisibleArmies = 0;
                R.VisiblePlayer = PlayerType.Unknown;
            }
            );

            // set visible regions
            Map.MapState.MapUpdates.ForEach(MU =>
            {
                MU.Region.IsVisible = true;
                MU.Region.VisiblePlayer = MU.Player;
                MU.Region.VisibleArmies = MU.Armies;
                // 
                MU.Region.CurrentPlayer = MU.Player;
                MU.Region.CurrentArmies = MU.Armies;
            }
            );

        }

    }
}
