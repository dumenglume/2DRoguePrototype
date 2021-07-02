using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class _Tile : MonoBehaviour
{

    #region Variables =============================================================================================================================================
    [field: SerializeField, DisplayOnly] public Vector3Int position {get; protected set;}
    public Tilemap tilemap  {get; protected set;}
    public TileBase tileBase;
    [field: SerializeField] public string tileType {get; private set;}
    [SerializeField] private TileStatus initialTileStatus;     
    [DisplayOnly] public TileStatus currentTileStatus;   

    #endregion Variables =============================================================================================================================================

    void Awake() => currentTileStatus = initialTileStatus;
    public void SetProperties(Vector3Int coordinate, Tilemap tilemap)
    {
        this.position = coordinate;
        this.tilemap  = tilemap;
    }
 
    public void Initialize() =>  tilemap.SetTile(position, tileBase);
}

[System.Serializable]
public struct TileStatus
{
    public bool visited; 
    public bool walkable;
    public bool interactive;
}
