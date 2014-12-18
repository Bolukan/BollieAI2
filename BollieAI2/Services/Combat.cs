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
        
        public const int MAX_ARMIES_IN_BATTLE = 20;

        public const double ATTACKERS_LUCK = 0.6;
        public const double DEFENDERS_LUCK = 0.7;
        
        public const double LOWEST_LUCK = 0;
        public const double LUCK = 0.16;
        public const double HIGHEST_LUCK = 1;

        public struct CombatResult
        {
            public int Attackers;
            public int Defenders;
            public bool AttackWins;
            public int AttackersLeft;
            public int DefendersLeft;
        }

        public static void test()
        {
            CombatResult cr;
            for (int d = 1; d <= 30; d++)
            {
                cr = CalcCombatWorst(AttackersNeededBadLuck(d), d);
                Console.WriteLine("Need {0} for {1} defenders, {2} survive, {3} die", cr.Attackers, cr.Defenders, cr.AttackersLeft, cr.Attackers - cr.AttackersLeft );
            }
        }

        public static int AttackersNeededBadLuck(int defenders)
        {
            int attackers = (int)(0.5 + (defenders * DEFENDERS_LUCK)) + 1;
            while (Math.Round(attackers * ATTACKERS_LUCK * (1 - LUCK), 0) < defenders)
                attackers++;
            return attackers;
        }

        public static CombatResult CalcCombatWorst(int attackers, int defenders)
        {
            CombatResult combatResult;
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

    }
}
