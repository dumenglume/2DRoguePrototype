using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class CameraShake : MonoBehaviour // TODO Convert to static class
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    [SerializeField] Transform camTransform;
    
    // How long the object should shake for.
    [SerializeField] float shakeDuration = 0f;
    
    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] float shakeIntensity = 0.25f;
    [SerializeField] float durationDecayFactor = 1.0f;
    [SerializeField] float intensityDecayFactor = 1.0f;
    
    Vector3 originalPos;
      
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
      
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
        CombatManager.ShakeCamera += Shake;
    }

    void OnDisable()
    {
        CombatManager.ShakeCamera -= Shake;
    }

    public void Shake(float _shakeDuration, float _shakeAmount, float _shakeDecay)
    {
        shakeDuration        = _shakeDuration;
        shakeIntensity       = _shakeAmount;
        intensityDecayFactor = _shakeDecay;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeIntensity;
          
            shakeDuration -= Time.deltaTime * durationDecayFactor;
            shakeIntensity   -= Time.deltaTime * intensityDecayFactor;
        }

        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }

        if (shakeIntensity < 0)
        {
            shakeIntensity = 0;
        }
    }
}
}