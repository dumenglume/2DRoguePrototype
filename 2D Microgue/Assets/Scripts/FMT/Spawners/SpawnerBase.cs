using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public abstract class SpawnerBase : MonoBehaviour
{
    [SerializeField] protected List<GameObject> objectPrefabsList;

    [SerializeField] protected int maxObjects = 10;

    protected List<_Tile> spawnablePositions;

    protected Dictionary<_Tile, GameObject> spawnedObjectsDictionary;

    protected float generationSpeed;

    public IEnumerator Co_BeginSpawnProcess()
    {
        generationSpeed = DungeonManager.Instance.GenerationSpeed;

        InitializeObjectDictionary();
        GetSpawnablePositions();
        yield return Co_SpawnObjects();

        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
    }

    public virtual void ClearAllObjects() // TODO May need to change this into reverse for loop
    {
        if (spawnedObjectsDictionary == null) { return; }

        if (spawnedObjectsDictionary.Count == 0) { return; }

        foreach (GameObject objectToClear in spawnedObjectsDictionary.Values) { Destroy(objectToClear); }

        spawnedObjectsDictionary.Clear();
    }

    protected void InitializeObjectDictionary() => spawnedObjectsDictionary = new Dictionary<_Tile, GameObject>();

    protected virtual void GetSpawnablePositions() => spawnablePositions = new List<_Tile>(DungeonManager.Instance.ListWalkableTiles);

    protected virtual IEnumerator Co_SpawnObjects() // Spawns objects at random spawnable positions
    {
        int objectsSpawned = 0;

        while(objectsSpawned < maxObjects)
        {
            int spawnInterval = Random.Range(0, spawnablePositions.Count);

            for (int i = 0; i < spawnablePositions.Count; i++)
            {
                if (i == spawnInterval)
                {
                    _Tile tileToSpawnOn   = spawnablePositions[i];
                    int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

                    GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
                    tileToSpawnOn.boundGameObject = thisObject;

                    AddObjectToDictionary(tileToSpawnOn, thisObject);
                    RemoveSpawnablePosition(i);

                    objectsSpawned ++;

                    if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
                }
            }
        }
    }

    protected void RemoveSpawnablePosition(int index)
    {
        spawnablePositions.RemoveAt(index);
        DungeonManager.Instance.RemoveFromWalkableList(index);
    }

    protected void AddObjectToDictionary(_Tile tile, GameObject @object) => spawnedObjectsDictionary.Add(tile, @object);
}
}