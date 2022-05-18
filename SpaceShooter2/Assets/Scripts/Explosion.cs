using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _explosionSoundSource;

    void Start()
    {
        _explosionSoundSource = GetComponent<AudioSource>();
        _explosionSoundSource.clip = _explosionSoundClip;
        _explosionSoundSource.Play();
        Destroy(gameObject, 2.6f);
    }
}
