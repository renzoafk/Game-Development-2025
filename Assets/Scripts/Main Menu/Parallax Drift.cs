using UnityEngine;
using UnityEngine.InputSystem;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)] public float strength = 0.08f;
    public Vector2 autoScroll = new Vector2(0.002f, 0f);
    private Vector3 start;

    void Start() { start = transform.position; }

    void Update()
    {
        // Read mouse position with the new Input System
        Vector2 mp = Mouse.current != null
            ? Mouse.current.position.ReadValue()
            : new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Vector2 mouse = new Vector2(mp.x / Screen.width - 0.5f, mp.y / Screen.height - 0.5f);
        Vector3 mouseOffset = new Vector3(-mouse.x, -mouse.y, 0f) * strength;

        transform.position = start
                           + mouseOffset
                           + new Vector3(Time.time * autoScroll.x, Time.time * autoScroll.y, 0f);
    }
}
