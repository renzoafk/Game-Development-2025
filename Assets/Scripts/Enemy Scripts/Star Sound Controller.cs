using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Starfish : MonoBehaviour
{
    [Header("Star")]
    public AudioClip[] StarSounds;
    public AudioClip[] DeathSounds;

    private AudioSource starSource;
    void Start()
    {
        starSource = GetComponent<AudioSource>();
    }
    void StarHurtSound()
    {
        AudioClip clip = StarSounds[Random.Range(0, StarSounds.Length)];
        starSource.clip = clip;
        starSource.Play();
        //Debug.Log(clip.name);
    }

    void StarDeathSound()
    {
        AudioClip clip = DeathSounds[Random.Range(0, StarSounds.Length)];
        starSource.clip = clip;
        starSource.Play();
        //Debug.Log(clip.name);
    }
}
