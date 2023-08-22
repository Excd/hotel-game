// TODO: Make flashlight an item.
// TODO: Make equipped flashlight the child of viewmodel-container independentchild.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : IndependentChild {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    private const float rotationSpeed = 10.0f;

    private Transform playerTransform, playerCamera;
    private Rigidbody playerRigidbody;
    private PlayerManager playerManager;
    private GameObject lightBeam;
    private IEnumerator toggleFlashLight;

    private void Awake() {
        lightBeam = transform.GetChild(0).gameObject;
        startPosition = transform.localPosition;
    }

    private void Start() {
        playerTransform = GameManager.instance.player.transform;
        playerCamera = playerTransform.parent.GetComponentInChildren<PlayerCamera>().transform;
        playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        playerManager = playerTransform.GetComponent<PlayerManager>();
        positionOffset = startPosition - playerTransform.localPosition;
        transform.localPosition = GetTargetPosition(playerTransform);
    }

    private void Update() {
        if (playerManager.isInputActive) {
            if (Input.GetButtonDown("Flashlight") && toggleFlashLight == null)
                StartCoroutine(toggleFlashLight = ToggleFlashlight());
        }

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            GetTargetPosition(playerTransform),
            Time.deltaTime * Mathf.Clamp(playerRigidbody.velocity.magnitude, playerManager.maxSpeed, playerRigidbody.velocity.magnitude)
        );

        transform.localRotation = Quaternion.Euler(
            Mathf.LerpAngle(transform.localEulerAngles.x, playerCamera.localEulerAngles.x, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(transform.localEulerAngles.y, playerCamera.localEulerAngles.y, Time.deltaTime * rotationSpeed * 5.0f),
            0.0f
        );
    }

    private IEnumerator ToggleFlashlight() {
        if (lightBeam.activeSelf) audioSource.clip = audioClips[0];
        else audioSource.clip = audioClips[1];

        audioSource.time = 0.0f;
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);
        lightBeam.SetActive(!lightBeam.activeSelf);
        toggleFlashLight = null;
    }
}