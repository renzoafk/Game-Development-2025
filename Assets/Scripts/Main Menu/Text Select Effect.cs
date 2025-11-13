using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPop : MonoBehaviour, IPointerClickHandler
{
    public float popScale = 1.15f;
    public float returnSpeed = 10f;

    private Vector3 originalScale;
    private bool isPopping = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isPopping)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * returnSpeed);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = originalScale * popScale;
        isPopping = true;
    }
}
