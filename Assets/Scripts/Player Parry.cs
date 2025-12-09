using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    [Header("Parry Settings")]
    [SerializeField] private float parryActiveTime = 0.3f; // How long parry is active
    [SerializeField] private float parryCooldown = 0.5f;   // Time between parries
    
    [Header("Animation")]
    [SerializeField] private string parryAnimationName = "Player Parry";
    [SerializeField] private string parryTriggerName = "parry";
    
    // State
    private bool isParrying = false;
    private bool canParry = true;
    
    // Components
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        
        if (anim == null)
        {
            Debug.LogError("No Animator found on player!");
        }
        else
        {
            Debug.Log("✅ Animator found!");
            Debug.Log($"Looking for animation: '{parryAnimationName}'");
            Debug.Log($"Looking for trigger: '{parryTriggerName}'");
            
            // List all parameters to verify
            foreach (var param in anim.parameters)
            {
                Debug.Log($"Parameter: {param.name} (Type: {param.type})");
            }
        }
        
        Debug.Log("🛡️ Parry System Ready! Press Right Click to parry anytime.");
    }
    
    void Update()
    {
        // PRESS F TO PARRY - ANYTIME!
        if (Input.GetMouseButton(1) && canParry)
        {
            StartParry();
        }
    }
    
    void StartParry()
    {
        Debug.Log("🎮 Right Click KEY PRESSED - Starting Parry!");
        
        // Set state
        isParrying = true;
        canParry = false;
        
        // PLAY THE ANIMATION
        if (anim != null)
        {
            // Method 1: Use trigger (preferred)
            bool hasTrigger = false;
            foreach (var param in anim.parameters)
            {
                if (param.name == parryTriggerName && param.type == AnimatorControllerParameterType.Trigger)
                {
                    hasTrigger = true;
                    break;
                }
            }
            
            if (hasTrigger)
            {
                Debug.Log($"Setting trigger: '{parryTriggerName}'");
                anim.SetTrigger(parryTriggerName);
            }
            else
            {
                Debug.LogWarning($"No '{parryTriggerName}' trigger found. Playing animation directly.");
                anim.Play(parryAnimationName, 0, 0f);
            }
        }
        else
        {
            Debug.LogError("No Animator component!");
        }
        
        // Parry is active for a short time
        Invoke("EndParry", parryActiveTime);
        
        // Can parry again after cooldown
        Invoke("ResetCooldown", parryCooldown);
        
        Debug.Log($"🛡️ Parry active for {parryActiveTime} seconds");
    }
    
    void EndParry()
    {
        isParrying = false;
        Debug.Log("[Parry] Parry window closed");
    }
    
    void ResetCooldown()
    {
        canParry = true;
        Debug.Log("[Parry] Ready to parry again!");
    }
    
    public bool TryParryAttack(GameObject attacker)
    {
        if (isParrying)
        {
            Debug.Log($"✅ PARRY SUCCESS! Parried {attacker.name}");
            return true;
        }
        return false;
    }
    
    // Debug GUI to see status
   
    
    float GetCooldownTime()
    {
        return Mathf.Max(0f, parryCooldown - Time.timeSinceLevelLoad % parryCooldown);
    }
}