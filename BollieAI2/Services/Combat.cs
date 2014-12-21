using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BollieAI2.Services
{
    /// <summary>
    /// Stats about result of Combat
    /// </summary>
    public class Combat
    {
        public const double LUCK = 0.16;

        public const double ATTACKERS_CHANCE = 0.6;
        public const double ATTACKERS_LUCK = LUCK;

        public const double DEFENDERS_CHANCE = 0.7;
        public const double DEFENDERS_LUCK = LUCK;

        public enum ChancesType
        {
            Attacker = 0,
            Defender,
            AttackerCum,
            DefenderCum
        }

        // 95% or more success
        private static int[] AttackersNeededSmall = { 0, 2, 3, 5, 7, 9, 10, 12, 14, 15, 17, 19, 21, 22, 24, 26, 27, 29 };
        public static int AttackersNeeded(int defenders)
        {
            if (defenders > 17)
            {
                // this is not scientific
                return (AttackersNeededSmall[17] + (int)((((defenders-17)*5)+1.7D)/3));
            }
            else
            {
                return AttackersNeededSmall[defenders];
            }
        }

        private static Dictionary<int, Dictionary<int, double[]>> Chances;


        private static Dictionary<int, Dictionary<int, double>> AttackChances;
        private static Dictionary<int, Dictionary<int, double>> DefendChances;
        private static int AttackChancesMax = 0;
        private static int DefendChancesMax = 0;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="armiesAttackers"></param>
        /// <returns>chances of result</returns>
        public static Dictionary<int, double> GetAttackChances(int armiesAttackers)
        {
            while (armiesAttackers > AttackChancesMax)
            {
                AttackChancesMax++;
                Dictionary<int, double> attackChances = new Dictionary<int, double>(AttackChancesMax + 1);
                // Calculate chances
                if (AttackChancesMax == 1)
                {
                    AttackChances = new Dictionary<int, Dictionary<int, double>>();
                    attackChances.Add(0, 1D - ATTACKERS_CHANCE);
                    attackChances.Add(1, ATTACKERS_CHANCE);
                }
                else
                {
                    // again a loss
                    attackChances.Add(0, (1D - ATTACKERS_CHANCE) * AttackChances[AttackChancesMax-1][0]);
                    // a loss + a win
                    for (int armies = 1; armies < AttackChancesMax; armies++)
                    {
                        attackChances.Add(armies, (1D - ATTACKERS_CHANCE) * AttackChances[AttackChancesMax - 1][armies]
                            + ATTACKERS_CHANCE * AttackChances[AttackChancesMax - 1][armies-1]);
                    }
                    // win only
                    attackChances.Add(AttackChancesMax, ATTACKERS_CHANCE * AttackChances[AttackChancesMax - 1][AttackChancesMax - 1]);
                }
                // add to memory
                AttackChances.Add(AttackChancesMax, attackChances);
            }
            // return from info
            return AttackChances[armiesAttackers];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="armiesDefenders"></param>
        /// <returns>chances of result</returns>
        public static Dictionary<int, double> GetDefendChances(int armiesDefenders)
        {
            while (armiesDefenders > DefendChancesMax)
            {
                DefendChancesMax++;
                Dictionary<int, double> defendChances = new Dictionary<int, double>(DefendChancesMax + 1);
                // Calculate chances
                if (DefendChancesMax == 1)
                {
                    DefendChances = new Dictionary<int, Dictionary<int, double>>();
                    defendChances.Add(0, 1D - DEFENDERS_CHANCE);
                    defendChances.Add(1, DEFENDERS_CHANCE);
                }
                else
                {
                    // again a loss
                    defendChances.Add(0, (1D - DEFENDERS_CHANCE) * DefendChances[DefendChancesMax - 1][0]);
                    // a loss + a win
                    for (int armies = 1; armies < DefendChancesMax; armies++)
                    {
                        defendChances.Add(armies, (1D - DEFENDERS_CHANCE) * DefendChances[DefendChancesMax - 1][armies]
                            + DEFENDERS_CHANCE * DefendChances[DefendChancesMax - 1][armies - 1]);
                    }
                    // a win
                    defendChances.Add(DefendChancesMax, DEFENDERS_CHANCE * DefendChances[DefendChancesMax - 1][DefendChancesMax - 1]);
                }
                // add to memory
                DefendChances.Add(DefendChancesMax, defendChances);
            }
            // return from info
            return DefendChances[armiesDefenders];
        }

/*
        public static void test()
        {
            CombatResult cr;
            for (int d = 1; d <= 30; d++)
            {
                cr = CalcCombatWorst(AttackersNeededBadLuck(d), d);
                Console.WriteLine("Need {0} for {1} defenders, {2} survive, {3} die", cr.Attackers, cr.Defenders, cr.AttackersLeft, cr.Attackers - cr.AttackersLeft );
            }
        }
*/
        public static int AttackersNeededBadLuck(int defenders)
        {
            int attackers = (int)(0.5 + (defenders * DEFENDERS_LUCK)) + 1;
            while (Math.Round(attackers * ATTACKERS_LUCK * (1 - LUCK), 0) < defenders)
                attackers++;
            return attackers;
        }
/*
        public static CombatResult CalcCombatWorst(int attackers, int defenders)
        {
            CombatResult combatResult = new CombatResult();
            combatResult.Attackers = attackers;
            combatResult.Defenders = defenders;

            int defendersCanBeDestroyed = (int)(0.5 + (attackers * ATTACKERS_LUCK * (1 - LUCK)));
            int attackersCanBeDestroyed = (int)(0.5 + (defenders * DEFENDERS_LUCK));
            
            // attackers left
            if (attackers > attackersCanBeDestroyed)
            {
                // defenders gone
                if (defenders > defendersCanBeDestroyed)
                {
                    // We survived, but also they did
                    combatResult.AttackWins = false;
                    combatResult.AttackersLeft = attackers - attackersCanBeDestroyed;
                    combatResult.DefendersLeft = defenders - defendersCanBeDestroyed;
                }
                else
                {
                    // YEAH WE WIN !!
                    combatResult.AttackWins = true;
                    combatResult.AttackersLeft = attackers - attackersCanBeDestroyed;
                    combatResult.DefendersLeft = 0;
                }
            }
            else
            {
                // oops too many
                combatResult.AttackWins = false;
                combatResult.AttackersLeft = 0;
                // a few or more survived
                if (defendersCanBeDestroyed < defenders)
                {
                    combatResult.DefendersLeft = defenders - defendersCanBeDestroyed;
                }
                else
                {
                    // if only one of us had survived to see them all killed. One will stay.
                    combatResult.DefendersLeft = 1;
                }
            }

            return combatResult;

        }
*/
    }
}
