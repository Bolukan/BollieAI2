using System;
using System.Collections.Generic;
using System.Linq;

namespace BollieAI2.Services
{
    /// <summary>
    /// Stats about result of Combat
    /// </summary>
    public class Combat
    {
        #region quick

        // 95% or more success
        private static int[] AttackersNeededSmall = { 0, 
                     2, 3, 5, 7, 8, 10, 12, 14, 15, 17, 
                     19, 21, 22, 24, 26, 27, 29, 31, 32, 34,
                     36, 38, 39, 41, 43, 44, 46, 48, 49, 51,
                     53, 55, 56, 58, 60, 61, 63, 65, 66, 68};

        // 95% or more confidence max loss attacker killed by defenders
        private static int[] AttackersLossesSmall = { 0,
                     1, 1, 2, 3, 4, 4, 5, 6, 7, 7,
                     8, 9, 10, 10, 11, 12, 12, 13, 14, 14,
                     15, 16, 17, 17, 18, 19, 20, 20, 21, 22,
                     22, 23, 24, 24, 25, 26, 27, 27, 28, 29};

        /// <summary>
        /// Number of attackers needed to kill all defenders with 95% or more confidence
        /// </summary>
        /// <param name="defenders"># defenders</param>
        /// <returns># attackers needed</returns>
        public static int AttackersNeeded(int defenders)
        {
            if (defenders > 40)
            {
                return (int)(0.5 + (defenders + 1) * 0.71D);
            }
            else
            {
                return AttackersNeededSmall[defenders];
            }
        }

        /// <summary>
        /// Number of attackers at 95% confidence (so kind of max value) that will be killed by defenders
        /// </summary>
        /// <param name="defenders"># defenders</param>
        /// <returns># attackers killed</returns>
        public static int AttackersLosses(int defenders)
        {
            if (defenders > 40)
            {
                return (int)(0.5 + (defenders + 1) * 5 / 3);
            }
            else
            {
                return AttackersLossesSmall[defenders];
            }
        }

        #endregion

        public const double LUCK = 0.16;

        public const double ATTACKERS_CHANCE = 0.6;
        public const double ATTACKERS_LUCK = LUCK;

        public const double DEFENDERS_CHANCE = 0.7;
        public const double DEFENDERS_LUCK = LUCK;

        public enum ChancesType
        {
            Attacker = 0,
            Defender = 1
        }
        
        // Attack, Defend
        // #armies (1 .. n)
        // #outcome (0 .. #armies)
        // chance single/cumulative (=cum(single_outcome(0..outcome))
        private static Dictionary<ChancesType, Dictionary<int, Dictionary<int, double[]>>> Chances =
             new Dictionary<ChancesType, Dictionary<int, Dictionary<int, double[]>>>(2);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="armiesAttackers"></param>
        /// <returns>chances of result</returns>
        public static Dictionary<int, double[]> GetChances(ChancesType chancesType, int armiesIngame)
        {
            // init Chance
            double Chance;
            if (chancesType == ChancesType.Attacker) Chance = ATTACKERS_CHANCE; else Chance = DEFENDERS_CHANCE;

            // Count number existing
            if (!Chances.ContainsKey(chancesType))
            {
                Chances.Add(chancesType, new Dictionary<int, Dictionary<int, double[]>>(armiesIngame));
            }

            int armiesMax = Chances[chancesType].Count();

           
            // Add new calculations till armies reached
            while (armiesMax < armiesIngame)
            {
                // create next outcome 'column'
                armiesMax++;
                
                // Create outcome variable: outcome is 0 .. # armies, which is 1 more than # armies
                Dictionary<int, double[]> outcome = new Dictionary<int, double[]>(armiesMax + 1);
                
                // 1 army is just the Chance
                if (armiesMax == 1)
                {
                    outcome.Add(0, new double[2] { 1-Chance, 1-Chance });
                    outcome.Add(1, new double[2] { Chance, 1 });
                }
                else
                {
                    // calculations use results from 'armies-1'
                    Dictionary<int, double[]> priorOutcome = Chances[chancesType][armiesMax-1];
                    
                    // all losses (outcome still '0')
                    int armies = 0;
                    double singleChance = (1 - Chance) * priorOutcome[armies][0]; // [0] Single outcome
                    outcome.Add(0, new double[2] { singleChance, singleChance } );
                    // loss + win
                    for (armies = 1; armies < armiesMax; armies++)
                    {
                        singleChance = (1-Chance) * priorOutcome[armies][0] + Chance * priorOutcome[armies-1][0]; // [0] single
                        outcome.Add(armies, new double[2] { singleChance, singleChance + outcome[armies-1][1] }); // [1] cumulative
                    }
                    // all wins
                    outcome.Add(armiesMax, new double[2] { Chance * priorOutcome[armiesMax-1][0], 1 });
                }
            
                // Add this to memory
                Chances[chancesType].Add(armiesMax, outcome);
            }

            // return from info
            return Chances[chancesType][armiesIngame];
        }
     
    }
}
