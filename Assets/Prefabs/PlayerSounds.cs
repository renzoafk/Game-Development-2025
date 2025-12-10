using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerSounds : MonoBehaviour
{
    [Header("Weapon")]
    public AudioClip[] swingSounds;
    public AudioClip[] swingSoundsSlam;

    [Header("Footsteps")]
    public List<AudioClip> BrickFS;

    [Header("Landing")]
    public List<AudioClip> Landing;

    [Header("Parry")]
    public List<AudioClip> Parry;


    private AudioSource weaponSource, footstepSource, landingSource, parrySource;
    void Start()
    {
        weaponSource = GetComponent<AudioSource>();
        footstepSource = GetComponent<AudioSource>();
        landingSource = GetComponent<AudioSource>();
        parrySource = GetComponent<AudioSource>();
    }

    void PlayCrowbarSwing()
    {
        AudioClip clip = swingSounds[Random.Range(0, swingSounds.Length)];
        weaponSource.clip = clip;
        weaponSource.Play();
        //Debug.Log(clip.name);
    }
    void PlayCrowbarSwingSlam()
    {
        AudioClip clip = swingSoundsSlam[Random.Range(0, swingSoundsSlam.Length)];
        weaponSource.clip = clip;
        weaponSource.Play();
        //Debug.Log(clip.name);
    }

    void PlayFootstep()
    {
        AudioClip clip = BrickFS[Random.Range(0, BrickFS.Count)];
        footstepSource.clip = clip;
        footstepSource.Play();
        //Debug.Log(clip.name);

    }

    void PlayLanding()
    {
        AudioClip clip = Landing[Random.Range(0, Landing.Count)];
        landingSource.clip = clip;
        landingSource.Play();
        //Debug.Log(clip.name);

    }

    void PlayParry()
    {
        AudioClip clip = Parry[Random.Range(0, Parry.Count)];
        parrySource.clip = clip;
        parrySource.Play();
        //Debug.Log(clip.name);
    }
}
