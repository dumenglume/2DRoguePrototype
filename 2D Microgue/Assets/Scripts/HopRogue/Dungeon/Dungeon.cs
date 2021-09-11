using System;
using System.Collections.Generic;
using UnityEngine;
using HopRogue.Entities;
using Random  = UnityEngine.Random;

namespace HopRogue
{
[Serializable]
public class Dungeon
{
    public HopTile[,] Tiles { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Player Player { get; set; }
    public List<Monster> Monsters { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<Vector3Int, Entity> EntityDictionary;

    public Dungeon(int width, int height)
    {
        Width    = width;
        Height   = height;
        Tiles    = new HopTile[width, height];
        Monsters = new List<Monster>();
        Items    = new List<Item>();
        EntityDictionary = new Dictionary<Vector3Int, Entity>();
    }

    public bool TileIsValid(Vector3Int location)
    {
        if (!TileIsWithinBounds(location)) { return false; }

        return Tiles[location.x, location.y].IsWalkable && !Tiles[location.x, location.y].HasEntity;
    }

    public bool TileIsWithinBounds(Vector3Int location) => location.x > 0 && location.y > 0 && location.x < Width && location.y < Height;

    public void AddEntity(Entity entity)
    {
        Vector3Int entityWorldPosition = entity.WorldPosition;

        EntityDictionary.Add(entityWorldPosition, entity);
        Tiles[entityWorldPosition.x, entityWorldPosition.y].HasEntity  = true;

        entity.OnEntityDied += RemoveEntity;
        entity.OnEntityMoved     += OnEntityMoved;

        AddEntityToList(entity);
    }

    void AddEntityToListOld(Entity entity)
    {
        if (entity.GetType() == typeof(Player))
            Player = entity as Player;

        else if (entity.GetType() == typeof(Monster))
            Monsters.Add(entity as Monster);

        else if (entity.GetType() == typeof(Item))
            Items.Add(entity as Item);
    }

    void AddEntityToList(Entity entity)
    {
        switch (entity)
        {
            case Player player:
                Player = player;
                break;

            case Monster monster:
                Monsters.Add(monster);
                break;

            case Item item:
                Items.Add(item);
                break;

            default:
                break;
        }
    }

    public void RemoveEntity(Entity entity)
    {
        Vector3Int entityWorldPosition = entity.WorldPosition;

        EntityDictionary.Remove(entityWorldPosition);
        Tiles[entityWorldPosition.x, entityWorldPosition.y].HasEntity = false;

        entity.OnEntityDied -= RemoveEntity;
        entity.OnEntityMoved     -= OnEntityMoved;

        RemoveEntityFromList(entity);
    }

    void RemoveEntityFromList(Entity entity)
    {
        switch (entity)
        {
            case Player player:
                Player = null;
                break;

            case Monster monster:
                Monsters.Remove(monster);
                break;

            case Item item:
                Items.Remove(item);
                break;

            default:
                break;
        }
    }

    public void RemoveAllEntitiesFromDungeon()
    {
        if (EntityDictionary == null) return;

        List<Entity> entitiesToRemove = new List<Entity>(EntityDictionary.Values);

        foreach (Entity entity in entitiesToRemove)
            RemoveEntity(entity);

        Monsters.Clear();
        Items.Clear();
        EntityDictionary.Clear();
    }

    public Entity GetEntityAtPosition(Vector3Int location)
    {
        Entity entity;
        EntityDictionary.TryGetValue(location, out entity);

        return entity;
    }

    void OnEntityMoved(Entity entity, Vector3Int oldPosition, Vector3Int newPosition)
    {
        Debug.Log($"{entity.name} moved from {(Vector2Int) oldPosition} to {(Vector2Int) newPosition}");
        Entity actualEntity;

        if (!EntityDictionary.TryGetValue(oldPosition, out actualEntity))
        {
            Debug.Log($"Entity Position mismatch.");
            return;
        }

        Tiles[oldPosition.x, oldPosition.y].HasEntity = false;
        Tiles[newPosition.x, newPosition.y].HasEntity = true;

        EntityDictionary.Remove(oldPosition);
        EntityDictionary.Add(newPosition, entity);
    }

    public HopTile GetTile(int x, int y)
    {
        return Tiles[x, y];
    }

    public void TriggerTile(Actor actor, Vector3Int targetPosition)
    {
        HopTile tileToTrigger = GetTile(targetPosition.x, targetPosition.y);

        if (tileToTrigger.HasTrigger)
            tileToTrigger.TriggerTile(actor);
    }

    public List<Vector3Int> GetCardinalValidNeighbors(Vector3Int currentPosition)
    {
        List<Vector3Int> validNeighbors = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left }; // TODO Shuffle direction list for less predictable pathfinding

        for (int i = 0; i < directions.Length; i++)
        {
            int x = currentPosition.x + directions[i].x;
            int y = currentPosition.y + directions[i].y;

            if (TileIsValid(Tiles[x, y]))
                validNeighbors.Add(new Vector3Int(x, y, 0));
        }

        return validNeighbors;
    }

    public HopTile GetRandomWalkableTile()
    {
        int randomX = Random.Range(0, Tiles.GetLength(0));
        int randomY = Random.Range(0, Tiles.GetLength(1));

        while (!Tiles[randomX, randomY].IsWalkable || Tiles[randomX, randomY].HasEntity || !Tiles[randomX, randomY].IsSpawnable) // TODO Find better method for this
        {
            randomX = Random.Range(0, Tiles.GetLength(0));
            randomY = Random.Range(0, Tiles.GetLength(1));
        }

        return Tiles[randomX, randomY];
    }

    public bool TileIsValid(HopTile tile) // ! Consolidate this with TileIsValid(Vector3Int location) method above
    {
        if (tile == null)
            return false;

        if (!TileIsWithinBounds(tile))
            return false;

        if (!tile.IsWalkable)
            return false;

        if (TileIsOccupied(tile))
            return false;

        return true;
    }

    bool TileIsOccupied(HopTile tile)
    {
        foreach (Entity entity in GetAllEntitiesInDungeon())
        {
            if (EntityIsOccupyingTile(entity, tile))
                return true;
        }

        return false;
    }

    bool TileIsWithinBounds(HopTile tile) => tile.X >= 0 && tile.Y >= 0 && tile.X < Width && tile.Y < Height;
    
    Dictionary<Vector3Int, Entity>.ValueCollection GetAllEntitiesInDungeon() => EntityDictionary.Values;

    bool EntityIsOccupyingTile(Entity entity, HopTile tile) => entity.WorldPosition.x == tile.X && entity.WorldPosition.y == tile.Y;

    public void LogEntities()
    {
        Debug.Log($"Total entity count: {EntityDictionary.Count}");
        
        foreach (KeyValuePair<Vector3Int, Entity> kvp in EntityDictionary)
        {
            Debug.Log($"Key = {kvp.Key}, Value = {kvp.Value}");
        }
    }
}
}