using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    // store which clip was used last so we can avoid repeating it
    private int lastIndex = -1;

    // this will be called by the animation event
    public void PlayFootstep()
    {
        if (audioSource == null || footstepClips == null || footstepClips.Length == 0)
            return;

        int index;

        if (footstepClips.Length == 1)
        {
            index = 0;
        }
        else
        {
            // pick a random index that is not the same as last time
            do
            {
                index = Random.Range(0, footstepClips.Length);
            }
            while (index == lastIndex);
        }

        lastIndex = index;

        audioSource.PlayOneShot(footstepClips[index]);
    }
}
