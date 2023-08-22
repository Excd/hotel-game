// TODO: Fix resize issues that occur on some higher resolutions. (Test on build.)
using UnityEngine;
using UnityEngine.EventSystems;

/*
    An event trigger class.
    When applied to a 'resize point' UI element allows the parent element to be resized with click and drag.
*/
[DisallowMultipleComponent]
public class DragResize : EventTrigger {
    // Minimum allowable distance between drag point and screen border in pixels.
    private const float marginScreen = 8.0f;
    // Minimum allowable window size in pixels.
    private const float marginWindow = 120.0f;

    private Vector2 dragOffset;
    private RectTransform windowRect;

    void Awake() {
        windowRect = transform.parent.GetComponent<RectTransform>();
    }

    // Called by the event system every time the pointer is moved during dragging.
    public override void OnDrag(PointerEventData eventData) {
        // Calculate parent rect offsetmax.x (right) based on mouse position within constraints.
        windowRect.offsetMax = new Vector2(
            Mathf.Clamp(
                Mathf.Clamp(Input.mousePosition.x, marginScreen, Screen.width),
                windowRect.offsetMin.x + marginWindow,
                Screen.width - marginScreen
            ) - dragOffset.x,
            windowRect.offsetMax.y
        );

        // Calculate parent rect offsetmin.y (bottom) based on mouse position within constraints.
        windowRect.offsetMin = new Vector2(
            windowRect.offsetMin.x,
            Mathf.Clamp(Input.mousePosition.y,
                marginScreen,
                Screen.height + windowRect.offsetMax.y - marginWindow
            ) - dragOffset.y
        );
    }

    // Called by the event system when a pointer down event occurs.
    public override void OnPointerDown(PointerEventData eventData) {
        // Store difference between mouse position and parent rect offsetmax.
        dragOffset = new Vector2(
            Input.mousePosition.x - windowRect.offsetMax.x,
            Input.mousePosition.y - windowRect.offsetMin.y
        );
    }
}