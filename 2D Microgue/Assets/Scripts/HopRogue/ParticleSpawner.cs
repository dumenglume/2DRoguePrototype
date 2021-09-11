using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] GameObject _particlesToSpawn;

    void OnDestroy() => Instantiate(_particlesToSpawn, transform.position, Quaternion.identity);
}
