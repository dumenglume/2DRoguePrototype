using UnityEngine;

namespace GraphTest
{
public class DungeonGizmos : MonoBehaviour
{
    Dungeon dungeon;

    [SerializeField] GameObject roomDebugPrefab;

    void Start()
    {

        dungeon = DungeonGeneratorV2.Instance.Dungeon;

        Debug.Log(dungeon);

        if (dungeon != null)
        {
            for (int i = 0; i < dungeon.Rooms.Count; i++)
            {
                Room thisRoom = dungeon.Rooms[i];
                Vector3 thisRoomPosition = new Vector3(thisRoom.roomPosition.x, thisRoom.roomPosition.y, 0f);
                GameObject roomDebugInstance = Instantiate(roomDebugPrefab, thisRoomPosition, Quaternion.identity) as GameObject;
                TextMesh floatingText = roomDebugInstance.GetComponentInChildren<TextMesh>();
                floatingText.text = thisRoom.roomPosition + "\n" + thisRoom.roomType + "\n" + thisRoom.roomOrientation + " / " + thisRoom.roomID;
            }
        }
    }

    void OnDrawGizmos()
        {
            if (dungeon != null)
            {
                foreach (Room room in dungeon.Rooms)
                {
                    switch(room.roomType)
                    {
                        case Room.RoomType.spawn:
                            { Gizmos.color = Color.green; }
                            break;

                        case Room.RoomType.exit:
                            { Gizmos.color = Color.magenta; }
                            break;

                        case Room.RoomType.normal:
                            { Gizmos.color = Color.grey; }
                            break;

                        case Room.RoomType.village:
                            { Gizmos.color = Color.blue; }
                            break;

                        case Room.RoomType.combat:
                            { Gizmos.color = Color.red; }
                            break;

                        case Room.RoomType.deadEnd:
                            { Gizmos.color = Color.cyan; }
                            break;

                        default:
                            { Gizmos.color = Color.white; }
                            break;
                    }
                    
                    Gizmos.DrawCube((Vector3Int) room.roomPosition, new Vector3(0.5f, 0.5f, 0.5f));
                }

                foreach (Corridor corridor in dungeon.Corridors)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine((Vector3Int) corridor.fromRoom.roomPosition, (Vector3Int) corridor.toRoom.roomPosition);
                }
            }
        }
}
}