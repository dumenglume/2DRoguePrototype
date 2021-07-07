using UnityEngine;

namespace FMT
{
[CreateAssetMenu(fileName = "EntityStats", menuName = "FMT/EntityStats", order = 0)]
public class EntityStats : ScriptableObject
{
    [SerializeField] new string name = "Entity";
    public string Name => name;

    [SerializeField] int powerLevel = 1;
    public int PowerLevel => powerLevel;

    [SerializeField] int healthMax = 1;
    public int HealthMax => healthMax;

    [SerializeField] int attackPower = 1;
    public int AttackPower => attackPower;

    [SerializeField] float movementDuration = 0.2f;
    public float MovementDuration => movementDuration;

    [SerializeField] float bumpDuration = 0.15f;
    public float BumpDuration => bumpDuration;
}
}