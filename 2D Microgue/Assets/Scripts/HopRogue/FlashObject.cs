using System.Collections;
using UnityEngine;
using HopRogue.Entities;

namespace HopRogue
{
/// <summary>
/// Requires a material of type GUI/Text Shader for flash to work
/// </summary>
public class FlashObject : MonoBehaviour
{
    [SerializeField] bool flashOnStart = false;
    public float _interval;
    public float _duration = 0.1f;
    SpriteRenderer _spriteRenderer;
    Material _materialDefault;
    [SerializeField] Material _materialFlashColor;

    void Awake()
    {
        _spriteRenderer  = _spriteRenderer ?? GetComponent<SpriteRenderer>();
        _materialDefault = _spriteRenderer.material;
    }

    void Start()
    {
        StopAllCoroutines();

        if (flashOnStart)
            FlashMaterial(); // TODO Allow different flash types
    }

    // TODO May need to cache current coroutine so that it can be stopped before a coroutine is re-initiated
    public void FlashMaterial(bool repeatFlash = false) => StartCoroutine(FlashMaterial(_duration, repeatFlash));

    public void FlashTint(bool repeatFlash = false) => StartCoroutine(FlashTint(_duration, Color.red, repeatFlash));

    public void FlashTransparent(bool repeatFlash = false) => StartCoroutine(FlashTransparent(_duration, repeatFlash));

    IEnumerator FlashTransparent(float duration, bool repeatFlash = false)
    {
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);

        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(duration);

        if (!repeatFlash)
            yield break;

        StartCoroutine(FlashTransparent(duration));
    }

    IEnumerator FlashTint(float duration, Color color, bool repeatFlash = false)
    {
        _spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);

        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(duration);

        if (!repeatFlash)
            yield break;

        StartCoroutine(FlashTint(duration, color, true));
    }

    IEnumerator FlashMaterial(float duration, bool repeatFlash = false)
    {
        _spriteRenderer.material = _materialFlashColor;
        yield return new WaitForSeconds(duration);

        _spriteRenderer.material = _materialDefault;
        yield return new WaitForSeconds(duration);

        if (!repeatFlash)
            yield break;

        StartCoroutine(FlashMaterial(_duration, true));
    }
}
}