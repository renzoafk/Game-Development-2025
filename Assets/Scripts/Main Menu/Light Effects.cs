using UnityEngine;

public class LightEffects : MonoBehaviour
{
    public Light pointLight;
    public float flickerSpeed = 2f;
    public float flickerAmount = 0.3f;
    private float baseIntensity;

    void Start()
    {
        if (pointLight == null)
            pointLight = GetComponent<Light>();
        baseIntensity = pointLight.intensity;
    }

    void Update()
    {
        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) * flickerAmount;
        pointLight.intensity = baseIntensity + flicker - (flickerAmount * 0.5f);
    }
}
