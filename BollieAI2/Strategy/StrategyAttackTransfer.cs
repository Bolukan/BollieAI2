using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Services;
using BollieAI2.Board;

namespace BollieAI2.Strategy
{
    public class StrategyAttackTransfer
    {
  
        public static List<AttackTransfer> GetAttackTransferList()
        {
            List<AttackTransfer> AT = new List<AttackTransfer>();

            // Try fighting with all my regions
            IEnumerable<Region> regionsMe = Map.Current.Regions.Where(R => ((R.CurrentPlayer == PlayerType.Me) && (R.CurrentArmies >= 3)));

            foreach (Region rm in regionsMe)
            {
                // find opponent neighbours (biggest first)
                IEnumerable<Region> ros = rm.Neighbours.Where(N => N.CurrentPlayer == PlayerType.Opponent)
                    .OrderByDescending(N => N.CurrentArmies);
                if (ros.Any())
                {
                        if (rm.CurrentArmies - 1 > ros.First().ArmiesToAttack)
                        {
                            AT.Add(AddAttack(rm, ros.First(), ros.First().ArmiesToAttack));
                        }
                    
                }
                else
                {

                    // find neutral neighbours (biggest first)
                    IEnumerable<Region> rns = rm.Neighbours.Where(N => N.CurrentPlayer == PlayerType.Neutral)
                        .OrderByDescending(N => N.CurrentArmies);
                    if (rns.Any())
                    {
                        int maxOpponent = rns.FirstOrDefault().CurrentArmies;
                    }
                    
                    foreach (Region rn in rns)
                    {
                        if (rm.CurrentArmies >= (2 * rn.CurrentArmies))
                        {
                            AT.Add(AddAttack(rm, rn, (rn.CurrentArmies * 2 - 1)));
                        }
                    }

                }
            }

            return AT;
        }

        private static AttackTransfer AddAttack(Region regionSource, Region regionTarget, int armies)
        {
            regionSource.CurrentArmies -= armies;
            return new AttackTransfer(armies, regionSource, regionTarget);
        }
    }
}
