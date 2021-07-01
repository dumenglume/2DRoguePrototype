using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HB 
{
    public class HBFogSpawner : MonoBehaviour
    {
        public static Action fogTileRevealed;

        [SerializeField] Grid grid;
        [SerializeField] Tilemap tilemapFog;
        [SerializeField] TileBase tileFog;

        [SerializeField] Vector2Int fogRevealBoundaryWidth = new Vector2Int(-1, 2);
        [SerializeField] Vector2Int fogRevealBoundaryHeight = new Vector2Int(-1, 2);

        HBDungeon dungeonGenerator;

        void OnEnable() 
        {
            HBEntitySpawner.allEntitiesSpawned   += InitiateSpawning;
            HBEntitySpawner.revealPlayerTile     += RevealTilesAtLocation;
            HBPlayerMovement.OnMovementInitiated += RevealTilesAtLocation;
        }

        void OnDisable() 
        {
            HBEntitySpawner.allEntitiesSpawned   -= InitiateSpawning;
            HBEntitySpawner.revealPlayerTile     -= RevealTilesAtLocation;
            HBPlayerMovement.OnMovementInitiated -= RevealTilesAtLocation;
        }

        void InitiateSpawning()
        {
            for (int x = 0; x < HBDungeon.Instance.WorldWidth; x++)
            {
                for (int y = 0; y < HBDungeon.Instance.WorldHeight; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemapFog.SetTile(tilePosition, tileFog);
                }
            }
        }

        void RevealTilesAtLocation(Vector3Int _location)
        {
            Vector3Int gridPosition = grid.WorldToCell(_location);

            for (int x = gridPosition.x + fogRevealBoundaryWidth.x; x < gridPosition.x + fogRevealBoundaryWidth.y; x++)
            {
                for (int y = gridPosition.y + fogRevealBoundaryHeight.x; y < gridPosition.y + fogRevealBoundaryHeight.y; y++)
                {
                    Vector3Int tileToReveal = new Vector3Int(x, y, 0);

                    if (tilemapFog.GetTile(tileToReveal) != null)
                    {
                        tilemapFog.SetTile(tileToReveal, null);
                        BroadcastFogTileRevealed(tileToReveal);
                    }
                }
            }
        }

        void RemoveTiles()
        {
            tilemapFog.ClearAllTiles();
        }

        void BroadcastFogTileRevealed(Vector3Int _revealedFogPosition) // TODO Change this to BroadcastFogReveal or move this to a "stress" manager object
        {
            fogTileRevealed?.Invoke();
        }
    }
}