using UnityEngine;
using UnityEngine.EventSystems;

/*
    An event trigger class.
    When applied to a 'drag bar' UI element allows the parent element to be moved with click and drag.
*/
[DisallowMultipleComponent]
public class DragMove : EventTrigger {
    // Minimum allowable distance between drag point and screen border in pixels.
    private const float margin = 12.0f;

    private Vector2 dragOffset;

    // Called by the event system every time the pointer is moved during dragging.
    public override void OnDrag(PointerEventData eventData) {
        // Calculate parent position from mouse position within constraints.
        transform.parent.position = new Vector2(
            Mathf.Clamp(Input.mousePosition.x, margin, Screen.width - margin),
            Mathf.Clamp(Input.mousePosition.y, margin, Screen.height - margin)
        ) - dragOffset;
    }

    // Called by the event system when a pointer down event occurs.
    public override void OnPointerDown(PointerEventData eventData) {
        // Store difference between mouse position and parent position.
        dragOffset = new Vector2(
            Input.mousePosition.x - transform.parent.position.x,
            Input.mousePosition.y - transform.parent.position.y
        );
    }
}