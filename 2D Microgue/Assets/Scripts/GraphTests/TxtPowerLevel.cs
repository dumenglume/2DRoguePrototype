using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxtPowerLevel : MonoBehaviour
{
    [SerializeField] TextMesh textMesh;
    void Start()
    {
        textMesh.text = "Lvl. " + Mathf.RoundToInt(Random.Range(1, 500));
    }
}
