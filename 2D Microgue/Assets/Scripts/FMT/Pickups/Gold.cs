using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class Gold : MonoBehaviour, IAmPickupable
{
    public static Action<int> PickedUpGold;

    [SerializeField] Vector2Int goldValueRange;

    int goldValue;

    void Start() => goldValue = Random.Range(goldValueRange.x, goldValueRange.y);

    public void TriggerPickup() => PickedUpGold?.Invoke(goldValue);
}
}