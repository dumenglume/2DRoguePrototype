using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue.Grid
{
public class BFS
{
    static List<BFSNode> _toBeCheckedList;
    static List<BFSNode> _alreadyCheckedList;
    static BFSNode[,] _allBFSNodes;
    static int width;
    static int height;

    public static List<Vector3Int> CalculateValidPositions(Vector3Int start, int rangeMin, int rangeMax, Dungeon dungeon)
    {
        width  = dungeon.Width;
        height = dungeon.Height;

        List<Vector3Int> validPositions = new List<Vector3Int>();

        _allBFSNodes        = new BFSNode[width, height];
        _toBeCheckedList    = new List<BFSNode>();
        _alreadyCheckedList = new List<BFSNode>();

        // Initialize array
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                _allBFSNodes[x, y] = new BFSNode();

        // Assign starting node
        BFSNode firstNode = new BFSNode() { position = start, isBeingChecked = true };
        _toBeCheckedList.Add(firstNode);

        while (_toBeCheckedList.Count > 0)
        {
            BFSNode currentNode          = _toBeCheckedList[0];
            currentNode.isBeingChecked   = false;
            currentNode.isAlreadyChecked = true;

            _toBeCheckedList.Remove(currentNode);
            _alreadyCheckedList.Add(currentNode);

            foreach (Vector3Int neighborPosition in GetCardinalValidNeighbors(currentNode.position, dungeon))
            {
                if (neighborPosition == firstNode.position) continue; // Ignore starting position

                /*
                float neighborDistance = Vector3Int.Distance(neighborPosition, firstNode.position);
                float roundedDistance  = Mathf.Ceil(neighborDistance);
                Debug.Log($"{neighborPosition} distance: {neighborDistance} rounded to {roundedDistance}");
                if (roundedDistance < rangeMin || roundedDistance > rangeMax) continue;
                */

                BFSNode neighborNode = new BFSNode() { position = neighborPosition, parent = currentNode, isBeingChecked = true };
                neighborNode.distance += 1;

                _toBeCheckedList.Add(neighborNode);

                if (neighborNode.distance > rangeMin && neighborNode.distance <= rangeMax)
                {
                    _allBFSNodes[neighborPosition.x, neighborPosition.y] = neighborNode;
                    validPositions.Add(neighborPosition);
                }
            }

            /*
            foreach (HopTile neighborTile in GetCardinalValidTiles(currentNode.position, dungeon))
            {
                if (neighborTile.Position == firstNode.position) continue; // Ignore starting position

                BFSNode neighborNode = new BFSNode() { position = neighborTile.Position, parent = currentNode, isBeingChecked = true };
                neighborNode.distance += 1;
                _allBFSNodes[neighborTile.Position.x, neighborTile.Position.y] = neighborNode;
                validPositions.Add(neighborTile.Position);
                _toBeCheckedList.Add(neighborNode);
            }
            */
        }

        return validPositions;
    }

    private static List<Vector3Int> GetCardinalValidNeighbors(Vector3Int parent, Dungeon dungeon)
    {
        List<Vector3Int> validNeighbors = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left }; // TODO Shuffle direction list for less predictable pathfinding

        for (int i = 0; i < directions.Length; i++)
        {
            int x = parent.x + directions[i].x;
            int y = parent.y + directions[i].y;

            if (TileIsValid(dungeon.Tiles[x, y]) && !_allBFSNodes[x, y].isBeingChecked && !_allBFSNodes[x, y].isAlreadyChecked)
                validNeighbors.Add(new Vector3Int(x, y, 0));
        }

        return validNeighbors;
    }

    private static List<HopTile> GetCardinalValidTiles(Vector3Int parent, Dungeon dungeon)
    {
        List<HopTile> validNeighbors = new List<HopTile>();

        Vector3Int[] directions = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left }; // TODO Shuffle direction list for less predictable pathfinding

        for (int i = 0; i < directions.Length; i++)
        {
            int x = parent.x + directions[i].x;
            int y = parent.y + directions[i].y;

            if (TileIsValid(dungeon.Tiles[x, y]) && !_allBFSNodes[x, y].isBeingChecked && !_allBFSNodes[x, y].isAlreadyChecked)
                validNeighbors.Add(dungeon.Tiles[x, y]);
        }

        return validNeighbors;
    }

    private static bool TileIsValid(HopTile tile) => IsWithinBounds(tile.X, tile.Y) && tile != null && tile.IsWalkable; // * Change these conditions to detemine if tiles can be pathed or not

    private static bool IsWithinBounds(int x, int y) => x >= 0 && y >= 0 && x < width && y < height; // ! Change 8 to width and height
}
}