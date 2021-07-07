using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class ItemSpawner : MonoBehaviour
{
    public IEnumerator Co_SpawnItems()
    {
        print("Items being spawned");
        yield return new WaitForSeconds(DungeonManager.Instance.GenerationSpeed);
    }

    public void ClearAllItems()
    {

    }
}
}