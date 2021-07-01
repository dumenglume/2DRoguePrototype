using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB {
public static class HBHelper
{
    public static List<Vector2Int> ShuffleList(List<Vector2Int> _list) // TODO Switch to generic list and move to static helper class?
    {
        List<Vector2Int> shuffledList = new List<Vector2Int>(_list);

        for (int i = 0; i < shuffledList.Count; i++) 
        {
            Vector2Int temp = shuffledList[i];
            int randomIndex = Random.Range(i, _list.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }

    public static List<Vector3Int> ShuffleList(List<Vector3Int> _list) // TODO Switch to generic list and move to static helper class?
    {
        List<Vector3Int> shuffledList = new List<Vector3Int>(_list);

        for (int i = 0; i < shuffledList.Count; i++) 
        {
            Vector3Int temp = shuffledList[i];
            int randomIndex = Random.Range(i, _list.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }

    public static bool WithinBounds(Vector3Int _position, int widthMin, int widthMax, int heightMin, int heightMax)
    {
        return _position.x > widthMin && _position.x < widthMax && _position.y > heightMin && _position.y < heightMax;
    }
}
}