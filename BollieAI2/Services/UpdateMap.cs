using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;

namespace BollieAI2.Services
{
    /// <summary>
    /// R02
    /// Process <see cref="MapUpdate"/> into <see cref="Region"/>
    /// </summary>
    public class UpdateMap
    {
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
                }
            );

            // set visible regions
            Map.MapState.MapUpdates.ForEach(MU =>
                {
                    MU.Region.IsVisible = true;
                    MU.Region.VisiblePlayer = MU.Player;
                    MU.Region.VisibleArmies = MU.Armies;
                    // old
                    MU.Region.CurrentPlayer = MU.Player;
                    MU.Region.CurrentArmies = MU.Armies;
                }
            );

        }
    }
}
