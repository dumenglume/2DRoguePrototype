using System;
using UnityEngine;

namespace GraphTest
{
public class TilemapFog : TilemapSpawner
{
    Vector3 playerPosition;
    [SerializeField] Grid grid;

    [SerializeField] Vector2Int fogBoundaryWidth = new Vector2Int(-10, 10);
    [SerializeField] Vector2Int fogBoundaryHeight = new Vector2Int(-10, 10);
    [SerializeField] Vector2Int fogRevealBoundaryWidth = new Vector2Int(-1, 2);
    [SerializeField] Vector2Int fogRevealBoundaryHeight = new Vector2Int(-1, 2);
    [SerializeField] bool onlyRevealCardinalNeighbors = false;

    public static Action fogTileRevealed;

    protected override void OnEnable() 
    {
        base.OnEnable();
        TilemapOverworld.allTilesSpawned += InitiateTileReveal;
        PlayerMovement.roomBeingEntered += InitiateTileReveal;
    }

    protected override void OnDisable() 
    {
        base.OnDisable();
        TilemapOverworld.allTilesSpawned -= InitiateTileReveal;
        PlayerMovement.roomBeingEntered -= InitiateTileReveal;
    }

    /// <summary>
    /// Fills tilemap with fog
    /// </summary>
    protected override void InitiateSpawning()
    {
        dungeon = DungeonGeneratorV2.Instance.Dungeon;

        if (dungeon == null) { return; }

        for (int x = fogBoundaryWidth.x; x < fogBoundaryWidth.y; x++)
        {
            for (int y = fogBoundaryHeight.y; y < fogBoundaryHeight.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }

    void InitiateTileReveal() // TODO Find less expensive method for this entire thing
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        int dungeonRoomDistance = DungeonGeneratorV2.Instance.RoomDistance;

        RevealTilesAtLocation(playerPosition);
    }

    void InitiateTileReveal(Room _room) // TODO Find less expensive method for this entire thing
    {
        Vector3Int roomPosition = (Vector3Int) _room.roomPosition;
        int dungeonRoomDistance = DungeonGeneratorV2.Instance.RoomDistance;

        RevealTilesAtLocation(roomPosition);
    }

    /// </summary>
    /// Reveals tiles around the player
    /// </summary>
    void RevealTilesAtLocation(Vector3 _location)
    {
        Vector3Int gridPosition = grid.WorldToCell(_location);

        for (int x = gridPosition.x + fogRevealBoundaryWidth.x; x < gridPosition.x + fogRevealBoundaryWidth.y; x++)
        {
            for (int y = gridPosition.y + fogRevealBoundaryHeight.x; y < gridPosition.y + fogRevealBoundaryHeight.y; y++)
            {
                Vector3Int tileToReveal = new Vector3Int(x, y, 0);

                if (tilemap.GetTile(tileToReveal) != null)
                {
                    tilemap.SetTile(tileToReveal, null);
                    BroadcastFogTileRevealed(tileToReveal);
                }
            }
        }
    }

    protected override void RemoveTiles()
    {
        tilemap.ClearAllTiles();
    }

    void BroadcastFogTileRevealed(Vector3Int _revealedFogPosition) // TODO Change this to BroadcastFogReveal or move this to a "stress" manager object
    {
        fogTileRevealed?.Invoke();
    }
}
}