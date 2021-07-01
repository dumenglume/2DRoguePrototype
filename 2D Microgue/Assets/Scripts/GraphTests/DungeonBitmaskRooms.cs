public static class DungeonBitmaskRooms
{
    /// <summary>
    /// Bitmask is based on clockwise N(1), E(2), S(4), W(8) assignment.
    /// </summary>
    public static void BitmaskRooms(Dungeon _dungeon)
    {
        for (int i = 0; i < _dungeon.Rooms.Count; i++)
        {
            Room thisRoom = _dungeon.Rooms[i];

            if (thisRoom.hasExitNorth) { thisRoom.bitmaskValue += 1; }
            if (thisRoom.hasExitEast)  { thisRoom.bitmaskValue += 2; }
            if (thisRoom.hasExitSouth) { thisRoom.bitmaskValue += 4; }
            if (thisRoom.hasExitWest)  { thisRoom.bitmaskValue += 8; }

            switch(thisRoom.bitmaskValue)
            {
                case 0:
                    thisRoom.roomOrientation = Room.RoomOrientation.Null;
                    break;
                case 1:
                    thisRoom.roomOrientation = Room.RoomOrientation.N;
                    thisRoom.roomType = Room.RoomType.deadEnd;
                    thisRoom.isDeadEnd = true;
                    break;
                case 2:
                    thisRoom.roomOrientation = Room.RoomOrientation.E;
                    thisRoom.roomType = Room.RoomType.deadEnd;
                    thisRoom.isDeadEnd = true;
                    break;
                case 3:
                    thisRoom.roomOrientation = Room.RoomOrientation.NE;
                    break;
                case 4:
                    thisRoom.roomOrientation = Room.RoomOrientation.S;
                    thisRoom.roomType = Room.RoomType.deadEnd;
                    thisRoom.isDeadEnd = true;
                    break;
                case 5:
                    thisRoom.roomOrientation = Room.RoomOrientation.NS;
                    break;
                case 6:
                    thisRoom.roomOrientation = Room.RoomOrientation.ES;
                    break;
                case 7:
                    thisRoom.roomOrientation = Room.RoomOrientation.NES;
                    break;
                case 8:
                    thisRoom.roomOrientation = Room.RoomOrientation.W;
                    thisRoom.roomType = Room.RoomType.deadEnd;
                    thisRoom.isDeadEnd = true;
                    break;
                case 9:
                    thisRoom.roomOrientation = Room.RoomOrientation.NW;
                    break;
                case 10:
                    thisRoom.roomOrientation = Room.RoomOrientation.EW;
                    break;
                case 11:
                    thisRoom.roomOrientation = Room.RoomOrientation.NEW;
                    break;
                case 12:
                    thisRoom.roomOrientation = Room.RoomOrientation.SW;
                    break;
                case 13:
                    thisRoom.roomOrientation = Room.RoomOrientation.NSW;
                    break;
                case 14:
                    thisRoom.roomOrientation = Room.RoomOrientation.ESW;
                    break;
                case 15:
                    thisRoom.roomOrientation = Room.RoomOrientation.NESW;
                    break;
            }
        }
    }
}
