using UnityEngine;

[DisallowMultipleComponent]
public class PlayerActions : MonoBehaviour {
    [Tooltip("Layers to include for interactions.")]
    [SerializeField] private LayerMask interactLayerMask;
    [Tooltip("Unit distance in which player can interact.")]
    [SerializeField] private float interactDistance = 2.0f;

    private enum STATE { ENABLED, DISABLED, INACTIVE };

    private const float lerpMargin = 0.1f;

    private float currentZoom;
    private STATE zoom, crouch;
    private Rigidbody playerRigidbody;
    private PlayerHead playerHead;
    private PlayerManager playerManager;
    private PlayerCamera playerCamera;

    private void Awake() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerHead = GetComponentInChildren<PlayerHead>();
        playerManager = GetComponent<PlayerManager>();
        playerCamera = transform.parent.GetComponentInChildren<PlayerCamera>();
    }

    // INPUT HANDLING //
    private void Update() {
        if (playerManager.isInputActive) {
            if (Input.GetButtonDown("Zoom")) zoom = STATE.ENABLED;
            if (Input.GetButtonUp("Zoom")) zoom = STATE.DISABLED;
            if (Input.GetButtonDown("Crouch")) crouch = STATE.ENABLED;
            if (Input.GetButtonUp("Crouch")) crouch = STATE.DISABLED;
            if (Input.GetButtonDown("Interact")) Interact();
        }
    }

    // ACTION STATE HANDLING //
    private void FixedUpdate() {
        switch (zoom) {
            case STATE.ENABLED: if (!Zoom(true)) zoom = STATE.INACTIVE; break;
            case STATE.DISABLED: if (!Zoom(false)) zoom = STATE.INACTIVE; break;
            default: break;
        }
        switch (crouch) {
            case STATE.ENABLED: if (!Crouch(true)) crouch = STATE.INACTIVE; break;
            case STATE.DISABLED: if (!Crouch(false)) crouch = STATE.INACTIVE; break;
            default: break;
        }
    }

    private bool Zoom(bool active) {
        const float zoomMagnitude = 5.0f, zoomDegree = -15.0f; // Zoom degree (vertical FOV).

        if (active) {
            if (currentZoom > zoomDegree) {
                playerCamera.UpdateFOV(currentZoom = Mathf.Lerp(currentZoom, zoomDegree - lerpMargin, Time.fixedDeltaTime * zoomMagnitude));

                return true;
            }
            else playerCamera.UpdateFOV(currentZoom = zoomDegree);
        }
        else {
            if (currentZoom < 0.0f) {
                playerCamera.UpdateFOV(currentZoom = Mathf.Lerp(currentZoom, lerpMargin, Time.fixedDeltaTime * zoomMagnitude * 2.0f));

                return true;
            }
            else playerCamera.UpdateFOV(currentZoom = 0.0f);
        }

        return false; // Return false when conditions are met.
    }

    private bool Crouch(bool active) {
        const float crouchMagnitude = 5.0f;

        if (active) {
            if (transform.localScale.y > (playerManager.height / 2.0f)) {
                transform.localScale = Vector3.Lerp(
                    transform.localScale,
                    new Vector3(transform.localScale.x, (playerManager.height / 2.0f) - lerpMargin, transform.localScale.z),
                    Time.fixedDeltaTime * crouchMagnitude
                );
                playerHead.UpdatePosition();
                playerRigidbody.AddRelativeForce(Vector3.down * playerManager.maxAccel, ForceMode.Impulse);
                playerManager.isStanding = false;

                return true;
            }
            else {
                transform.localScale = new Vector3(transform.localScale.x, playerManager.height / 2.0f, transform.localScale.z);
                playerHead.UpdatePosition();
                playerManager.crouchSpeed = -1.0f;
            }
        }
        else {
            if (transform.localScale.y < playerManager.height) {
                if (playerManager.hasHeadroom) {
                    transform.localScale = Vector3.Lerp(
                        transform.localScale,
                        new Vector3(transform.localScale.x, playerManager.height + lerpMargin, transform.localScale.z),
                        Time.fixedDeltaTime * crouchMagnitude
                    );
                }
                playerHead.UpdatePosition();

                return true;
            }
            else {
                transform.localScale = new Vector3(transform.localScale.x, playerManager.height, transform.localScale.z);
                playerHead.UpdatePosition();
                playerManager.crouchSpeed = 0.0f;
                playerManager.isStanding = true;
            }
        }

        return false; // Return false when conditions are met.
    }

    private void Interact() {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactDistance, interactLayerMask)) {
            // Ignore raycast hits on the default layer.
            if (hit.transform.gameObject.layer == 0) return;

            Interactable interactable = hit.transform.GetComponentInChildren<Interactable>();

            if (interactable != null) {
                // Destroy rigidbody on items.
                if (interactable.gameObject.layer == 9) {
                    Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                    if (rigidbody != null) Destroy(rigidbody);
                }

                interactable.Interact();
            }
        }
    }

    public void ResetInput() {
        zoom = STATE.DISABLED;
        crouch = STATE.DISABLED;
    }
}