using System;
using System.Collections.Generic;
using System.Linq;
using BollieAI2.Model;
using BollieAI2.Services;

namespace BollieAI2.Simulation
{
    /// <summary>
    /// 
    /// </summary>
    public class Simulation
    {
        /// <summary>
        /// SuperRegion
        /// </summary>
        public SuperRegion SuperRegion { get; set; }

        /// <summary>
        /// List of conquers
        /// </summary>
        public List<Connection> Connections { get; set; }

        /// <summary>
        /// Armies needed to conquer SuperRegion
        /// </summary>
        public int ArmiesNeeded { get; private set; }

        /// <summary>
        /// Rounds needed to conquer SuperRegion
        /// </summary>
        public int RoundsNeeded { get; private set; }

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="superRegion">SuperRegion the simulation is about</param>
        /// <param name="connections">Consecutive conquers</param>
        public Simulation(SuperRegion superRegion, List<Connection> connections)
        {
            SuperRegion = superRegion;
            Connections = new List<Connection>(connections);
        }

        public void Calculate()
        {
            InitCombat();
            CalcAttackWithMore();
            CalcRounds();
        }

        // 01. Armies needed to combat and result
        private void InitCombat()
        {
            // Reset to my armies only
            SuperRegion.Regions.ForEach(R =>
                {
                    R.SimulateArmies = R.CurrentPlayer.Is(PlayerType.Me) ? R.CurrentArmies : 0;
                    R.SimulatePlayer = R.CurrentPlayer;
                }
            );

            // Insert intial net armies
            Connections.ForEach(Conn =>
                {
                    int attackersNeeded = Combat.AttackersNeeded(Conn.TargetRegion.CurrentArmies);
                    int attackersLosses = Combat.AttackersLosses(Conn.TargetRegion.CurrentArmies);
                    Conn.SourceRegion.SimulateArmies -= attackersNeeded;
                    Conn.TargetRegion.SimulateArmies += (attackersNeeded - attackersLosses);
                }
            );
        }

        // 02. Attack with more if needed later
        private void CalcAttackWithMore()
        {
            Connections.ForEach(Conn =>
                {
                    // find predecessor
                    Connection source = Connections.Find(c => c.TargetRegion == Conn.SourceRegion);
                    // we need more armies here !!
                    while (source != null && Conn.SourceRegion.SimulateArmies < 1)
                    {
                        if (source.SourceRegion.SimulateArmies > 1)
                        {
                            int armiesTransfer = Math.Min(
                                (source.SourceRegion.SimulateArmies - 1), 
                                (1 - Conn.SourceRegion.SimulateArmies));
                            Conn.SourceRegion.SimulateArmies += armiesTransfer;
                            source.SourceRegion.SimulateArmies -= armiesTransfer;
                        }
                        // find next predecessor
                        source = Connections.Find(c => c.TargetRegion == source.SourceRegion);
                    }
                }
            );
        }

        // 03.
        public void CalcRounds()
        {

        }
 
     
    }
}
