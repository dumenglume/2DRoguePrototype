using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HBTileBase : MonoBehaviour
{
    public static event Action<Vector3Int> ExitTileTriggered;
    public static event Action PickedUpGold;

    public Vector3 localPosition { get; set; }
    public Tilemap tilemap { get; set; }

    public enum HBTileType { empty, floor, wall, deadEnd, start, exit, enemy, item }
    public HBTileType tileType { get; set; }

    public new GameObject gameObject { get; set; }

    bool isExplored    = false; 
    bool isWalkable    = false;
    bool isInteractive = false;
    bool isEnemy       = false;

    public bool IsExplored    { get { return isExplored; }    set { isExplored = value; } }
    public bool IsWalkable    { get { return isWalkable; }    set { isWalkable = value; } }
    public bool IsInteractive { get { return isInteractive; } set { isInteractive = value; } }
    public bool IsEnemy       { get { return isEnemy; }       set { isEnemy = value; } }

    public void TriggerInteraction()
    {
        switch(tileType)
        {
            case HBTileType.item:
            {
                tileType = HBTileType.floor;
                tilemap.RefreshTile(Vector3Int.RoundToInt(localPosition));
                isInteractive = false;
                gameObject.SetActive(false);

                PickedUpGold?.Invoke();

                break;
            }

            case HBTileType.enemy:
            {
                tileType = HBTileType.floor;
                tilemap.RefreshTile(Vector3Int.RoundToInt(localPosition));
                isWalkable = true;
                isInteractive = false;
                gameObject.SetActive(false);

                break;
            }

            case HBTileType.exit:
            {
                ExitTileTriggered?.Invoke(Vector3Int.RoundToInt(localPosition));
                break;
            }
        }
    }
}
