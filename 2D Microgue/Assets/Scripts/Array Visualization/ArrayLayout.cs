using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct rowData
    {
        public int[] row; // Change bool to other types if needed
    }

    public rowData[] rows = new rowData[7]; // Grid of 7x7
}
