using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerSounds : MonoBehaviour
{
    [Header("Weapon")]
    public AudioClip[] swingSounds;

    [Header("Footsteps")]
    public List<AudioClip> BrickFS;

    [Header("Landing")]
    public List<AudioClip> Landing;


    private AudioSource weaponSource, footstepSource, LandingSource;
    void Start()
    {
        weaponSource = GetComponent<AudioSource>();
        footstepSource = GetComponent<AudioSource>();
        LandingSource = GetComponent<AudioSource>();
    }

    void PlayCrowbarSwing()
    {
        AudioClip clip = swingSounds[Random.Range(0, swingSounds.Length)];
        weaponSource.clip = clip;
        weaponSource.Play();
        //Debug.Log(clip.name);
    }

    void PlayFootstep()
    {
        AudioClip clip = BrickFS[Random.Range(0, BrickFS.Count)];
        weaponSource.clip = clip;
        weaponSource.Play();
        //Debug.Log(clip.name);

    }

    void PlayLanding()
    {
        AudioClip clip = Landing[Random.Range(0, Landing.Count)];
        LandingSource.clip = clip;
        LandingSource.Play();
        //Debug.Log(clip.name);

    }
}
