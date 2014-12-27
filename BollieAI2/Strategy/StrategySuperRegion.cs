using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;
using BollieAI2.Services;

namespace BollieAI2.Strategy
{
    /// <summary>
    /// Strategy for SuperRegion based on owners of regions
    /// </summary>
    public enum StrategySuperRegionType
    {
        Defend = 1,   // only me
        MixedHostile, // me and opponent
        MixedNeutral, // me and not opponent
        KeepWatch,    // not me, not only opponent
        Attack,       // not me, only opponent
        Unknown       // no sight
    }

    /// <summary>
    /// Strategy on SuperRegions
    /// </summary>
    public class StrategySuperRegion
    {
        /// <summary>
        /// Update StrategySuperRegion
        /// </summary>
        public static void UpdateStrategySuperRegion()
        {
            Map.Current.SuperRegions.ForEach(SR =>
                {
                    // count amount per players
                    IEnumerable<Region> RegionMe = SR.Regions.Player(PlayerType.Me);
                    IEnumerable<Region> RegionOpponent = SR.Regions.Player(PlayerType.Opponent);
                    IEnumerable<Region> RegionNeutral = SR.Regions.Player(PlayerType.Neutral);
                    // IEnumerable<Region> RegionUnknown = SR.Regions.Player(PlayerType.Unknown);
                    
                    // all me
                    if (RegionMe.Count() == SR.Regions.Count())
                    {
                        SR.StrategySuperRegion = StrategySuperRegionType.Defend;
                    }
                    // some me
                    else if (RegionMe.Count() != 0)
                    {
                        // opponent seen SuperRegion
                        if (RegionOpponent.Count() != 0)
                        {
                            SR.StrategySuperRegion = StrategySuperRegionType.MixedHostile;
                        }
                        else
                        // together in SuperRegion
                        {
                            SR.StrategySuperRegion = StrategySuperRegionType.MixedNeutral;
                        }
                    }
                    // not me
                    else
                    {
                        // opponent seen SuperRegion
                        if (RegionOpponent.Count() != 0)
                        {
                            // also Neutral Regions
                            if (RegionNeutral.Count() != 0)
                            {
                                // we have some time left
                                SR.StrategySuperRegion = StrategySuperRegionType.KeepWatch;
                            }
                            else
                            {
                                // they seem to have control, attack asap
                                SR.StrategySuperRegion = StrategySuperRegionType.Attack;
                            }
                        }
                        else
                        {
                            // no idea what is going on there
                            SR.StrategySuperRegion = StrategySuperRegionType.Unknown;
                        }
                    }
                }
            );
        } // end sub
    
    }
}
