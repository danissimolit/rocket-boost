using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _levelCompleteSound;
    [SerializeField] private AudioClip _crashSound;
    [SerializeField] private float _soundVolume = 1f;

    private static AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayCrashSound()
    {
        _audio.PlayOneShot(_crashSound, _soundVolume);
    }

    public void PlaySuccessSound()
    {
        _audio.PlayOneShot(_levelCompleteSound, _soundVolume);
    }
}
