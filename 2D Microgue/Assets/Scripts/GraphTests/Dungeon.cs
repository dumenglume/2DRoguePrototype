using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Graph based data structure containing a list of Rooms (nodes) and Corridors (edges).
/// </summary>
public class Dungeon
{
    public List<Room> Rooms { get; set; }
    public List<Corridor> Corridors { get; set; }

    /// <summary>
    /// Dungeon (graph) is a data structure comprised of Rooms (nodes) connected by Corridors (edges)
    /// </summary>
    public Dungeon()
    {
        Rooms = new List<Room>();
        Corridors = new List<Corridor>();
    }
}

public class Room
{
    public int roomID = -1;
    public Vector2Int roomPosition { get; set; }

    public enum RoomType { spawn, exit, key, normal, village, combat, treasure, shop, shrine, npc, corridor, deadEnd }
    public RoomType roomType = RoomType.normal;

    public bool hasExitNorth { get; set; } // 1
    public bool hasExitEast { get; set; } // 2
    public bool hasExitSouth { get; set; } // 4
    public bool hasExitWest { get; set; } // 8
    public bool isDeadEnd { get; set; }

    public int bitmaskValue = 0;

    public enum RoomOrientation { Null, N, E, S, W, NE, NS, NW, ES, EW, SW, NES, NEW, NSW, ESW, NESW }
    public RoomOrientation roomOrientation = RoomOrientation.Null;
}

public class Corridor
{
    public enum CorridorOrientation { horizontal, vertical }
    public CorridorOrientation corridorOrientation = CorridorOrientation.horizontal;

    public Room fromRoom { get; set; }
    public Room toRoom { get; set; }
}
