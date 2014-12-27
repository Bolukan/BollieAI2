using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;

namespace BollieAI2.Services
{
    public class FloodsCalculation
    {
        public static void CalcDangerousBorderDistance()
        {
            IEnumerable<Region> regionsDangerous = Map.Current.Regions.Player(PlayerType.Dangerous);
            
            // reset value
            Map.Current.Regions.ForEach(R => R.DangerousBorderDistance = int.MaxValue);
            // Set Dangerous on 0
            foreach(Region r in regionsDangerous)
            {
                r.DangerousBorderDistance = 0;
            }

            IEnumerable<Region> regionsOpen = Map.Current.Regions.Where(R => R.DangerousBorderDistance == int.MaxValue);
            while (regionsOpen.Any())
            {
                List<Region> regionsFlood = regionsOpen.Where(R => R.Neighbours.Any(N => N.DangerousBorderDistance < int.MaxValue)).ToList();
                if (regionsFlood.Any())
                    regionsFlood.ForEach(R => R.DangerousBorderDistance = R.Neighbours.Min(N => N.DangerousBorderDistance) + 1);
            }
        }
    }
}
