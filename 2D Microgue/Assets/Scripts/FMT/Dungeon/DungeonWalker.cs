using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class DungeonWalker
{
    #region Variables ==================================================================================================================
    public DungeonGenerator dungeonGenerator;
    List<Vector3Int> directions = new List<Vector3Int>() { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };
    Vector3Int currentDirection = Vector3Int.right;
    Vector3Int currentPosition;

    int currentCorridorLength = 0; // Used for preventing too long of hallways
    [SerializeField] int maxCorridorLength = 4;
    [SerializeField] [Range(0.01f, 0.99f)] float turnChance = 0.5f;

    public DungeonWalker(int x, int y)
    {
        currentPosition = new Vector3Int(x, y, 0);
    }

    #endregion Variables ================================================================================================================

    public IEnumerator Co_CarvePathway(TileBase floorTileSprite)
    {
        while(dungeonGenerator.DictionaryWalkableTiles.Count < dungeonGenerator.WalkablePositionsLimit)
        {
            Vector3Int nextTilePosition = currentPosition + currentDirection;
            bool targetOutsideBounds    = !WithinBounds(nextTilePosition, 0, dungeonGenerator.WorldWidth - 1, 0, dungeonGenerator.WorldHeight - 1);

            if   ( DirectionShouldChange() || targetOutsideBounds) { ChangeDirection(); }
            else { CarveTile(nextTilePosition, floorTileSprite); }

            if (DungeonManager.Instance.GenerationSpeed > 0.0f) { yield return new WaitForSeconds(DungeonManager.Instance.GenerationSpeed); }
        }
    }

    void CarveTile(Vector3Int thisTilePosition, TileBase floorTileBase)
    {
        currentPosition = thisTilePosition;
        currentCorridorLength += 1;

        _Tile existingTile = dungeonGenerator.GetTile(currentPosition.x, currentPosition.y);
        if (existingTile != null && existingTile is IAmWalkable) { return; }

        _FloorTile floorTile = new _FloorTile(floorTileBase);

        dungeonGenerator.PlaceTile(currentPosition.x, currentPosition.y, floorTile);
    }

    void CarveTileOld(Vector3Int thisTilePosition, _Tile tilePrefab)
    {
        currentPosition = thisTilePosition;
        currentCorridorLength += 1;

        _Tile existingTile = dungeonGenerator.GetTile(currentPosition.x, currentPosition.y);
        if (existingTile != null && existingTile is IAmWalkable) { return; }

        dungeonGenerator.PlaceTile(currentPosition.x, currentPosition.y, tilePrefab);
    }

    bool DirectionShouldChange()
    {
        bool maxCorridorLengthReached = currentCorridorLength >= maxCorridorLength;
        bool changeDirectionChance    = Random.value <= turnChance;
        bool directionShouldChange    = maxCorridorLengthReached && changeDirectionChance ? true : false;

        return directionShouldChange;
    }

    void ChangeDirection()
    {
        currentCorridorLength = 0;

        List<Vector3Int> availableDirections = new List<Vector3Int>(directions);
        availableDirections.Remove(currentDirection);
        availableDirections = ShuffleList(availableDirections);
        currentDirection    = availableDirections[0];
        availableDirections.RemoveAt(0);

        while (!WithinBounds(currentPosition + currentDirection, 0, dungeonGenerator.WorldWidth - 1, 0, dungeonGenerator.WorldHeight - 1))
        {
            currentDirection = availableDirections[0];
            availableDirections.RemoveAt(0);
        }
    }

    #region Helper Methods ==================================================================================================================

    public bool WithinBounds(Vector3Int _position, int widthMin, int widthMax, int heightMin, int heightMax)
    {
        return _position.x > widthMin && _position.x < widthMax && _position.y > heightMin && _position.y < heightMax;
    }

        public List<Vector2Int> ShuffleList(List<Vector2Int> _list) // TODO Switch to generic list and move to static helper class?
    {
        List<Vector2Int> shuffledList = new List<Vector2Int>(_list);

        for (int i = 0; i < shuffledList.Count; i++) 
        {
            Vector2Int temp = shuffledList[i];
            int randomIndex = Random.Range(i, _list.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }

    public List<Vector3Int> ShuffleList(List<Vector3Int> _list) // TODO Switch to generic list and move to static helper class?
    {
        List<Vector3Int> shuffledList = new List<Vector3Int>(_list);

        for (int i = 0; i < shuffledList.Count; i++) 
        {
            Vector3Int temp = shuffledList[i];
            int randomIndex = Random.Range(i, _list.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }

    #endregion Helper Methods ===============================================================================================================
}
}