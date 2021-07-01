using UnityEngine;
using UnityEngine.Tilemaps;

    public static class HelperTilemaps
    {
        public static bool TileIsVacant(Tilemap _groundTilemap, Tilemap _collisionTilemap, Vector3 _currentPosition, Vector2 _direction, out TileBase _tile)
        {
            Vector3Int gridPosition = _groundTilemap.WorldToCell(_currentPosition + (Vector3) _direction);

            bool groundExists   = _groundTilemap.HasTile(gridPosition);
            bool obstacleExists = _collisionTilemap.HasTile(gridPosition);
            bool entityExists   = CheckForEntityAtPosition(gridPosition);
            entityExists = false;

            if (!groundExists || obstacleExists || entityExists)
            {
                _tile = _collisionTilemap.GetTile(gridPosition);
                return false;
            }

            _tile = null;
            return true;
        }

        static bool CheckForEntityAtPosition(Vector3Int _gridPosition)
        {
            var allEntities = GameManager.Instance.allEntitiesList;

            foreach (var entity in allEntities)
            {
                bool gridPositionMatchesEntityPosition = _gridPosition == entity.transform.position;

                if (gridPositionMatchesEntityPosition) { return true; }
            }

            return false;
        }
    }