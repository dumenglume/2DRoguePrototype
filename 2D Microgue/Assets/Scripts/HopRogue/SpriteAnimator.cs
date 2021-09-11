using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue
{
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> spriteFrames;
    [SerializeField] float durationBetweenFrames = 0.5f; // In seconds

    [SerializeField] bool loopAnimation = true;
    [SerializeField] bool destroyAfterAnimation = false;

    void Awake() => spriteRenderer = spriteRenderer ?? GetComponent<SpriteRenderer>();

    void Start() => StartCoroutine(AnimationIdle(spriteFrames));

    IEnumerator AnimationIdle(List<Sprite> spriteFrameList)
    {
        int i = 0;

        while(i < spriteFrameList.Count)
        {
            spriteRenderer.sprite = spriteFrameList[i];
            i++;

            yield return new WaitForSeconds(durationBetweenFrames);
        }

        if (loopAnimation)
            StartCoroutine(AnimationIdle(spriteFrames));

        if (destroyAfterAnimation)
            Destroy(gameObject);
        else
            yield break;
    }
}
}