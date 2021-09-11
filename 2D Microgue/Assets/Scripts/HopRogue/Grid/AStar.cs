using System.Collections.Generic;
using UnityEngine;
using HopRogue.Entities;

// https://nomadhermitsgrove.prometheaninteractive.com/2020/05/10/building-a-roguelike-from-scratch-unity-tutorial-part-9-a-algorithm/
// NOTE Change from Vector3Int to Vector2Int if not needing to allow for 3D movement

/// <summary>
/// <para> 1. Creates open list of all tiles being evaluated and closed list of all tiles already evaluated. </para>
/// <para> 2. Gets neighbors from a start position and adds them to the open list. </para>
/// <para> 3. Calculates H, G, and F costs of each tile when adding to the open list. </para>
/// <para> 4. Assigns neighbors that its parent is the tile we're coming from. </para>
/// <para> 5. Gets the tile with cheapest F cost, adds it to the closed list, and removes it from the open list. </para>
/// <para> 6. Gets that new tile's neighbors. </para>
/// <para> 7. Repeats steps 3-6 until the target is on the open list. </para>
/// <para> 8. Retraces the path from target's parent back to the starting point. </para>
/// </summary>

namespace HopRogue.Grid
{
public class AStar
{
    List<AStarNode> openList; // ! Consider implementing as a binary min heap to increase performance
    List<AStarNode> closedList;
    AStarNode[,] allNodes;
    int width;
    int height;

    Entity _targetEntity;
    List<Entity> _entitiesToIgnore;
    Dungeon _dungeon;

    public AStar(Dungeon dungeon)
    {
        _dungeon = dungeon;
    }

    public List<Vector3Int> CalculatePath(Vector3Int start, Vector3Int target, Entity targetEntity = null, List<Entity> entitiesToIgnore = null)
    {
        _targetEntity     = targetEntity;
        _entitiesToIgnore = entitiesToIgnore;

        // Get dungeon dimensions
        width  = _dungeon.Width;
        height = _dungeon.Height;

        // Initialize path
        List<Vector3Int> path = new List<Vector3Int>();

        // Initialize collections
        openList   = new List<AStarNode>();
        closedList = new List<AStarNode>();
        allNodes   = new AStarNode[width, height];

        // Initialize array
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                allNodes[x, y] = new AStarNode();
            }
        }

        // Initialize first node and add to closed list
        AStarNode firstNode = new AStarNode() { position = start, gCost = 0, isAlreadyChecked = true };
        closedList.Add(firstNode);

        List<Vector3Int> walkableNeighbors = GetCardinalValidNeighbors(firstNode.position); // ! Warning this may allocate lots of garbage, consider moving inline to this method

        // Calculate costs of each walkable neighbor
        foreach (Vector3Int position in walkableNeighbors)
        {
            int neighborHCost = CalculateHCost(position, target);
            int neighborGCost = CalculateGCost(firstNode, position);

            AStarNode node    = new AStarNode() { position = position, parent = firstNode, hCost = neighborHCost, gCost = neighborGCost };
            node.fCost        = node.hCost + node.gCost;

            allNodes[position.x, position.y] = node;
            node.isBeingChecked = true;
            openList.Add(node);
        }

        // Node that will be assigned when target is found
        AStarNode lastNode = new AStarNode();

        // Find and store the cheapest node
        while (openList.Count > 0)
        {
            AStarNode node = openList[0];

            foreach (AStarNode pathNode in openList)
            {
                bool pathNodeIsCheaper = (node.fCost > pathNode.fCost) || (node.fCost == pathNode.fCost && node.hCost > pathNode.hCost);

                if (pathNodeIsCheaper)
                    node = pathNode;
            }

            // Break out of loop if target node is reached
            if (node.position == target)
            {
                lastNode = node;
                break;
            }

            // Remove node from open list and add it to closed list
            closedList.Add(node);
            allNodes[node.position.x, node.position.y].isAlreadyChecked = true;

            openList.Remove(node);
            allNodes[node.position.x, node.position.y].isBeingChecked = false;

            // Check all neighbors of chosen node and break loop if target is found
            bool foundTarget = false;

            foreach (Vector3Int position in GetCardinalValidNeighbors(node.position))
            {
                if (position == target)
                {
                    foundTarget = true;
                    break;
                }

                int neighborHCost = CalculateHCost(position, target);
                int neighborGCost = CalculateGCost(node, position);

                AStarNode neighbor = new AStarNode() { position = position, parent = node, gCost = neighborGCost, hCost = neighborHCost, isBeingChecked = true };
                neighbor.fCost = neighbor.hCost + neighbor.gCost;

                allNodes[position.x, position.y] = neighbor;
                openList.Add(neighbor);
            }

            if (foundTarget)
            {
                openList.Clear();
                lastNode = node;
                break;
            }
        }

