using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BollieAI2.Board;

namespace BollieAI2.Strategy
{
    /// <summary>
    /// All pairs of regions with distance
    /// </summary>
    /// <typeparam name="Node"></typeparam>
    public class Graph
    {
        /// <summary>
        /// All neighbour relations = edges
        /// </summary>
        private Dictionary<Region[], double> edges;
        
        /// <summary>
        /// All regions = nodes
        /// </summary>
        public List<Region> regions { get; private set; }
        
        /// <summary>
        /// Number of nodes
        /// </summary>
        public int Length { get { return regions.Count; } }
 
        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="nodes">All regions</param>
        public Graph(List<Region> regions)
        {
            edges = new Dictionary<Region[], double>();
            this.regions = regions;
        }
 
        /// <summary>
        /// Add neighbours distance
        /// </summary>
        /// <param name="a">Region a</param>
        /// <param name="b">Region b</param>
        /// <param name="cost">Distance</param>
        public void Add(Region a, Region b, double cost)
        {
            if (!regions.Contains(a) || !regions.Contains(b))
                throw new Exception("Invalid node");
 
            edges.Add(new[] { a, b }, cost);
        }
 
        /// <summary>
        /// Distance between neighbours (directly!)
        /// </summary>
        /// <param name="a">Region a</param>
        /// <param name="b">Region b</param>
        /// <returns>Distance</returns>
        public double Cost(Region a, Region b)
        {
            foreach (Region[] key in edges.Keys)
            {
                if (key[0].Equals(a) && key[1].Equals(b))
                    return edges[key];
            }
            throw new Exception("Edge not found");
 
        }
    }
 
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Node"></typeparam>
    public class Dijkstra<Node>
    {
        /* Takes adjacency matrix in the following format, for a directed graph (2-D array)
         * Ex. node 1 to 3 is accessible at a cost of 4
         *        0  1  2  3  4
         *   0  { 0, 2, 5, 0, 0},
         *   1  { 0, 0, 0, 4, 0},
         *   2  { 0, 6, 0, 0, 8},
         *   3  { 0, 0, 0, 0, 9},
         *   4  { 0, 0, 0, 0, 0}
         */
 
        /* Resulting arrays with distances to nodes and how to get there */
        public Dictionary<Region, double> dist { get; private set; }
        public Dictionary<Region, Region> path { get; private set; }
        public List<Region> shortest_path { get; private set; }
        Graph G;
 
        /* Holds queue for the nodes to be evaluated */
        private List<Region> queue = new List<Region>();
 
        /* Sets up initial settings */
        private void Initialize(Region s, int len)
        {
            dist = new Dictionary<Region, double>();
            path = new Dictionary<Region, Region>();
 
            /* Set distance to all nodes to infinity - alternatively use Int.MaxValue for use of Int type instead */
            for (int i = 0; i < len; i++)
            {
                dist[G.regions[i]] = Double.PositiveInfinity;
 
                queue.Add(G.regions[i]);
            }
 
            /* Set distance to 0 for starting point and the previous node to null (-1) */
            dist[s] = 0;
            path[s] = default(Region);
        }
 
        /* Retrives next node to evaluate from the queue */
        private Region GetNextVertex()
        {
            double min = Double.PositiveInfinity;
            Region Vertex = default(Region);
 
            /* Search through queue to find the next node having the smallest distance */
            foreach (Region j in queue)
            {
                if (dist[j] <= min)
                {
                    min = dist[j];
                    Vertex = j;
                }
            }
 
            queue.Remove(Vertex);
 
            return Vertex;
 
        }
 
        /* Takes a graph as input an adjacency matrix (see top for details) and a starting node */
        public Dijkstra(Graph G, Region s)
        {
            this.G = G;
 
            int len = G.Length;
 
            /* Check graph format and that the graph actually contains something */
            if (len < 1)
            {
                throw new ArgumentException("Graph error, wrong format or no nodes to compute");
            }
 
            Initialize(s, len);
 
            while (queue.Count > 0)
            {
                Region u = GetNextVertex();
 
                /* Find the nodes that u connects to and perform relax */
                for (int vi = 0; vi < len; vi++)
                {
                    Region v = G.regions[vi];
                    /* Checks for edges with negative weight */
                    if (G.Cost(u, v) < 0)
                    {
                        throw new ArgumentException("Graph contains negative edge(s)");
                    }
 
                    /* Check for an edge between u and v */
                    if (G.Cost(u, v) > 0)
                    {
                        /* Edge exists, relax the edge */
                        if (dist[v] > dist[u] + G.Cost(u, v))
                        {
                            dist[v] = dist[u] + G.Cost(u, v);
                            path[v] = u;
                        }
                    }
                }
            }
        }
 
        /* Return shortest path */
        public ObservableCollection ShortestPath(Region d)
        {
            if (!IsNullable(d))
            {
                Debug.WriteLine("Warning, type <" + typeof(Region).ToString() + "> is not nullable");
            }
            var result = new ObservableCollection<Node>();
            Node u = d;
            var shortest_path = new List<Region>();
            //while (u != null)
            while (!EqualityComparer<Region>.Default.Equals(u, default(Region)))
            {
                shortest_path.Add(u);
                u = path[u];
            }
            shortest_path.Reverse();
 
            shortest_path.ForEach(x => result.Add(x));
 
            return result;
        }
 
        /*
         * Algorithm doesn't work very well if type is not nullable
         * Check http://stackoverflow.com/questions/374651/how-to-check-if-an-object-is-nullable
         */
        static bool IsNullable(Node obj)
        {
            if (obj == null) return true; // obvious
            Type type = typeof(Node);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }
    }
}

