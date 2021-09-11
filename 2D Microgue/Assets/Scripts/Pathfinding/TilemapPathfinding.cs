/**
 * Represent a grid of nodes we can search paths on.
 * Based on code and tutorial by Sebastian Lague (https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ).
 *   
 * Author: Ronen Ness.
 * Since: 2016. 
*/
using System.Collections.Generic;

namespace NesScripts.Controls.PathFind
{
    /// <summary>
    /// A 2D grid of nodes we use to find path.
    /// The grid mark which tiles are walkable and which are not.
    /// </summary>
    public class Grid
    {
        // nodes in grid
        public Node[,] nodes;

        // grid size
        int gridSizeX, gridSizeY;

        /// <summary>
        /// Create a new grid with tile prices.
        /// </summary>
        /// <param name="tiles_costs">A 2d array of tile prices.
        ///     0.0f = Unwalkable tile.
        ///     1.0f = Normal tile.
        ///     > 1.0f = costy tile.
        ///     < 1.0f = cheap tile.
        /// </param>
        public Grid(float[,] tiles_costs)
        {
            // create nodes
            CreateNodes(tiles_costs.GetLength(0), tiles_costs.GetLength(1));

            // init nodes
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    nodes[x, y] = new Node(tiles_costs[x, y], x, y);
                }
            }
        }

        /// <summary>
        /// Create a new grid without tile prices, eg with just walkable / unwalkable tiles.
        /// </summary>
        /// <param name="walkable_tiles">A 2d array representing which tiles are walkable and which are not.</param>
        public Grid(bool[,] walkable_tiles)
        {
            // create nodes
            CreateNodes(walkable_tiles.GetLength(0), walkable_tiles.GetLength(1));

            // init nodes
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }

        /// <summary>
        /// Create the nodes grid and set size.
        /// </summary>
        /// <param name="width">Nodes grid width.</param>
        /// <param name="height">Nodes grid height.</param>
        private void CreateNodes(int width, int height)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[gridSizeX, gridSizeY];
        }

		/// <summary>
		/// Updates the already created grid with new tile prices.
		/// </summary>
		/// <returns><c>true</c>, if grid was updated, <c>false</c> otherwise.</returns>
		/// <param name="tiles_costs">Tiles costs.</param>
		public void UpdateGrid (float[,] tiles_costs)
        {
            // check if need to re-create grid
            if (nodes == null ||
                gridSizeX != tiles_costs.GetLength(0) ||
                gridSizeY != tiles_costs.GetLength(1))
            {
                CreateNodes(tiles_costs.GetLength(0), tiles_costs.GetLength(1));
            }

            // update nodes
			for (int x = 0; x < gridSizeX; x++)
			{
				for (int y = 0; y < gridSizeY; y++)
				{
					nodes[x, y].Update(tiles_costs[x, y], x, y);
				}
			}
		}

		/// <summary>
		/// Updates the already created grid without new tile prices, eg with just walkable / unwalkable tiles.
		/// </summary>
		/// <returns><c>true</c>, if grid was updated, <c>false</c> otherwise.</returns>
		/// <param name="walkable_tiles">Walkable tiles.</param>
		public void UpdateGrid (bool[,] walkable_tiles)
        {
            // check if need to re-create grid
            if (nodes == null ||
                gridSizeX != walkable_tiles.GetLength(0) ||
                gridSizeY != walkable_tiles.GetLength(1))
            {
                CreateNodes(walkable_tiles.GetLength(0), walkable_tiles.GetLength(1));
            }

            // update grid
			for (int x = 0; x < gridSizeX; x++)
			{
				for (int y = 0; y < gridSizeY; y++)
				{
					nodes[x, y].Update(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
				}
			} 
		}

        /// <summary>
        /// Get all the neighbors of a given tile in the grid.
        /// </summary>
        /// <param name="node">Node to get neighbors for.</param>
        /// <returns>List of node neighbors.</returns>
        public System.Collections.IEnumerable GetNeighbours(Node node, Pathfinding.DistanceType distanceType)
        {
			int x = 0, y = 0;
            switch (distanceType)
            {
                case Pathfinding.DistanceType.Manhattan:
                    y = 0;
                    for (x = -1; x <= 1; ++x)
                    {
                        var neighbor = AddNodeNeighbour(x, y, node);
                        if (neighbor != null)
                            yield return neighbor;
                    }

                    x = 0;
                    for (y = -1; y <= 1; ++y)
                    {
                        var neighbor = AddNodeNeighbour(x, y, node);
                        if (neighbor != null)
                            yield return neighbor;
                    }
                    break;

                case Pathfinding.DistanceType.Euclidean:
                    for (x = -1; x <= 1; x++)
                    {
                        for (y = -1; y <= 1; y++)
                        {
                            var neighbor = AddNodeNeighbour(x, y, node);
                            if (neighbor != null)
                                yield return neighbor;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Adds the node neighbour.
        /// </summary>
        /// <returns><c>true</c>, if node neighbour was added, <c>false</c> otherwise.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="node">Node.</param>
        /// <param name="neighbours">Neighbours.</param>
        Node AddNodeNeighbour(int x, int y, Node node)
        {
            if (x == 0 && y == 0)
            {
                return null;
            }

            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                return nodes[checkX, checkY];
            }

            return null;
        }
    }

/// <summary>
    /// Represent a single node in the pathfinding grid.
    /// </summary>
    public class Node
    {
        // is this node walkable?
        public bool walkable;
        public int gridX;
        public int gridY;
        public float price;

        // calculated values while finding path
        public int gCost;
        public int hCost;
        public Node parent;

        /// <summary>
        /// Create the grid node.
        /// </summary>
        /// <param name="_price">Price to walk on this node (set to 1.0f to ignore).</param>
        /// <param name="_gridX">Node x index.</param>
        /// <param name="_gridY">Node y index.</param>
        public Node(float _price, int _gridX, int _gridY)
        {
            walkable = _price != 0.0f;
            price = _price;
            gridX = _gridX;
            gridY = _gridY;
        }

        /// <summary>
        /// Create the grid node.
        /// </summary>
        /// <param name="_walkable">Is this tile walkable?</param>
        /// <param name="_gridX">Node x index.</param>
        /// <param name="_gridY">Node y index.</param>
        public Node(bool _walkable, int _gridX, int _gridY)
        {
            walkable = _walkable;
            price = _walkable ? 1f : 0f;
            gridX = _gridX;
			gridY = _gridY;
        }

		/// <summary>
		/// Updates the grid node.
		/// </summary>
		/// <param name="_price">Price to walk on this node (set to 1.0f to ignore).</param>
		/// <param name="_gridX">Node x index.</param>
		/// <param name="_gridY">Node y index.</param>
		public void Update(float _price, int _gridX, int _gridY) {
			walkable = _price != 0.0f;
			price = _price;
			gridX = _gridX;
			gridY = _gridY;
		}

		/// <summary>
		/// Updates the grid node.
		/// </summary>
		/// <param name="_walkable">Is this tile walkable?</param>
		/// <param name="_gridX">Node x index.</param>
		/// <param name="_gridY">Node y index.</param>
		public void Update(bool _walkable, int _gridX, int _gridY) {
			walkable = _walkable;
			price = _walkable ? 1f : 0f;
			gridX = _gridX;
			gridY = _gridY;
		}

        /// <summary>
        /// Get fCost of this node.
        /// </summary>
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }

    /// <summary>
    /// Main class to find the best path to walk from A to B.
    /// 
    /// Usage example:
    /// Grid grid = new Grid(width, height, tiles_costs);
    /// List<Point> path = Pathfinding.FindPath(grid, from, to);
    /// </summary>
    public class Pathfinding
    {
        /// <summary>
        /// Different ways to calculate path distance.
        /// </summary>
		public enum DistanceType
		{
            /// <summary>
            /// The "ordinary" straight-line distance between two points.
            /// </summary>
			Euclidean,

            /// <summary>
            /// Distance without diagonals, only horizontal and/or vertical path lines.
            /// </summary>
			Manhattan
        }

        /// <summary>
        /// Find a path between two points.
        /// </summary>
        /// <param name="grid">Grid to search.</param>
        /// <param name="startPos">Starting position.</param>
		/// <param name="targetPos">Ending position.</param>
        /// <param name="distance">The type of distance, Euclidean or Manhattan.</param>
        /// <param name="ignorePrices">If true, will ignore tile price (how much it "cost" to walk on).</param>
        /// <returns>List of points that represent the path to walk.</returns>
		public static List<Point> FindPath(Grid grid, Point startPos, Point targetPos, DistanceType distance = DistanceType.Euclidean, bool ignorePrices = false)
        {
            // find path
            List<Node> nodes_path = _ImpFindPath(grid, startPos, targetPos, distance, ignorePrices);

            // convert to a list of points and return
            List<Point> ret = new List<Point>();
            if (nodes_path != null)
            {
                foreach (Node node in nodes_path)
                {
                    ret.Add(new Point(node.gridX, node.gridY));
                }
            }
            return ret;
        }

        /// <summary>
        /// Internal function that implements the path-finding algorithm.
        /// </summary>
        /// <param name="grid">Grid to search.</param>
        /// <param name="startPos">Starting position.</param>
        /// <param name="targetPos">Ending position.</param>
        /// <param name="distance">The type of distance, Euclidean or Manhattan.</param>
        /// <param name="ignorePrices">If true, will ignore tile price (how much it "cost" to walk on).</param>
        /// <returns>List of grid nodes that represent the path to walk.</returns>
        private static List<Node> _ImpFindPath(Grid grid, Point startPos, Point targetPos, DistanceType distance = DistanceType.Euclidean, bool ignorePrices = false)
        {
            Node startNode = grid.nodes[startPos.x, startPos.y];
            Node targetNode = grid.nodes[targetPos.x, targetPos.y];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(grid, startNode, targetNode);
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode, distance))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (ignorePrices ? 1 : (int)(10.0f * neighbour.price));
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Retrace path between two points.
        /// </summary>
        /// <param name="grid">Grid to work on.</param>
        /// <param name="startNode">Starting node.</param>
        /// <param name="endNode">Ending (target) node.</param>
        /// <returns>Retraced path between nodes.</returns>
        private static List<Node> RetracePath(Grid grid, Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Get distance between two nodes.
        /// </summary>
        /// <param name="nodeA">First node.</param>
        /// <param name="nodeB">Second node.</param>
        /// <returns>Distance between nodes.</returns>
        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = System.Math.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = System.Math.Abs(nodeA.gridY - nodeB.gridY);
            return (dstX > dstY) ? 
                14 * dstY + 10 * (dstX - dstY) :
                14 * dstX + 10 * (dstY - dstX);
        }
    }

    /// <summary>
    /// A 2d point on the grid
    /// </summary>
    public struct Point
    {
        // point X
        public int x;

        // point Y
        public int y;

        /// <summary>
        /// Init the point with values.
        /// </summary>
        public Point(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
        }

        /// <summary>
        /// Init the point with a single value.
        /// </summary>
        public Point(Point b)
        {
            x = b.x;
            y = b.y;
        }

        /// <summary>
        /// Get point hash code.
        /// </summary>
        public override int GetHashCode()
        {
            return x ^ y;
        }

        /// <summary>
        /// Compare points.
        /// </summary>
        public override bool Equals(System.Object obj)
        {
            // check type
            if (!(obj.GetType() == typeof(PathFind.Point)))
                 return false;

            // check if other is null
            Point p = (Point)obj;
            if (ReferenceEquals(null, p))
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        /// <summary>
        /// Compare points.
        /// </summary>
        public bool Equals(Point p)
        {
            // check if other is null
            if (ReferenceEquals(null, p))
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        /// <summary>
        /// Check if points are equal in value.
        /// </summary>
        public static bool operator ==(Point a, Point b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (ReferenceEquals(null, a))
            {
                return false;
            }
            if (ReferenceEquals(null, b))
            {
                return false;
            }
            // Return true if the fields match:
            return a.x == b.x && a.y == b.y;
        }

        /// <summary>
        /// Check if points are not equal in value.
        /// </summary>
        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Set point value.
        /// </summary>
        public Point Set(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
            return this;
        }
    }
}