using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class FloatingText : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveDuration = 0.25f;
    [SerializeField] float moveYOffset = 1.0f;

    [Header("Hold Settings")]
    [SerializeField] float holdDuration = 0.25f;

    [Header("Flash Settings")]
    [SerializeField] int flashCount = 5;
    [SerializeField] float flashDurationOff = 0.05f;
    [SerializeField] float flashDurationOn = 0.05f;
    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        transform.position = Vector3.zero;

        meshRenderer.enabled = true;

        LeanTween.moveLocalY(gameObject, transform.localPosition.y + moveYOffset, moveDuration).setEaseOutQuint().setOnComplete(() => 
        {
            StartCoroutine(Co_FlashObject(flashDurationOff, flashDurationOn));
        });    
    }

    IEnumerator Co_FlashObject(float _flashDurationOff, float _flashDurationOn)
    {
        for (int i = 0; i < flashCount; i++)
        {
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(_flashDurationOff);
            meshRenderer.enabled = true;
            yield return new WaitForSeconds(_flashDurationOn);
        }

        meshRenderer.enabled = false;
    }
}
}