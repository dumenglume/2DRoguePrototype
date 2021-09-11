using System;
using UnityEngine;

namespace HopRogue.Entities
{
public class PlayerSkill : MonoBehaviour
{
    public static event Action<Entity, int, int> OnEntitySpawned;

    [SerializeField] ClickableSkill skillButton;

    [SerializeField] Entity _entityToSpawn; // TODO May need to move this to another script?

    public void SpawnEntityAtPostion(Vector3Int clickedPosition)
    {
        OnEntitySpawned?.Invoke(_entityToSpawn, clickedPosition.x, clickedPosition.y);
    }
}
}