using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class Pickup : MonoBehaviour, IAmPickupable // TODO Convert to scriptable object or a base item class?
{
    public static Action<int> PickedUpItem;

    [SerializeField] Vector2Int valueRange;

    int value;

    void Start() => value = Random.Range(valueRange.x, valueRange.y);

    public void TriggerPickup()
    {
        PickedUpItem?.Invoke(value);
        gameObject.SetActive(false);
    }
}
}