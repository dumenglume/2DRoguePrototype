using UnityEngine;
using UnityEngine.Tilemaps;
using GraphTest; // ! Need to change this to where CustomTile doesn't try to pull from GraphTest custom tile

# if UNITY_EDITOR
using UnityEditor;
# endif

public class HBCustomTile : Tile
{
    [SerializeField] GameObject[] objectsToSpawn = new GameObject[16];
    GameObject objectToSpawn;

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
        tileData.sprite       = sprite;
        tileData.transform    = transform;
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
