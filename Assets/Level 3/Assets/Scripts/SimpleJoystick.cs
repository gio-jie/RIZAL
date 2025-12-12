using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Joystick Components")]
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;

    [Header("Joystick Output")]
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private Vector2 input = Vector2.zero;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        // Convert screen point to local point in the joystick background
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBackground.sizeDelta.x);
            pos.y = (pos.y / joystickBackground.sizeDelta.y);

            // Normalize input (range -1 to 1)
            input = new Vector2(pos.x * 2, pos.y * 2);
            input = (input.magnitude > 1.0f) ? input.normalized : input;

            // Move joystick handle
            joystickHandle.anchoredPosition = new Vector2(
                input.x * (joystickBackground.sizeDelta.x / 2),
                input.y * (joystickBackground.sizeDelta.y / 2)
            );

            Horizontal = input.x;
            Vertical = input.y;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        Horizontal = 0;
        Vertical = 0;
    }
}
