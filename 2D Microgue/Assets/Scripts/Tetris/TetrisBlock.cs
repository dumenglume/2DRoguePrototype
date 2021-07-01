using System;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public event Action BlockDiscarded;

    [SerializeField] float animationSpeed = 0.35f;
    public Vector3Int blockPosition { get; set; }

    bool isOverflow = false;
    public bool IsOverflow { get { return isOverflow; } set { isOverflow = value; } }

    public void TweenDiscard()
    {
        LeanTween.scale(gameObject, Vector3.zero, animationSpeed).setEaseInOutBack().setOnComplete(OnDiscard);
    }

    void OnDiscard()
    {
        BlockDiscarded?.Invoke();
        gameObject.SetActive(false); // TODO Implement pooling system
    }
}
