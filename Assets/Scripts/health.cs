using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OxygenHealthSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDrainRate = 2f; 
    public float enemyOxygenReward = 25f; 
    public float tankOxygenReward = 50f; 

    [Header("UI References")]
    public Slider oxygenSlider;
    public Image oxygenFillImage;
    public Color fullOxygenColor = Color.cyan;
    public Color lowOxygenColor = Color.red;
    public GameObject lowOxygenWarning;

    [Header("Low Oxygen Effects")]
    public float lowOxygenThreshold = 30f;
    public AudioClip lowOxygenSound;
    public ParticleSystem lowOxygenParticles;

    private bool isOxygenDraining = true;
    private AudioSource audioSource;
    private PlayerMovement playerMovement;

    void Start()
    {
        currentOxygen = maxOxygen;
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = currentOxygen;
        }

        UpdateOxygenUI();
        StartCoroutine(OxygenDrainRoutine());
    }

    IEnumerator OxygenDrainRoutine()
    {
        while (isOxygenDraining)
        {
            if (currentOxygen > 0)
            {
                DrainOxygen(oxygenDrainRate * Time.deltaTime);
            }
            else
            {
                OnOxygenDepleted();
            }
            yield return null;
        }
    }

    void DrainOxygen(float amount)
    {
        currentOxygen -= amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        UpdateOxygenUI();

        if (currentOxygen <= lowOxygenThreshold && currentOxygen > 0)
        {
            OnLowOxygen();
        }
        else if (currentOxygen > lowOxygenThreshold)
        {
            OnOxygenRestored();
        }
    }

    public void AddOxygen(float amount)
    {
        currentOxygen += amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        UpdateOxygenUI();

        StartCoroutine(OxygenGainEffect());
    }

    public void OnEnemyDefeated()
    {
        AddOxygen(enemyOxygenReward);
        Debug.Log("Gained " + enemyOxygenReward + " oxygen from enemy!");
    }

    public void OnOxygenTankPickup()
    {
        AddOxygen(tankOxygenReward);
        Debug.Log("Gained " + tankOxygenReward + " oxygen from tank!");
    }

    void OnLowOxygen()
    {
        if (lowOxygenWarning != null)
            lowOxygenWarning.SetActive(true);

        if (lowOxygenSound != null && audioSource != null)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(lowOxygenSound);
        }

        
        if (lowOxygenParticles != null && !lowOxygenParticles.isPlaying)
            lowOxygenParticles.Play();

        if (playerMovement != null)
            playerMovement.speed = playerMovement.speed * 0.7f;
    }

    void OnOxygenRestored()
    {
        if (lowOxygenWarning != null)
            lowOxygenWarning.SetActive(false);

        if (lowOxygenParticles != null && lowOxygenParticles.isPlaying)
            lowOxygenParticles.Stop();

        if (playerMovement != null)
            playerMovement.speed = playerMovement.originalSpeed;
    }

    void OnOxygenDepleted()
    {
        Debug.Log("Player died from oxygen depletion!");
        
        isOxygenDraining = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;

            Animator anim = GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("die");
        }

        
        Invoke("GameOver", 2f);
    }

    void GameOver()
    
    {

        Debug.Log("GAME OVER - Oxygen depleted");
        Time.timeScale = 0f; 
    
    }

    void UpdateOxygenUI()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxygen;
        }

        if (oxygenFillImage != null)
        {
            oxygenFillImage.color = Color.Lerp(lowOxygenColor, fullOxygenColor, currentOxygen / maxOxygen);
        }
    }

    IEnumerator OxygenGainEffect()
    {
        
        if (oxygenFillImage != null)
        {
            Color originalColor = oxygenFillImage.color;
            oxygenFillImage.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            oxygenFillImage.color = originalColor;
        }
    }

      public float GetCurrentOxygen() => currentOxygen;
    public float GetOxygenPercentage() => currentOxygen / maxOxygen;
    public bool IsOxygenLow() => currentOxygen <= lowOxygenThreshold;
}