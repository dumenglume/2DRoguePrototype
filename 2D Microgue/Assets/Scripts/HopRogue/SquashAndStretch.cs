using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue
{
public class SquashAndStretch : MonoBehaviour
{
    // TODO Create an animation controller later that each actor has
    // TODO Allow for animations to speed up by making _animationSpeed variable public

    [SerializeField] float _animationDuration = 1.0f;

    void Start()
    {
        Invoke(nameof(AnimateSquashAndStretch), Random.Range(0.0f, 1.0f));
    }

    void AnimateSquashAndStretch()
    {
        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.scaleY(this.gameObject, 0.9f, _animationDuration).setEaseInQuint());
        sequence.append(LeanTween.scaleY(this.gameObject, 1.0f, _animationDuration).setEaseInQuint().setOnComplete(AnimateSquashAndStretch));
    }
}
}