using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Services;
using BollieAI2.Board;
using BollieAI2.Helpers;

namespace BollieAI2.Strategy
{
    public class StrategyAttackTransfer
    {
  
        public static List<AttackTransfer> GetAttackTransferList()
        {
            List<AttackTransfer> AT = new List<AttackTransfer>();

            // Try fighting with all my regions
            IEnumerable<Region> regionsMe = Map.Current.Regions.Player(PlayerType.Me).Where(R => R.CurrentArmies >= 3);

            // OneWay out -> go for it !!
            foreach (Connection OneWayOut in MeOneOther())
            {
                if (Combat.AttackersNeeded(OneWayOut.TargetRegion.CurrentArmies) < OneWayOut.SourceRegion.CurrentArmies)
                {
                    AT.Add(AddAttack(OneWayOut.SourceRegion, OneWayOut.TargetRegion, OneWayOut.SourceRegion.CurrentArmies -1));
                }
            }

            foreach (Region rm in regionsMe)
            {
                // find opponent neighbours (biggest first)
                IEnumerable<Region> ros = rm.Neighbours.Where(N => N.CurrentPlayer.Is(PlayerType.Opponent))
                    .OrderByDescending(N => N.CurrentArmies);
                if (ros.Any())
                {

                        while (rm.CurrentArmies - 1 > ros.First().ArmiesToAttack)
                        {
                            AT.Add(AddAttack(rm, ros.First(), 
                                Math.Max(rm.CurrentArmies / rm.Neighbours.Count(N => N.CurrentPlayer.In(PlayerType.NotMe)), 
                                Combat.AttackersNeeded(ros.First().CurrentArmies + 5))));
                        }
                    
                }
                else
                {
                    
                    // find neutral neighbours (biggest first)
                    IEnumerable<Region> rns = rm.Neighbours.Where(N => N.CurrentPlayer.Is(PlayerType.Neutral))
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

            // move armies to front
            IEnumerable<Region> saveArea = Map.Current.Regions.Where(R => R.DangerousBorderDistance > 1 && R.CurrentArmies > 1 && R.CurrentPlayer.Is(PlayerType.Me));
            foreach (Region saveRegion in saveArea)
            {
                IEnumerable<Region> buren = saveRegion.Neighbours
                    .Where(N => N.CurrentPlayer.In(PlayerType.Me) && (N.DangerousBorderDistance < saveRegion.DangerousBorderDistance))
                    .OrderBy(N => N.DangerousBorderDistance);
                if (buren.Any())
                {
                    AT.Add(AddAttack(saveRegion, buren.First(), saveRegion.CurrentArmies - 1));
                }

            }

            return AT;
        }

        /// <summary>
        /// All connections from Me to Not me
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Connection> MeOther()
        {
            return Map.Current.Connections.Where(C => 
                   C.SourceRegion.CurrentPlayer.Is(PlayerType.Me) 
                && C.TargetRegion.CurrentPlayer.In(PlayerType.NotMe));
        }

        /// <summary>
        /// All connections from Me to Not me
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Connection> MeOneOther()
        {
            return MeOther().GroupBy(C => C.SourceRegion)
                .Where(grp => grp.Count() == 1)
                .Select(grp => grp.First());
        }


        /// <summary>
        /// All connections
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Connection> OpponentMeConnections()
        {
            return Map.Current.Connections.Where(C => C.SourceRegion.CurrentPlayer.Is(PlayerType.Me) && C.TargetRegion.CurrentPlayer.Is(PlayerType.Opponent));
        }

        /// <summary>
        /// Minimal success chance
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Connection> OpponentMeConnectionsMin()
        {
            return OpponentMeConnections()
                .Where(C => C.SourceRegion.Neighbours.Count(N => N.CurrentPlayer.Is(PlayerType.Opponent)) == 1);
        }


        private static AttackTransfer AddAttack(Region regionSource, Region regionTarget, int armies)
        {
            if (armies > (regionSource.CurrentArmies - 1))
            {
                armies = regionSource.CurrentArmies - 1;
            }
            
            regionSource.CurrentArmies -= armies;
            return new AttackTransfer(armies, regionSource, regionTarget);
        }
    }
}
