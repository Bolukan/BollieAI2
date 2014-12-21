using System;
using System.Collections.Generic;
using System.Linq;

namespace BollieAI2.Board
{
    public class Regions : List<Region>
    {

        /// <summary>
        /// Initialise
        /// </summary>
        public Regions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Regions"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public Regions(IEnumerable<Region> collection) : base(collection)
        {
        }

        /// <summary>
        /// Find Regions owned by specified player
        /// </summary>
        /// <param name="wherePlayer">Player</param>
        /// <returns>Regions</returns>
        public IEnumerable<Region> Player(PlayerType wherePlayer)
        {
            return this.Where(region => region.CurrentPlayer == wherePlayer);
        }

        /// <summary>
        /// Find Region by Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>Region</returns>
        public Region GetId(int Id)
        {
            return this.Find(region => region.Id == Id);
        }

        /// <summary>
        /// Find Region by Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>Region</returns>
        public Region GetId(String Id)
        {
            return this.Find(region => region.Id == int.Parse(Id));
        }

    }
}
