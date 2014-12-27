using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;
using BollieAI2.Services;
using BollieAI2.State;
using BollieAI2.Strategy;

namespace BollieAI2.Simulation
{
    // Temporary class to analyse micro-solution of SR
    public class AnalyseSuperRegion
    {
        private static List<List<Connection>> Solutions = new List<List<Connection>>();
        
        //private static 
        private static int counter;

        
        // initialise
        public AnalyseSuperRegion()
        {
        }

        public static void ReciproceConn(IEnumerable<Region> regionsToCombat, IEnumerable<Connection> connectionsFromMeToNotMe, List<Connection> SolutionConnections)
        {
            // any regions left?
            if (regionsToCombat.Any())
            {
                // go for all connections
                foreach (Connection myConn in connectionsFromMeToNotMe)
                {
                    SolutionConnections.Add(myConn);
                    // set them on conquered
                    myConn.TargetRegion.SimulatePlayer = PlayerType.Me;
                    // try next region
                    ReciproceConn(regionsToCombat, connectionsFromMeToNotMe, SolutionConnections);
                    // reset region from solution
                    myConn.TargetRegion.SimulatePlayer = PlayerType.Neutral;
                    SolutionConnections.Remove(myConn);
                }
            }
            else
            {
                // Yeah, add to solutions for this superregion
                Solutions.Add(new List<Connection>(SolutionConnections));
            }
        }


        /// <summary>
        /// Copy Current to Simulate
        /// </summary>
        /// <param name="superRegion"></param>
        public static void CopyCurrent2Simulate()
        {
            Map.Current.Regions.ForEach(R =>
            {
                R.SimulateArmies = R.CurrentArmies;
                R.SimulatePlayer = R.CurrentPlayer;
            }
            );
        }

        /// <summary>
        /// Copy Current to Simulate
        /// </summary>
        /// <param name="superRegion"></param>
        public static void CopyCurrent2Simulate(SuperRegion superRegion)
        {
            superRegion.Regions.ForEach(R =>
                {
                    R.SimulateArmies = R.CurrentArmies;
                    R.SimulatePlayer = R.CurrentPlayer;
                }
            );
        }


        public static void TestSuperRegion(SuperRegion superRegion)
        {
            CopyCurrent2Simulate();

            // Target regions - (SuperRegion, but not me)
            IEnumerable<Region> regionsToCombat = superRegion.Regions
                .Where(R => R.SimulatePlayer.In(PlayerType.NotMe));
        
            // Attacklines (Me -> NotMe)
            IEnumerable<Connection> connectionsFromMeToNotMe = Map.Current.Connections
                .Where(CN => 
                    CN.SourceRegion.SimulatePlayer.Is(PlayerType.Me) && 
                    CN.TargetRegion.SimulatePlayer.In(PlayerType.NotMe) && 
                    CN.TargetRegion.SuperRegion == superRegion);

            List<Connection> SolutionConnections = new List<Connection>(regionsToCombat.Count());

            ReciproceConn(regionsToCombat, connectionsFromMeToNotMe, SolutionConnections);
            
            Console.WriteLine("SR: {0} #R: {1} #R+: {2} #CN: {3} Solutions: {4}", 
                superRegion.Id, 
                superRegion.Regions.Count(),
                regionsToCombat.Count(), 
                connectionsFromMeToNotMe.Count(),
                Solutions.Count());

            Solutions.Clear(); 
            
        }

    }
}
