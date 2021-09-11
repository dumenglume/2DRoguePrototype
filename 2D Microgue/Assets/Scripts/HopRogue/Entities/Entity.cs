using System;
using UnityEngine;

namespace HopRogue.Entities
{
public class Entity : MonoBehaviour
{
    public event Action<Entity> OnEntityDied;
    public event Action<Entity, Vector3Int, Vector3Int> OnEntityMoved;
    public event Action OnEntityTookDamage;

    public Vector3Int WorldPosition { get; set; }

    [SerializeField] protected int _health = 1;
    public int Health { get => _health; set => _health = value; }

    [SerializeField] protected int _healthMax = 1;
    public int HealthMax { get => _healthMax; set => _healthMax = value; }

    public bool BlocksMovement { get; protected set; } = true; // ! Figure out how to inherit this into subclasses

    public EntityAudio EntityAudio;
    public GameObject Particles;

    [SerializeField] FlashObject flashObject;

    protected virtual void Start()
    {
        _health = _healthMax;
    }

    public void SetPosition(int x, int y)
    {
        WorldPosition      = new Vector3Int(x, y, 0);
        transform.position = WorldPosition;
    }

    # region Movement Methods // TODO Move movement methods to own class (currently only used for announcing events)
    protected void BroadcastMove(Vector3Int oldPosition, Vector3Int newPosition) => OnEntityMoved?.Invoke(this, oldPosition, newPosition);

    # endregion

    # region Combat Methods // TODO Move combat methods to own class
    public void TakeDamage(int damageAmount = 1)
    {
        Health -= damageAmount;

        flashObject.FlashMaterial(); // TODO Change to event system

        if (Health <= 0)
            ResolveDeath();
    }

    public virtual void ResolveDeath()
    {
        // TODO Drop item

        SpawnParticles();
        OnEntityDied?.Invoke(this);
    }

    # endregion

    void SpawnParticles() => Instantiate(Particles, transform.position, Quaternion.identity);
}
}