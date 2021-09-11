using UnityEngine;

namespace GraphTest
{
public class AttachToObject : MonoBehaviour
{
    void OnEnable()
    {
        EntitySpawner.allEntitiesSpawned += AttachToTarget;
        DungeonGenerator.newDungeon += DetachFromTarget;
    }

    void OnDisable()
    {
        EntitySpawner.allEntitiesSpawned -= AttachToTarget;
        DungeonGenerator.newDungeon -= DetachFromTarget;
    }

    void AttachToTarget()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        transform.SetParent(playerTransform);
    }

    void DetachFromTarget()
    {
        transform.SetParent(null);
    }
}
}