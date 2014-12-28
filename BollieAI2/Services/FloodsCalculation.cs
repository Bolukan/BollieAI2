using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;

namespace BollieAI2.Services
{
    public class FloodsCalculation
    {
        public static void CalculateFlood(IEnumerable<Region> FloodSource)
        {
            // reset value
            Map.Current.Regions.ForEach(R => R.Flood = int.MaxValue);
            // Set Source of flood on 0
            foreach (Region r in FloodSource)
            {
                r.Flood = 0;
            }

            // regions not calculated yet
            IEnumerable<Region> regionsOpen = Map.Current.Regions.Where(R => R.Flood == int.MaxValue);
            while (regionsOpen.Any())
            {
                List<Region> regionsFlood = regionsOpen.Where(R => R.Neighbours.Any(N => N.Flood < int.MaxValue)).ToList();
                if (regionsFlood.Any())
                {
                    regionsFlood.ForEach(R => R.Flood = R.Neighbours.Min(N => N.Flood) + 1);
                }
                else
                {
                    // can't find any neighbours .... some floods can not reach every region
                    regionsOpen = Enumerable.Empty<Region>();
                }
            }
        }

        public static void CalcDistanceRegion()
        {

        }

    }
}
