using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDLAGenerator : MonoBehaviour
{
    [SerializeField] int roomWidth = 16;
    [SerializeField] int roomHeight = 10;

    [SerializeField] int prefabLimit = 10;
    [SerializeField] float growthSpeed = 0.1f;
    [SerializeField] float chanceToTurn = 0.5f;
    [SerializeField] GameObject inputPrefab;

    List<GameObject> allPrefabs = null;
    List<Vector2> allPrefabPositions = null;
    List<Vector2> cardinalDirections = new List<Vector2>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    Vector2 previousDirection = Vector2.right;
    GameObject indicatorPrefab = null;

    int placementAttempts = 100;

    void Start()
    {
        ClearScene();
        SetupScene();
        RunDLACoroutine();
    }

    void ClearScene()
    {
        if (allPrefabs == null)    { return; }
        if (allPrefabs.Count == 0) { return; }

        foreach (GameObject prefab in allPrefabs)
        {
            Destroy(prefab); // TODO Switch to pooling system
        }

        allPrefabs.Clear();
        allPrefabPositions.Clear();
    }

    void SetupScene()
    {
        prefabLimit = Mathf.Clamp(prefabLimit, 0, roomWidth * roomHeight); // Prevents out of range placement issues
        allPrefabs = new List<GameObject>();
        allPrefabPositions = new List<Vector2>();

        Vector3 originPoint = new Vector3(Mathf.RoundToInt(roomWidth / 2), Mathf.RoundToInt(roomHeight / 2), 0f);

        GameObject newPrefab = Instantiate(inputPrefab, originPoint, Quaternion.identity); // TODO Move to function as it's duplicated later on
        allPrefabs.Add(newPrefab);

        Vector3 newPrefabPosition = newPrefab.transform.position;
        allPrefabPositions.Add(newPrefabPosition);

        
        if (indicatorPrefab == null)
        {
            indicatorPrefab = Instantiate(inputPrefab, newPrefabPosition, Quaternion.identity);
            indicatorPrefab.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            indicatorPrefab.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
    }

    void RunDLACoroutine()
    {
        StartCoroutine(nameof(RunDLA));
    }

    IEnumerator RunDLA()
    {
        while(allPrefabs.Count < prefabLimit)
        {
            yield return new WaitForSeconds(growthSpeed);
            AddPrefab();
        }
    }

    void AddPrefab()
    {
        List<Vector2> shuffledDirections = ShuffleList(cardinalDirections);

        int randomPrefabInstance = Random.Range(0, allPrefabs.Count - 1);

        GameObject thisPrefab = allPrefabs[randomPrefabInstance];

        for (int i = 0; i < shuffledDirections.Count; i++)
        {
            Vector2 desiredDirection = Random.value < chanceToTurn ? shuffledDirections[i] : previousDirection;
            Vector2 desiredPosition = (Vector2) indicatorPrefab.transform.position + desiredDirection;

            if (!WithinBounds(desiredPosition)) { return; }

            if (!allPrefabPositions.Contains(desiredPosition))
            {
                GameObject newPrefab = Instantiate(inputPrefab, desiredPosition, Quaternion.identity); 
                allPrefabs.Add(newPrefab);
                allPrefabPositions.Add(newPrefab.transform.position);

                indicatorPrefab.transform.position = newPrefab.transform.position;

                previousDirection = shuffledDirections[i];
                return;
            }

            else
            {
                GameObject nextPrefab = allPrefabs[randomPrefabInstance];
                indicatorPrefab.transform.position = nextPrefab.transform.position;
            }
        }
    }

    bool WithinBounds(Vector2 _position)
    {
        return _position.x > 0 || _position.x < roomWidth || _position.y > 0 || _position.y < roomHeight;
    }

    List<Vector2> ShuffleList(List<Vector2> _list) // TODO Switch to generic list and move to static helper class?
    {
        List<Vector2> shuffledList = new List<Vector2>(_list);

        for (int i = 0; i < shuffledList.Count; i++) 
        {
            Vector2 temp = shuffledList[i];
            int randomIndex = Random.Range(i, _list.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }
}
