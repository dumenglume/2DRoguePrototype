using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using HopRogue.Entities;
using Monster = HopRogue.Entities.Monster;
using Item    = HopRogue.Entities.Item;

namespace HopRogue
{
public class World : MonoBehaviour // TODO Rename to EntityManager or EntityFactory
{
    public static World Instance { get; private set; }
    private DungeonGenerator _dungeonGenerator;
    public Dungeon CurrentDungeon { get; private set; }

    [Header("Actor Settings")]
    public Player _playerPrefab;
    public List<Monster> _monsterPrefabs;
    [SerializeField] Vector2Int _monsterCount = new Vector2Int(2, 3);

    [Header("Item Settings")]
    [SerializeField] List<Item> _itemPrefabs;
    [SerializeField] Vector2Int _itemCount = new Vector2Int(0, 2);

    [Header("Container Settings")]
    [SerializeField] List<Container> _containerPrefabs;
    [SerializeField] Vector2Int _containerCount = new Vector2Int(0, 2);

    [Header("Shrine Settings")]
    [SerializeField] Shrine _shrinePrefab;

    [Header("Tile Settings")]
    public Tilemap Tilemap; // * Currently referenced by GameManager for detecting which tile mouse clicks
    [SerializeField] TileBase _floorTile;
    [SerializeField] TileBase _wallTile;
    [SerializeField] TileBase _exitTile;
    [SerializeField] TileBase _emptyTile;

    List<Entity> allEntityGameObjects = new List<Entity>();

    void Awake() 
    {
        if   (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }
    }

    void OnEnable() 
    {
        PlayerSkill.OnEntitySpawned += CreateEntity; // TODO Refactor to allow any entity to create other entities
    }

    void OnDisable() 
    {
        PlayerSkill.OnEntitySpawned -= CreateEntity;
    }

    public void CreateWorld()
    {
        ClearExistingDungeon();
        CreateDungeon();
        CreatePlayer();
        CreateMonsters();
        // CreateItems();
        CreateContainers();
        CreateShrine();
    }

    public void CreateNewWorld()
    {
        if (CurrentDungeon == null)
            return;

        if (CurrentDungeon.EntityDictionary.Count > 0)
            CurrentDungeon.RemoveAllEntitiesFromDungeon();

        CreateWorld();
    }

    void ClearExistingDungeon() => Tilemap.ClearAllTiles();

    void CreateDungeon()
    {
        _dungeonGenerator = new DungeonGenerator(Tilemap, _floorTile, _wallTile, _exitTile, _emptyTile);
        CurrentDungeon    = new Dungeon(_dungeonGenerator.GetDungeonLength(0), _dungeonGenerator.GetDungeonLength(1));
        CurrentDungeon    = _dungeonGenerator.GetDungeon();
    }

    void CreatePlayer() => CreateEntity<Player>(_playerPrefab);

    void CreateMonsters() => CreateEntityFromList(_monsterPrefabs, _monsterCount); 

    void CreateItems() => CreateEntityFromList(_itemPrefabs, _itemCount);

    void CreateContainers() => CreateEntityFromList(_containerPrefabs, _containerCount);

    void CreateShrine() => CreateEntity<Shrine>(_shrinePrefab);

    public void CreateEntity<T>(T prefab, int x = -999, int y = -999) where T : Entity
    {
        T entity = Instantiate(prefab, transform.position, Quaternion.identity);

        entity.OnEntityDied += DestroyEntity;

        AddToWorldEntitiesList(entity);

        HopTile tileToSpawnAt = (x == -999 || y == -999) ? CurrentDungeon.GetRandomWalkableTile() : CurrentDungeon.GetTile(x, y);
        entity.SetPosition(tileToSpawnAt.X, tileToSpawnAt.Y);
        CurrentDungeon.AddEntity(entity);
    }

    void CreateEntityFromList<T>(List<T> prefabList, Vector2Int entityCountRange) where T : Entity
    {
        int numberOfEntities = Random.Range(entityCountRange.x, entityCountRange.y);

        for (int i = 0; i <= numberOfEntities; i++)
        {
            T entityPrefab = GetRandomEntityFromList<T>(prefabList);
            CreateEntity<T>(entityPrefab);
        }
    }

    void DestroyEntity(Entity entity)
    {
        entity.OnEntityDied -= DestroyEntity;
        RemoveFromWorldEntitiesList(entity);
        Destroy(entity.gameObject);
    }

    void AddToWorldEntitiesList(Entity entity) => allEntityGameObjects.Add(entity);

    void RemoveFromWorldEntitiesList(Entity entity) => allEntityGameObjects.Remove(entity);

    T GetRandomEntityFromList<T>(List<T> entityList) where T : Entity => entityList[Random.Range(0, entityList.Count)];

    public void DestroyAllEntityGameObjects()
    {
        for (int i = allEntityGameObjects.Count - 1; i >= 0 ; i--)
            DestroyEntity(allEntityGameObjects[i]);
    }
}
}