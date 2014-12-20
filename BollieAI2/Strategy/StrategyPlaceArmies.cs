using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Services;
using BollieAI2.Board;

namespace BollieAI2.Strategy
{
    public class StrategyPlaceArmies
    {
        public static List<PlaceArmies> SelectPlaceArmies()
        {
            // Commando
            List<PlaceArmies> PA = new List<PlaceArmies>();

            // Find Mixed SuperRegions
            List<SuperRegion> sr_MIXED = Map.Current.SuperRegions
                .Where(sr => ((sr.StrategySuperRegion == StrategySuperRegionType.MixedHostile) ||
                              (sr.StrategySuperRegion == StrategySuperRegionType.MixedNeutral))).ToList();
            if (sr_MIXED.Count > 0)
            {
                // Find SR with less regions to combat
                SuperRegion sr_BEST = sr_MIXED.OrderBy(sr => (sr.Regions.Count(R => R.CurrentPlayer !=  PlayerType.Me))).First();
                //
                PA.Add(new PlaceArmies(Map.Current.GetArmies(), MixedRegion(sr_BEST)));
            }
            
            // Find SuperRegions with opponents
            if (Map.Current.StartingArmies > 0)
            {
                List<Region> Rs = Map.Current.Regions.Where(R => R.CurrentPlayer == PlayerType.Me)
                    .OrderByDescending(R => R.Neighbours.Count(N => N.CurrentPlayer == PlayerType.Opponent)).ToList();
                if (Rs.Count() > 0)
                {
                    PA.Add(new PlaceArmies(Map.Current.GetArmies(), Rs.First()));
                }
            }

            // Find SuperRegions with neutral
            if (Map.Current.StartingArmies > 0)
            {
                List<Region> Rs = Map.Current.Regions.Where(R => R.CurrentPlayer == PlayerType.Me)
                    .OrderByDescending(R => R.Neighbours.Count(N => N.CurrentPlayer != PlayerType.Me)).ToList();
                if (Rs.Count() > 0)
                {
                    PA.Add(new PlaceArmies(Map.Current.GetArmies(), Rs.First()));
                }
            }

            return PA;
        }

        public static Region MixedRegion(SuperRegion SR)
        {
            return SR.Regions.Where(R => R.CurrentPlayer != PlayerType.Me)
                .SelectMany(Target => Target.Neighbours).Distinct()
                .Where(N => N.CurrentPlayer == PlayerType.Me)
                .OrderByDescending(R =>
                    R.Neighbours.Count(N => N.CurrentPlayer == PlayerType.Opponent) * 2 +
                    R.Neighbours.Count(N => N.CurrentPlayer == PlayerType.Neutral)).First();
        }

    }
}
