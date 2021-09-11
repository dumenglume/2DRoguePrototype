using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float timeBetweenSpread = 0.1f;

    [SerializeField] Vector3Int directionToSpawn = Vector3Int.zero;
    [SerializeField] GameObject projectileToSpawnNext;

    void Start() => Invoke(nameof(SpreadProjectile), timeBetweenSpread);

    void SpreadProjectile()
    {
        Vector3 nextPosition = transform.position + directionToSpawn;
        Instantiate(projectileToSpawnNext, nextPosition, Quaternion.identity);
    }
}