        if (lastNode.parent == null)
            return null;

        // path.Add(target); // * Remove if only moving adjacent to target

        RetracePath(path, start, lastNode);

        return path;
    }

    /// <summary>
    /// Gets current node (last node passed through), adds its position to the path list, then changes it into its parent until start is found
    /// </summary>
    private static void RetracePath(List<Vector3Int> path, Vector3Int start, AStarNode lastNode)
    {
        AStarNode currentNode = lastNode;

        while (currentNode.position != start)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
    }

    /// <summary>
    /// Checks 3x3 grid of tile around parent tile for walkable neighbors
    /// </summary>
    private List<Vector3Int> GetAllValidNeighbors(Vector3Int parent)
    {
        List<Vector3Int> validNeighbors = new List<Vector3Int>();

        for (int y = parent.y - 1; y <= parent.y + 1; y++)
        {
            for (int x = parent.x - 1; x <= parent.x + 1; x++)
            {
                HopTile currentTile = _dungeon.Tiles[x, y];

                if (!TileIsWithinBounds(currentTile)) { continue; }

                if (currentTile == null) { continue; }
                
                if (!currentTile.IsWalkable) { continue; }

                if (!allNodes[x, y].isBeingChecked && !allNodes[x, y].isAlreadyChecked)
                {
                    validNeighbors.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        return validNeighbors;
    }

    /// <summary>
    /// Checks cardinal tiles (NESW) around parent tile for walkable neighbors
    /// </summary>
    private List<Vector3Int> GetCardinalValidNeighbors(Vector3Int parent)
    {
        List<Vector3Int> validNeighbors = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left }; // TODO Shuffle direction list for less predictable pathfinding

        for (int i = 0; i < directions.Length; i++)
        {
            int x = parent.x + directions[i].x;
            int y = parent.y + directions[i].y;

            if (TileIsValid(_dungeon.Tiles[x, y]) && !allNodes[x, y].isBeingChecked && !allNodes[x, y].isAlreadyChecked)
                validNeighbors.Add(new Vector3Int(x, y, 0));
        }

        return validNeighbors;
    }

    private bool TileIsValid(HopTile tile) 
    {
        if (!TileIsWithinBounds(tile))
            return false;

        if (tile == null)
            return false;

        if (!tile.IsWalkable)
            return false;

        if (TileIsOccupied(tile))
            return false;

        return true;
    }

    bool TileIsOccupied(HopTile tile)
    {
        foreach (Entity entity in GetAllEntitiesInDungeon())
        {
            if (EntityIsOccupyingTile(entity, tile) && entity != _targetEntity)
                return true;
        }

        return false;
    }

    Dictionary<Vector3Int, Entity>.ValueCollection GetAllEntitiesInDungeon() => _dungeon.EntityDictionary.Values;

    bool EntityIsOccupyingTile(Entity entity, HopTile tile) => entity.WorldPosition.x == tile.X && entity.WorldPosition.y == tile.Y;

    /// <summary>
    /// Calculates heuristic cost which is the distance between a position and a target without considering obstacles
    /// </summary>
    private int CalculateHCost(Vector3Int position, Vector3Int target)
    {
        int hCost = 0;
        int x = Mathf.Abs(position.x - target.x);
        int y = Mathf.Abs(position.y - target.y);

        hCost = x + y;

        return hCost;
    }

    private int CalculateGCost(AStarNode parent, Vector3Int position)
    {
        bool isDiagonal = position.x != parent.position.x && position.y != parent.position.y;
        int localG      = isDiagonal ? 14 : 10;
        int gCost       = parent.fCost + localG;

        return gCost;
    }

    private bool TileIsWithinBounds(HopTile tile) => tile.X >= 0 && tile.Y >= 0 && tile.X < width && tile.Y < height;
}
}