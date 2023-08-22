// TODO: Improve how FOV changes are handled.
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerCamera : IndependentChild {
    private const float verticalBound = 75.0f;

    private float moveSpeed;
    private Vector2 rotation;
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private Camera mainCam, viewmodelCam;
    private PlayerManager playerManager;
    private GameProperties gameProperties;

    private void Awake() {
        mainCam = GetComponentInChildren<Camera>();
        viewmodelCam = mainCam.transform.GetComponentInChildren<Camera>();
        startPosition = transform.localPosition;
    }

    private void Start() {
        playerTransform = GameManager.instance.player.transform;
        gameProperties = GameManager.instance.GetComponent<GameProperties>();
        playerManager = playerTransform.GetComponent<PlayerManager>();
        playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        rotation = playerTransform.localEulerAngles;
        positionOffset = startPosition - playerTransform.localPosition;
        transform.localPosition = GetTargetPosition(playerTransform);
    }

    private void Update() {
        // Move head toward adjusted player position every frame.
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            GetTargetPosition(playerTransform),
            Time.deltaTime * Mathf.Clamp(playerRigidbody.velocity.magnitude, playerManager.maxSpeed, playerRigidbody.velocity.magnitude)
        );

        // Use mouse axis inputs to increment rotation vector scaled by sensitivity values.
        if (playerManager.isInputActive) {
            rotation.y += Input.GetAxisRaw("Mouse X") * (gameProperties.sensitivity + playerManager.sensitivityModifier);
            rotation.x -= Input.GetAxisRaw("Mouse Y") * (gameProperties.sensitivity + playerManager.sensitivityModifier);
            rotation.x = Mathf.Clamp(rotation.x, -verticalBound, verticalBound); // Clamp rotation x value between vertical bounds.
            transform.localEulerAngles = rotation; // Rotate head
        }
    }

    private void FixedUpdate() {
        if (playerManager.isInputActive)
            playerTransform.localEulerAngles = new Vector2(0.0f, rotation.y); // Rotate player about y in fixed timestep.
    }

    public void ResetRotation() {
        transform.localEulerAngles = rotation = Vector2.zero;
    }

    public void UpdateFOV(float degree) {
        playerManager.fovModifier = degree;
        mainCam.fieldOfView = viewmodelCam.fieldOfView = gameProperties.cameraVerticalFOV + playerManager.fovModifier;
    }
}