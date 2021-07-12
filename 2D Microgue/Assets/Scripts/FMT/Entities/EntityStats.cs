using UnityEngine;

namespace FMT
{
[CreateAssetMenu(fileName = "EntityStats", menuName = "FMT/EntityStats", order = 0)]
public class EntityStats : ScriptableObject
{
    [SerializeField] new string name = "Entity";
    public string Name => name;

    [SerializeField] int powerCurrent = 1;
    public int PowerCurrent { get => powerCurrent; set => powerCurrent = value; }

    [SerializeField] int powerMax = 1;
    public int PowerMax { get => powerMax; set => powerMax = value; }

    [SerializeField] float movementDuration = 0.2f;
    public float MovementDuration { get => movementDuration; set => movementDuration = value; }

    [SerializeField] float bumpDuration = 0.15f;
    public float BumpDuration { get => bumpDuration; set => bumpDuration = value; }
}
}