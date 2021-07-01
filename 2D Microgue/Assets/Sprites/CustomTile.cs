using UnityEngine;
using UnityEngine.Tilemaps;

# if UNITY_EDITOR
using UnityEditor;
# endif

public class CustomTile : Tile
{
    [SerializeField] Sprite[] sprites = new Sprite[16];

    [Tooltip("spawn, exit, key, normal, village, combat, treasure, shop, shrine, npc, corridor, deadEnd")]
    [SerializeField] GameObject[] objectsToSpawn = new GameObject[16];
    GameObject objectToSpawn;

    int spriteOrientationIndex = 0;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        tileData.color        = color;
        tileData.flags        = flags;
        tileData.gameObject   = objectToSpawn;
        tileData.colliderType = ColliderType.None;
        tileData.sprite       = sprites[spriteOrientationIndex];
        tileData.transform    = transform;
    }

    public void SetTileType(int _roomTypeIndex)
    {
        objectToSpawn = objectsToSpawn[_roomTypeIndex];
    }

    public void SetSpriteOrientation(Room.RoomOrientation _roomOrientation)
    {
        spriteOrientationIndex = (int) _roomOrientation;
    }

    # if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Custom Tile")]
    public static void CreateCustomTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Custom Tile", "New Custom Tile", "Asset", "Save Custom Tile", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTile>(), path);
    }
    # endif
}
