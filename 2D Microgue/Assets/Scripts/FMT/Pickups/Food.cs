using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class Food : MonoBehaviour, IAmPickupable // TODO Convert to scriptable object or a base item class?
{
    public static Action<int> PickedUpFood;

    [SerializeField] Vector2Int valueRange;

    int value;

    void Start() => value = Random.Range(valueRange.x, valueRange.y);

    public void TriggerPickup()
    {
        PickedUpFood?.Invoke(value);
        gameObject.SetActive(false);
    }
}
}