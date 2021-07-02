using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class _Tile : MonoBehaviour
{

    #region Variables =============================================================================================================================================
    public Vector3Int coordinate {get; protected set;}
    public Tilemap tilemap  {get; protected set;}
    // public TileBase tileBase {get; protected set;}
    public GameObject gameObjectTarget {get; protected set;}
    public TileBase tileBase;

    [field: SerializeField]
    public string tileType {get; private set;}
    
    public TileState tileState;        
    #endregion Variables =============================================================================================================================================

    protected void Awake() {
        SetState(true, true);
    }

    public void SetProperties(Vector3Int coordinate, Tilemap tilemap)
    {
        this.coordinate = coordinate;
        this.tilemap  = tilemap;
    }

    protected virtual void SetState( bool interactive, bool walkable ) {
        tileState.interactive = interactive;
        tileState.walkable = walkable;
    }

    public void Initialize()
    {
        tilemap.SetTile(coordinate, tileBase);
    }
}

[System.Serializable]
public struct TileState
{
    public bool visited; 
    public bool walkable;
    public bool interactive;
}
