using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGeneration : MonoBehaviour
{
    public int openingDirection;
    // 1 Bottom Door
    // 2 Top Door
    // 3 Left Door
    // 4 Right Door

    Rooms rooms;
    int rand;

    void Start()
    {
        Invoke(nameof(Generate), 0.1f);
    }

    void Generate()
    {
        if (openingDirection == 1) // Spawn room with bottom door
        {
            rand = Random.Range(0, rooms.bottomRooms.Length);
            GameObject randomRoom = rooms.bottomRooms[rand];
            Quaternion randomRoomRotation = randomRoom.transform.rotation;
            Instantiate(randomRoom, transform.position, randomRoomRotation);
        }

        else if (openingDirection == 2) // Spawn room with top door
        {
            rand = Random.Range(0, rooms.topRooms.Length);
            GameObject randomRoom = rooms.topRooms[rand];
            Quaternion randomRoomRotation = randomRoom.transform.rotation;
            Instantiate(randomRoom, transform.position, randomRoomRotation);
        }

        else if (openingDirection == 3) // Spawn room with left door
        {
            rand = Random.Range(0, rooms.leftRooms.Length);
            GameObject randomRoom = rooms.leftRooms[rand];
            Quaternion randomRoomRotation = randomRoom.transform.rotation;
            Instantiate(randomRoom, transform.position, randomRoomRotation);
        }

        else if (openingDirection == 4) // Spawn room with right door
        {
            rand = Random.Range(0, rooms.rightRooms.Length);
            GameObject randomRoom = rooms.rightRooms[rand];
            Quaternion randomRoomRotation = randomRoom.transform.rotation;
            Instantiate(randomRoom, transform.position, randomRoomRotation);
        }
    }
}

public class Rooms : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
}
