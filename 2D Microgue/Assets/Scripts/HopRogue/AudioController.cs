using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip bgm;
    [SerializeField] AudioClip _stairs;
    [SerializeField] AudioSource _audioSource;

    // public void PlayBGM() => PlayLoop(bgm);
    public void PlaySFXStairs() => PlayOneShot(_stairs);

    void PlayOneShot(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);
    // void PlayLoop(AudioClip audioClip) => _audioSource.Play(audioClip);
}
