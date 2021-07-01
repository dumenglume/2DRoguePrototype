using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLAGenerator : MonoBehaviour
{
    [SerializeField] int DLAgrowthCount = 0;
    [SerializeField] float growthSpeed = 0.1f;
    [SerializeField] GameObject inputPrefab;

    List<GameObject> allPrefabs = null;
    GameObject activePrefab = null;
    GameObject directionPrefab = null;

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
    }

    void SetupScene()
    {
        allPrefabs = new List<GameObject>();
        GameObject newPrefab = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
        allPrefabs.Add(newPrefab);

        if (activePrefab == null)
        {
            Vector3 newPrefabPosition = newPrefab.transform.position;
            activePrefab = Instantiate(inputPrefab, newPrefabPosition, Quaternion.identity);
            activePrefab.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            activePrefab.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (directionPrefab == null)
        {
            directionPrefab = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
            directionPrefab.transform.localScale = Vector3.one * 0.5f;
            directionPrefab.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    void AddDLAAgent()
    {
        if (activePrefab == null) { return; }

        int randomXValue = Random.Range(-1, 2);
        int randomYValue = Random.Range(-1, 2);

        Vector3 DLADirection = new Vector3(randomXValue, randomYValue, 0f) * 2;

        if (allPrefabs.Count < 1000)
        {
            Vector3 DLATester = DLADirection + activePrefab.transform.position;
            directionPrefab.transform.position = DLADirection;

            GameObject closestGeo = GetClosestGeo(DLATester, allPrefabs);
            Vector3 growthDir = DLATester - closestGeo.transform.position;
            AddPrefab(closestGeo, growthDir);
        }

    }

  private GameObject GetClosestGeo(Vector3 _testVector, List<GameObject> _allPrefabs)
  {
      GameObject closestGeo = null;

      if (allPrefabs.Count > 0)
      {
          float closestDistance = 1000f;

          for (int i = 0; i < allPrefabs.Count; i++)
          {
              GameObject currenGeo = allPrefabs[i];
              float dist = Vector3.Distance(_testVector, currenGeo.transform.position);

              if (dist < closestDistance && dist > 0.5f)
              {
                  closestGeo = currenGeo;
                  closestDistance = dist;
              }
          }
      }

      return closestGeo;
  }

  public void AddPrefab(GameObject _testGameObject, Vector3 _growthDir)
    {
        _growthDir = forceOrtho(_growthDir);

        Vector3 newPosition = _testGameObject.transform.position + _growthDir;
        GameObject newPrefab = Instantiate(inputPrefab, newPosition, Quaternion.identity);
        allPrefabs.Add(newPrefab);

        activePrefab.transform.position = newPosition;
    }

    void RunDLACoroutine()
    {
        StartCoroutine(nameof(RunDLA));
    }

    IEnumerator RunDLA()
    {
        for (int i = 0; i < DLAgrowthCount; i++)
        {
            yield return new WaitForSeconds(growthSpeed);
            AddDLAAgent();
        }
    }

    Vector3 forceOrtho(Vector3 _inputVector)
    {
        Vector3 temp = _inputVector;

        if (Mathf.Abs(temp.x) > Mathf.Abs(temp.y) && Mathf.Abs(temp.x) > Mathf.Abs(temp.z))
        {
            temp.y = 0;
            temp.z = 0;
        }

        else if (Mathf.Abs(temp.y) > Mathf.Abs(temp.z))
        {
            temp.x = 0;
            temp.z = 0;
        }

        else
        {
            temp.x = 0;
            temp.y = 0;
        }

        temp.Normalize();

        return temp;
    }
}
