using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BollieAI2.Board;

namespace BollieAI2.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class Parser
    {
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
                        case "place_armies":
                            PlaceArmies(parts);
                            break;
                        case "attack/transfer":
                            AttackTransfer(parts);
                            break;
                        default:
                            break;
                    }
                    break;
                case "opponent_moves":
                    OpponentMoves(parts);
                    break;
                case "update_map":
                    UpdateMap(parts);
                    break;
                case "settings":
                    switch (parts[1].ToLowerInvariant())
                    {
                        case "timebank":
                            TimeBank(parts);
                            break;
                        case "time_per_move":
                            TimePerMove(parts);
                            break;
                        case "max_rounds":
                            MaxRounds(parts);
                            break;
                        case "your_bot":
                            YourBot(parts);
                            break;
                        case "opponent_bot":
                            OpponentBot(parts);
                            break;
                        case "starting_armies":
                            StartingArmies(parts);
                            break;
                        default:
                            break;
                    }
                    break;
                case "pick_starting_region":
                    PickStartingRegion(parts);
                    break;
                case "setup_map":
                    switch (parts[1].ToLowerInvariant())
                    {
                        case "super_regions":
                            TimeBank(parts);
                            break;
                        case "regions":
                            TimePerMove(parts);
                            break;
                        case "neighbors":
                            MaxRounds(parts);
                            break;
                        case "wastelands":
                            YourBot(parts);
                            break;
                        case "opponent_starting_regions":
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

        /// <summary>
        /// The superregions are given, with their bonus armies reward, all separated by spaces.
        /// Odd numbers are superregion ids, even numbers are rewards. 
        /// </summary>
        /// <param name="parts"></param>
        public void SuperRegions(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                int superRegionId = int.Parse(parts[i]);
                int bonusArmiesAward = int.Parse(parts[++i]);
                Map.Current.SuperRegions.Add(new SuperRegion() { Id = superRegionId, BonusArmiesAward = bonusArmiesAward });
            }
        }

        /// <summary>
        /// The regions are given, with their parent superregion, all separated by spaces.
        /// Odd numbers are the region ids, even numbers are the superregion ids.
        /// </summary>
        /// <param name="parts"></param>
        public void Regions(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                int regionId = int.Parse(parts[i]);
                int superRegionId = int.Parse(parts[++i]);
                SuperRegion superRegion = Map.Current.SuperRegions
                    .Where(sr => sr.Id == superRegionId)
                    .FirstOrDefault();
                Region region = new Region() { Id = regionId, SuperRegion = superRegion };
                superRegion.Regions.Add(region);
                Map.Current.Regions.Add(region);
            }
        }

        /// <summary>
        /// The connectivity of the regions are given, first is the region id.
        /// Then the neighbouring regions' ids, separated by commas.
        /// Connectivity is only given in one way: 'region id' - 'neighbour id'.
        /// </summary>
        /// <param name="parts"></param>
        public void Neighbors(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                int regionId = int.Parse(parts[i]);
                Region firstRegion = Map.Current.Regions
                    .Where(region => region.Id == regionId)
                    .FirstOrDefault();

                String[] neighborStrings = parts[++i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                List<Region> neighborRegions =
                    neighborStrings
                    .Select(n => Map.Current.Regions.Where(region => region.Id == int.Parse(n)).FirstOrDefault())
                    .ToList();

                neighborRegions.ForEach(
                   (secondRegion) =>
                   {
                       secondRegion.Neighbours.Add(firstRegion);
                       firstRegion.Neighbours.Add(secondRegion);
                   }
               );
            }
        }

        /// <summary>
        /// The regions ids of the regions that are wastelands are given.
        /// These are neutral regions with more than 2 armies on them.
        /// </summary>
        /// <param name="parts"></param>
        public void Wastelands(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                int regionId = int.Parse(parts[i]);
                Region wasteland = Map.Current.Regions.Where(region => region.Id == regionId).FirstOrDefault();
                wasteland.CurrentPlayer = PlayerType.Wasteland;
                // keep it wasteland till other information is received
                wasteland.CurrentArmies = Configuration.WASTELAND_ARMIES;
                Map.Current.Wastelands.Add(wasteland);
            }
        }

        /// <summary>
        /// All the regions your opponent has picked to start on, called after distribution of starting regions.
        /// </summary>
        /// <param name="parts"></param>
        public void OpponentStartingRegions(String[] parts)
        {
            for (int i = 2; i < parts.Length; i++)
            {
                Region region = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[i])).FirstOrDefault();
                region.CurrentPlayer = PlayerType.Opponent;
            }
        }

        /// <summary>
        /// Starting regions to be chosen from are given, one region id is to be returned by your bot
        /// </summary>
        /// <param name="parts"></param>
        public void PickStartingRegion(String[] parts)
        {
            Map.Current.CurrentTimebank = int.Parse(parts[1]);

            // read possible regions
            List<Region> pickRegions = new List<Region>();
            for (int i = 2; i < parts.Length; i++)
            {
                Region pickRegion = Map.Current.Regions.Where(region => region.Id == int.Parse(parts[i])).FirstOrDefault();
                pickRegions.Add(pickRegion);
            }
            
            // choose region
            Region iWantThisRegion = PickStartingRegions.PickFromRegions(pickRegions);
            
            // tell server
            Console.WriteLine("{0}", iWantThisRegion.Id);
            iWantThisRegion.CurrentPlayer = PlayerType.Me;
        }

        /// <summary>
        /// The maximum (and initial) amount of time in the timebank is given in ms. 
        /// </summary>
        /// <param name="parts"></param>
        public void TimeBank(String[] parts)
        {
            Map.Current.TimebankInitial = int.Parse(parts[2]);
        }

        /// <summary>
        /// The amount of time that is added to your timebank each time a move is requested in ms.
        /// </summary>
        /// <param name="parts"></param>
        public void TimePerMove(String[] parts)
        {
            Map.Current.TimePerMove = int.Parse(parts[2]);
        }

        /// <summary>
        /// The maximum amount of rounds in this game. When this number is reached it's a draw. 
        /// </summary>
        /// <param name="parts"></param>
        public void MaxRounds(String[] parts)
        {
            Map.Current.MaxRounds = int.Parse(parts[2]);
        }

        /// <summary>
        /// The name of your bot is given.
        /// </summary>
        /// <param name="parts"></param>
        public void YourBot(String[] parts)
        {
            Map.Current.YourBot = parts[2];
        }

        /// <summary>
        /// The name of your opponent bot is given.
        /// </summary>
        /// <param name="parts"></param>
        public void OpponentBot(String[] parts)
        {
            Map.Current.OpponentBot = parts[2];
        }

        /// <summary>
        /// The amount of armies your bot can place on the map at the start of this round.
        /// </summary>
        /// <param name="parts"></param>
        public void StartingArmies(String[] parts)
        {
            Map.Current.StartingArmies = int.Parse(parts[2]);
        }

        /// <summary>
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
                Region regionUpdate = Map.Current.Regions.Where(region => region.Id == int.Parse(parts[i])).FirstOrDefault();
                PlayerType playerUpdate = Player.Player(parts[++i]);
                int armiesUpdate = int.Parse(parts[++i]);
                mapUpdates.Add(new MapUpdate(regionUpdate, playerUpdate, armiesUpdate));
            }

            Map.Current.MapUpdates = mapUpdates;
            
        }

        /// <summary>
        /// all the visible moves the opponent has done are given in consecutive order.
        /// </summary>
        /// <param name="parts"></param>
        public void OpponentMoves(String[] parts)
        {
            List<PlaceArmies> opponentPlaceArmies = new List<PlaceArmies>();
            List<AttackTransfer> opponentAttackTransfer = new List<AttackTransfer>();

            for (int i = 1; i < parts.Length; i++)
            {
                PlayerType playerUpdate = Player.Player(parts[i]);
                String command = parts[++i];
                if (command == "place_armies")
                {
                    Region region = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[++i])).FirstOrDefault();
                    int armies = int.Parse(parts[++i]);
                    opponentPlaceArmies.Add(new PlaceArmies(armies, region);
                }
                else if (command == "attack/transfer")
                {
                    Region regionSource = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[++i])).FirstOrDefault();
                    Region regionTarget = Map.Current.Regions.Where(r => r.Id == int.Parse(parts[++i])).FirstOrDefault();
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
        /// Request for the bot to return his place armies moves.
        /// </summary>
        /// <param name="parts"></param>
        public void PlaceArmies(String[] parts)
        { 
        }

        /// <summary>
        /// Request for the bot to return his attack and/or transfer moves 
        /// </summary>
        /// <param name="parts"></param>
        public void AttackTransfer(String[] parts)
        {
        }


    }
}
