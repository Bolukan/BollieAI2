using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Board;
using BollieAI2.Strategy;

namespace BollieAI2.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class Parser
    {
        #region Parser

        /// <summary>
        /// Parse output from engine
        /// </summary>
        /// <param name="line">line from engine</param>
        public void Parse(String line)
        {
            String[] parts = line.Split(' ');
            switch (parts[0].ToLowerInvariant())
            {
                case "go":
                    switch (parts[1].ToLowerInvariant())
                    {
                        case "place_armies": // R05
                            PlaceArmies(parts);
                            break;
                        case "attack/transfer": // R06
                            AttackTransfer(parts);
                            break;
                        default:
                            break;
                    }
                    break;
                case "opponent_moves": // R03
                    OpponentMoves(parts);
                    break;
                case "update_map": // R02
                    UpdateMap(parts);
                    break;
                case "settings":
                    switch (parts[1].ToLowerInvariant())
                    {
                        case "timebank":
                            TimeBank(parts); // I01
                            break;
                        case "time_per_move":
                            TimePerMove(parts); // I02
                            break;
                        case "max_rounds": // I03
                            MaxRounds(parts);
                            break;
                        case "your_bot": // I04
                            YourBot(parts);
                            break;
                        case "opponent_bot": // I05
                            OpponentBot(parts);
                            break;
                        case "starting_armies": // R01
                            StartingArmies(parts);
                            break;
                        case "starting_regions": // I10
                            StartingRegions(parts);

                            BollieAI2.Services.StartingRegions.SetStartingRegions();

                            break;
                        default:
                            break;
                    }
                    break;
                case "pick_starting_region": // 11 multiple
                    PickStartingRegion(parts);
                    break;
                case "round": // R04
                    Round(parts);
                    break;
                case "setup_map":
                    switch (parts[1].ToLowerInvariant())
                    {
                        case "super_regions": // I06
                            SuperRegions(parts);
                            break;
                        case "regions": // I07
                            Regions(parts);
                            break;
                        case "neighbors": // I08
                            Neighbors(parts);
                            break;
                        case "wastelands": // I09
                            Wastelands(parts);
                            break;
                        case "opponent_starting_regions": // 12
                            OpponentStartingRegions(parts);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region InitialMap

        /// <summary>
        /// I01
        /// The maximum (and initial) amount of time in the timebank is given in ms. 
        /// </summary>
        /// <param name="parts"></param>
        public void TimeBank(String[] parts)
        {
            Map.Current.TimebankInitial = int.Parse(parts[2]);
        }

        /// <summary>
        /// I02
        /// The amount of time that is added to your timebank each time a move is requested in ms.
        /// </summary>
        /// <param name="parts"></param>
        public void TimePerMove(String[] parts)
        {
            Map.Current.TimePerMove = int.Parse(parts[2]);
        }

        /// <summary>
        /// I03
        /// The maximum amount of rounds in this game. When this number is reached it's a draw. 
        /// </summary>
        /// <param name="parts"></param>
        public void MaxRounds(String[] parts)
        {
            Map.Current.MaxRounds = int.Parse(parts[2]);
        }

        /// <summary>
        /// I04
        /// The name of your bot is given.
        /// </summary>
        /// <param name="parts"></param>
        public void YourBot(String[] parts)
        {
            Map.Current.YourBot = parts[2];
        }

        /// <summary>
        /// I05
        /// The name of your opponent bot is given.
        /// </summary>
        /// <param name="parts"></param>
        public void OpponentBot(String[] parts)
        {
            Map.Current.OpponentBot = parts[2];
        }

        /// <summary>
        /// I06
        /// The superregions are given, with their bonus armies reward.
        /// </summary>
        /// <param name="parts"></param>
        public void SuperRegions(String[] parts)
        {
            // Odd numbers are superregion ids, even numbers are rewards. 
            for (int i = 2; i < parts.Length; i++)
            {
                int superRegionId = int.Parse(parts[i]);
                int bonusArmiesAward = int.Parse(parts[++i]);
                Map.Current.SuperRegions.Add(new SuperRegion() { Id = superRegionId, BonusArmiesAward = bonusArmiesAward });
            }
        }

        /// <summary>
        /// I07
        /// The regions are given, with their parent superregion.
        /// </summary>
        /// <param name="parts"></param>
        public void Regions(String[] parts)
        {
            // Odd numbers are the region ids, even numbers are the superregion ids.
            for (int i = 2; i < parts.Length; i++)
            {
                int regionId = int.Parse(parts[i]);
                int superRegionId = int.Parse(parts[++i]);
                // Find SuperRegion object
                SuperRegion superRegion = Map.Current.SuperRegions
                    .Find(sr => sr.Id == superRegionId);
                // Create Region and add to Map/SuperRegion
                Region region = new Region() { Id = regionId, SuperRegion = superRegion };
                superRegion.Regions.Add(region);
                Map.Current.Regions.Add(region);
            }
        }

        /// <summary>
        /// I08
        /// The connectivity of the regions are given
        /// </summary>
        /// <param name="parts"></param>
        public void Neighbors(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                // first region
                Region firstRegion = Map.Current.Regions.GetId(parts[i]);

                // list of second regions
                String[] neighborStrings = parts[++i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<Region> neighborRegions = neighborStrings.Select(n => Map.Current.Regions.GetId(n));

                // connect them
                foreach(Region secondRegion in neighborRegions)
                {
                    Map.Current.Connections.Add(new Connection(firstRegion, secondRegion));
                    Map.Current.Connections.Add(new Connection(secondRegion, firstRegion));
                    secondRegion.Neighbours.Add(firstRegion);
                    firstRegion.Neighbours.Add(secondRegion);
                }
            }
        }

        /// <summary>
        /// I09
        /// The regions ids of the regions that are wastelands are given.
        /// These are neutral regions with more than 2 armies on them.
        /// </summary>
        /// <param name="parts"></param>
        public void Wastelands(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                Region wasteland = Map.Current.Regions.GetId(parts[i]);

                Map.Current.Wastelands.Add(wasteland);

                // initialise TO BE MOVED SOMEWHERE ELSE
                wasteland.CurrentPlayer = PlayerType.Neutral;
                wasteland.CurrentArmies = Configuration.WASTELAND_ARMIES;
            }
        }

        /// <summary>
        /// I10.
        /// The engine will make a list of regions by picking one random region from each super region.
        /// </summary>
        /// <param name="parts"></param>
        public void StartingRegions(String[] parts)
        {
            // read possible regions
            Regions startingRegions = new Regions();
            for (int i = 2; i < parts.Length; i++)
            {
                startingRegions.Add(Map.Current.Regions.Find(region => region.Id == int.Parse(parts[i])));
            }

            Map.Current.StartingRegions = startingRegions;
        }

        #endregion

        #region StartingRegion

        /// <summary>
        /// Starting regions to be chosen from are given, one region id is to be returned by your bot
        /// </summary>
        /// <param name="parts"></param>
        public void PickStartingRegion(String[] parts)
        {
            Map.Current.CurrentTimebank = int.Parse(parts[1]);

            // read possible regions
            Regions pickRegions = new Regions();
            for (int i = 2; i < parts.Length; i++)
            {
                Region pickRegion = Map.Current.Regions.Find(region => region.Id == int.Parse(parts[i]));
                pickRegions.Add(pickRegion);
            }

            // choose region
            Region iWantThisRegion = BollieAI2.Services.StartingRegions.PickFromRegions(pickRegions);

            // tell server
            Console.WriteLine("{0}", iWantThisRegion.Id);

        }

        /// <summary>
        /// All the regions your opponent has picked to start on, called after distribution of starting regions.
        /// </summary>
        /// <param name="parts"></param>
        public void OpponentStartingRegions(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                Region region = Map.Current.Regions.Find(r => r.Id == int.Parse(parts[i]));
                region.CurrentPlayer = PlayerType.Opponent;
            }
        }

        #endregion

        #region Round

        /// <summary>
        /// R01
        /// The amount of armies your bot can place on the map at the start of this round.
        /// </summary>
        /// <param name="parts"></param>
        public void StartingArmies(String[] parts)
        {
            Map.Current.StartingArmies = int.Parse(parts[2]);
            Map.Current.Round++;
        }

        /// <summary>
        /// R02
        /// Visible map for the bot is given like this: 
        /// region id; player owning region; number of armies. 
        /// </summary>
        /// <param name="parts"></param>
        public void UpdateMap(String[] parts)
        {
            // read updated regions
            List<MapUpdate> mapUpdates = new List<MapUpdate>();
            for (int i = 1; i < parts.Length; i++)
            {
                Region regionUpdate = Map.Current.Regions.GetId(parts[i]);
                PlayerType playerUpdate = Player.PlayerId(parts[++i]);
                int armiesUpdate = int.Parse(parts[++i]);
                mapUpdates.Add(new MapUpdate(regionUpdate, playerUpdate, armiesUpdate));
            }
            // Save updates for implementation AFTER OpponentMoves
            Map.Current.MapUpdates = mapUpdates;
            
        }

        /// <summary>
        /// R03
        /// all the visible moves the opponent has done are given in consecutive order.
        /// </summary>
        /// <param name="parts"></param>
        public void OpponentMoves(String[] parts)
        {
            List<PlaceArmies> opponentPlaceArmies = new List<PlaceArmies>();
            List<AttackTransfer> opponentAttackTransfer = new List<AttackTransfer>();

            for (int i = 1; i < parts.Length; i++)
            {
                PlayerType playerUpdate = Player.PlayerId(parts[i]);
                String command = parts[++i];
                if (command == "place_armies")
                {
                    i++;
                    Region region = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[i])).FirstOrDefault();
                    int armies = int.Parse(parts[++i]);
                    opponentPlaceArmies.Add(new PlaceArmies(armies, region));
                }
                else if (command == "attack/transfer")
                {
                    i++;
                    Region regionSource = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[i])).FirstOrDefault();
                    i++;
                    Region regionTarget = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[i])).FirstOrDefault();
                    int armies = int.Parse(parts[++i]);
                    opponentAttackTransfer.Add(new AttackTransfer(armies, regionSource, regionTarget));
                }
            }

            // Save moves for further use
            Map.Current.OpponentLastPlaceArmies = opponentPlaceArmies;
            Map.Current.OpponentLastAttackTransfer = opponentAttackTransfer;

            // Update values 
            MapUpdate.UpdateOpponent();
            MapUpdate.UpdateMap();

        }

        /// <summary>
        /// R04
        /// Round number broadcasted
        /// </summary>
        /// <param name="parts"></param>
        public void Round(String[] parts)
        {
        }
        
        /// <summary>
        /// R05
        /// Request for the bot to return his place armies moves.
        /// </summary>
        /// <param name="parts"></param>
        public void PlaceArmies(String[] parts)
        {
            CurrentUpdate.UpdateRegion();
            CurrentUpdate.UpdateSuperRegion();
            CurrentUpdate.UpdateMap();
            StrategySuperRegion.UpdateStrategySuperRegion();
                        
            List<PlaceArmies> Pas = StrategyPlaceArmies.SelectPlaceArmies();
            if (Pas.Count() == 0)
            {
                Console.WriteLine("No moves");
            }
            else
            {
                Console.WriteLine(String.Join(",",Pas.Select(pa => pa.ToString())));
            }
            
        }

        /// <summary>
        /// R06
        /// Request for the bot to return his attack and/or transfer moves 
        /// </summary>
        /// <param name="parts"></param>
        public void AttackTransfer(String[] parts)
        {
            List<AttackTransfer> ats = StrategyAttackTransfer.GetAttackTransferList();
            if (ats.Count() == 0)
            {
                Console.WriteLine("No moves");
            }
            else
            {
                Console.WriteLine(String.Join(",", ats.Select(at => at.ToString())));
            }
            // ERROR
        }

        #endregion
    }
}
