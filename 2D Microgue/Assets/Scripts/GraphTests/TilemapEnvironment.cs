using System.Collections.Generic;
using UnityEngine;

public class TilemapEnvironment : TilemapSpawner
{
    [SerializeField] GameObject backgroundTilePrefab;
    [SerializeField] List<Sprite> spriteList = new List<Sprite>(); // TODO Use array or different method entirely?

    List<GameObject> backgroundTiles = new List<GameObject>();

    protected override void InitiateSpawning()
    {
        dungeon = DungeonGenerator.Instance.Dungeon;

        if (dungeon != null)
        {
            for (int i = 0; i < dungeon.Rooms.Count; i++)
            {
                Room thisRoom = dungeon.Rooms[i];
                Vector3Int tilePosition = (Vector3Int) thisRoom.roomPosition; // TODO Switch to grid position

                GameObject backgroundTileInstance = Instantiate(backgroundTilePrefab, tilePosition, Quaternion.identity) as GameObject;
                SpriteRenderer backgroundSpriteRenderer = backgroundTileInstance.GetComponent<SpriteRenderer>();

                int roomOrientation = (int) thisRoom.roomOrientation;
                backgroundSpriteRenderer.sprite = spriteList[roomOrientation];

                backgroundTiles.Add(backgroundTileInstance);
            }
        }
    }

    protected override void RemoveTiles()
    {
        for (int i = backgroundTiles.Count - 1; i >= 0 ; i--)
        {
            GameObject backgroundTileToRemove = backgroundTiles[i];
            Destroy(backgroundTileToRemove);
            backgroundTiles.RemoveAt(i);
        }
    }
}
