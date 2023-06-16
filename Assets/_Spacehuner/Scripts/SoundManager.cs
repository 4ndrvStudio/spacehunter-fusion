using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip _soundClick;

    [SerializeField]  private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void PlaySoundButton()
    {
        _audioSource.clip = _soundClick;
        _audioSource.Play();
    }

}
