// TODO: Limit movement slope detection height.
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {
    [Tooltip("Layers to include in slope detection.")]
    [SerializeField] private LayerMask slopeLayerMask;
    [Tooltip("The player respawns when under this y value.")]
    [SerializeField] private float yFloor = -25.0f;

    private const float lerpMargin = 0.1f;

    private float translation, strafe, minAccel, curAccel, posDifference, minFriction, drag, adjustedMaxSpeed;
    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;
    private PlayerManager playerManager;

    private void Awake() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerManager = GetComponent<PlayerManager>();
        curAccel = minAccel = 0.0f;
        minFriction = playerCollider.material.dynamicFriction;
        drag = playerRigidbody.drag;
    }

    private void Update() {
        if (playerManager.isInputActive) {
            translation = Input.GetAxisRaw("Vertical");
            strafe = Input.GetAxisRaw("Horizontal");
        }
    }

    private void FixedUpdate() {
        // Respawn below yFloor.
        if (transform.position.y < yFloor)
            playerManager.Initialize(GameObject.FindGameObjectWithTag("Respawn").transform);

        if (playerManager.isInputActive && playerManager.isGrounded && (translation != 0.0f || strafe != 0.0f)) {
            playerCollider.material.dynamicFriction = minFriction;                  // Reset to minimum friction when moving.
            adjustedMaxSpeed = playerManager.maxSpeed + playerManager.crouchSpeed;  // Adjust maximum speed value if necessary.
            // Increase acceleration smoothly.
            curAccel = Mathf.Clamp(
                Mathf.SmoothStep(
                    curAccel,
                    playerManager.maxAccel + lerpMargin,
                    Time.fixedDeltaTime * playerManager.maxAccel
                ),
                minAccel,
                playerManager.maxAccel
            );

            // Calculate slope height.
            if (Physics.Raycast(transform.position + new Vector3(playerRigidbody.velocity.x, 0.0f, playerRigidbody.velocity.z).normalized / 2.0f, Vector3.down, out RaycastHit hit, 2.0f, slopeLayerMask))
                posDifference = (transform.position.y - transform.localScale.y) - hit.point.y;
            else posDifference = 0.0f;
            // Add force from normalized movement vector.
            if (playerRigidbody.velocity.magnitude < adjustedMaxSpeed)
                playerRigidbody.AddRelativeForce((new Vector3(strafe, 0.0f, translation).normalized + new Vector3(0.0f, -posDifference, 0.0f)) * curAccel, ForceMode.Impulse);
            // Clamp velocity to adjusted maximum speed.
            else playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, adjustedMaxSpeed);
        }
        else {
            // Increase friction smoothly.
            playerCollider.material.dynamicFriction = Mathf.Clamp(
                Mathf.SmoothStep(playerCollider.material.dynamicFriction, playerManager.maxFriction + lerpMargin, Time.fixedDeltaTime * playerManager.maxAccel),
                minFriction,
                playerManager.maxFriction
            );

            // Reset acceleration when not moving.
            if (curAccel != minAccel && (translation == 0.0f && strafe == 0.0f)) curAccel = minAccel;
        }

        playerCollider.material.staticFriction = playerCollider.material.dynamicFriction;
    }

    private void OnCollisionStay() {
        if (playerManager.isGrounded) playerRigidbody.drag = drag;
    }

    public void Teleport(Vector3 position) {
        transform.localPosition = position;

        foreach (IndependentChild child in transform.parent.GetComponentsInChildren<IndependentChild>())
            child.transform.localPosition = child.GetTargetPosition(transform);
    }
}