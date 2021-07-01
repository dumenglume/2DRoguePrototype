using System.Collections;
using UnityEngine;

public class FlashObject : MonoBehaviour
{
    public float interval;
    public float duration;
    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (interval > 0f)
            StartCoroutine(Flash(duration));
    }

    IEnumerator Flash(float _duration)
    {
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(_duration);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(_duration);

        StartCoroutine(Flash(duration));
    }
}
