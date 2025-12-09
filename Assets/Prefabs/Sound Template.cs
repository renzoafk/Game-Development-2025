using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundTemplate : MonoBehaviour
{
    [Header("Weapon")]
    public AudioClip[] swingSounds;

    private AudioSource weaponSource;
    void Start()
    {
        weaponSource = GetComponent<AudioSource>();
    }
    void PlayCrowbarSwing()
    {
        AudioClip clip = swingSounds[Random.Range(0, swingSounds.Length)];
        weaponSource.clip = clip;
        weaponSource.Play();
        //Debug.Log(clip.name);
    }
}
