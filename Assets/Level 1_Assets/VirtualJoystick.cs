using UnityEngine;
using UnityEngine.EventSystems; // Required for UI events

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    [Range(0, 1)]
    public float handleLimit = 0.5f; // How far the handle can go

    // This is the output vector the Player will read (-1 to 1)
    public Vector2 InputDirection { set; get; }

    private Vector2 joystickPosition = Vector2.zero;
    private Camera cam;

    void Start()
    {
        // Setup initial values
        InputDirection = Vector2.zero;
        cam = Camera.main; // Only needed if using Screen Space - Camera, but safe to have
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 1. Get the background position directly (It is already in Screen Space)
        Vector2 position = background.position;

        // 2. Calculate offset
        Vector2 radius = background.sizeDelta / 2;
        InputDirection = (eventData.position - position) / (radius * canvasScaleFactor());

        // 3. Limit the handle movement
        HandleInput(InputDirection.magnitude, InputDirection.normalized);

        // 4. Move the handle
        handle.anchoredPosition = InputDirection * radius * handleLimit;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Treat the initial touch as a drag
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset everything when let go
        InputDirection = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    private void HandleInput(float magnitude, Vector2 normalised)
    {
        // Clamp values so it doesn't go outside the circle
        if (magnitude > 1)
            InputDirection = normalised;
    }

    // Helper to handle canvas scaling
    private float canvasScaleFactor()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return 1f;
        return canvas.scaleFactor;
    }
}