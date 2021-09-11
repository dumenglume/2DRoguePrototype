using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HopRogue
{
public class DungeonGenerator
{
    private Dungeon _dungeon;
    private int[,] _dungeonBlueprint;

    Tilemap _tilemap;
    TileBase _floorTile;
    TileBase _wallTile;
    TileBase _exitTile;
    TileBase _emptyTile;

    public DungeonGenerator(Tilemap tilemap, TileBase floorTile, TileBase wallTile, TileBase exitTile, TileBase emptyTile)
    {
        _tilemap   = tilemap;
        _floorTile = floorTile;
        _wallTile  = wallTile;
        _exitTile  = exitTile;
        _emptyTile = emptyTile;

        GenerateDungeon();
        DebugDungeon(_dungeonBlueprint);
    }

    private void GenerateDungeon()
    {
        _dungeonBlueprint = GetDungeonBlueprint(); // TODO May need to move this into DungeonGenerator constructor to make choosing different layouts easier

        int dungeonWidth  = GetDungeonLength(0);
        int dungeonHeight = GetDungeonLength(1);

        _dungeon = new Dungeon(dungeonWidth, dungeonHeight);

        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                int dungeonValue = _dungeonBlueprint[x, y];

                HopTile tileBeingSpawned = _dungeon.Tiles[x, y];

                switch(dungeonValue)
                {
                    case 0:
                    {
                        _dungeon.Tiles[x, y] = new HopTileFloor(x, y, _tilemap, _floorTile);
                        break;
                    }

                    case 1:
                    {
                        _dungeon.Tiles[x, y] = new HopTileWall(x, y, _tilemap, _wallTile);
                        break;
                    }

                    case 2:
                    {
                        int chanceForWall = Random.Range(0, 2);

                        if (chanceForWall == 0)
                            _dungeon.Tiles[x, y] = new HopTileFloor(x, y, _tilemap, _floorTile);

                        else
                            _dungeon.Tiles[x, y] = new HopTileWall(x, y, _tilemap, _wallTile);
                        break;
                    }

                    case 3: 
                    {
                        _dungeon.Tiles[x, y] = new HopTileExit(x, y, _tilemap, _exitTile);
                        break;
                    }

                    case 9: 
                    {
                        _dungeon.Tiles[x, y] = new HopTileEmpty(x, y, _tilemap, _emptyTile);
                        break;
                    }

                    default:
                        break;
                }
            }
        }
    }

    public Dungeon GetDungeon() => _dungeon;

    public int GetDungeonLength(int dimension) => _dungeonBlueprint.GetLength(dimension);

    public void DebugDungeon(int[,] dungeonData)
    {
        int width  = GetDungeonLength(0);
        int height = GetDungeonLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.Log(string.Format($"[{x}, {y}]: {dungeonData[x, y]}"));
            }
        }
    }

    // TODO Move to own class eventually
    private int[,] GetDungeonBlueprint()
    {
        int[][,] blueprintArray = new int[1][,]
        {
            new int[,]
            {
                { 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 1, 1, 1, 3, 1, 1, 1, 9 },
                { 9, 1, 0, 0, 0, 0, 0, 1, 9 },
                { 9, 1, 0, 0, 0, 0, 0, 1, 9 },
                { 9, 1, 1, 0, 1, 0, 1, 3, 9 },
                { 9, 1, 0, 0, 0, 0, 0, 1, 9 },
                { 9, 1, 0, 0, 1, 0, 0, 1, 9 },
                { 9, 1, 1, 1, 3, 1, 1, 1, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9 }
            }
        };

        int randomBlueprintIndex = Random.Range(0, blueprintArray.Length - 1);
        return blueprintArray[randomBlueprintIndex];
    }
}
}