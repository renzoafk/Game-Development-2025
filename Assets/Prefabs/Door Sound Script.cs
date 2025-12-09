using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorSounds : MonoBehaviour
{
    [Header("Door")]
    public AudioClip[] doorSounds;

    private AudioSource doorSource;
    void Start()
    {
        doorSource = GetComponent<AudioSource>();
    }
    void DoorSwingSounds()
    {
        AudioClip clip = doorSounds[Random.Range(0, doorSounds.Length)];
        doorSource.clip = clip;
        doorSource.Play();
        //Debug.Log(clip.name);
    }
}
