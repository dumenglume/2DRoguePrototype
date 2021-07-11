using UnityEngine;

namespace FMT
{
[CreateAssetMenu(fileName = "EntityStats", menuName = "FMT/EntityStats", order = 0)]
public class EntityStats : ScriptableObject
{
    [SerializeField] new string name = "Entity";
    public string Name => name;

    [SerializeField] int powerLevel = 1;
    public int PowerLevel { get => powerLevel; set => powerLevel = value; }

    [SerializeField] int healthCurrent = 1;
    public int HealthCurrent { get => healthCurrent; set => healthCurrent = value; }

    [SerializeField] int healthMax = 1;
    public int HealthMax { get => healthMax; set => healthMax = value; }

    [SerializeField] int attackPower = 1;
    public int AttackPower { get => attackPower; set => attackPower = value; }

    [SerializeField] float movementDuration = 0.2f;
    public float MovementDuration { get => movementDuration; set => movementDuration = value; }

    [SerializeField] float bumpDuration = 0.15f;
    public float BumpDuration { get => bumpDuration; set => bumpDuration = value; }
}
}