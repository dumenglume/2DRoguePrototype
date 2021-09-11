using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue
{
public class DungeonData
{
    public int[,] currentDungeonData;

    public DungeonData()
    {
        InitializeDungeonData();
        Debug.Log(currentDungeonData.Length);
    }

    public void InitializeDungeonData()
    {
        int[,] currentDungeonData = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 0, 0, 0, 0, 1, 1 },
            { 1, 1, 0, 0, 0, 0, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
        };
    }

    public int[,] GetDungeonData()
    {
        return currentDungeonData;
    }
}
}