using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue
{
public class EntityAudio : MonoBehaviour
{
    [SerializeField] AudioClip _sfxWalk;
    [SerializeField] AudioClip _sfxAttack;
    [SerializeField] AudioSource _audioSource;

    public void PlaySFXWalk()   => PlayOneShot(_sfxWalk);
    public void PlaySFXAttack() => PlayOneShot(_sfxAttack);

    void PlayOneShot(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);
}
}