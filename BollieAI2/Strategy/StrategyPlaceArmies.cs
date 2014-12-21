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
                Region placeRegion = MixedRegion(sr_BEST);
                int placeArmies = Map.Current.GetArmies();
                placeRegion.CurrentArmies += placeArmies;
                PA.Add(new PlaceArmies(placeArmies, placeRegion));

            }
            
            // Find SuperRegions with opponents
            if (Map.Current.StartingArmies > 0)
            {
                IEnumerable<Region> Rs = Map.Current.Regions.Player(PlayerType.Me)
                    .OrderByDescending(r1 => r1.Neighbours.Count(N => N.CurrentPlayer == PlayerType.Opponent));
                
                if (Rs.Count() > 0)
                {
                    Region placeRegion = Rs.First();
                    int placeArmies = Map.Current.GetArmies();
                    placeRegion.CurrentArmies += placeArmies;
                    PA.Add(new PlaceArmies(placeArmies, placeRegion));
                }
            }

            // Find SuperRegions with neutral
            if (Map.Current.StartingArmies > 0)
            {
                IEnumerable<Region> Rs = Map.Current.Regions.Player(PlayerType.Me)
                    .OrderByDescending(r => r.Neighbours.Count(N => N.CurrentPlayer != PlayerType.Me));
                if (Rs.Count() > 0)
                {
                    Region placeRegion = Rs.First();
                    int placeArmies = Map.Current.GetArmies();
                    placeRegion.CurrentArmies += placeArmies;
                    PA.Add(new PlaceArmies(placeArmies, placeRegion));
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
