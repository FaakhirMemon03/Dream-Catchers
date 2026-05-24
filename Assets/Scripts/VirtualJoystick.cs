using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Settings")]
    public float handleRange = 1f;
    public float deadZone = 0f;

    [Header("References")]
    public RectTransform background;
    public RectTransform handle;

    private Vector2 input = Vector2.zero;

    public float Horizontal => input.x;
    public float Vertical => input.y;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        
        if (input.magnitude > 1)
            input = input.normalized;

        handle.anchoredPosition = input * radius * handleRange;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    private Canvas canvas;
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }
}
