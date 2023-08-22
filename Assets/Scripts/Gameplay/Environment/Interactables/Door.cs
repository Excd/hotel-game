// TODO: Add jiggle animation to locked door interaction.
using System.Collections;
using UnityEngine;

public class Door : Interactable {
    [Tooltip("Whether or not the door starts locked.")]
    [SerializeField] private bool startLocked = false;
    [Tooltip("Unique integer ID of the key used to unlock the door, leave zero if none.")]
    [SerializeField] private int keyID;
    [Tooltip("Time in seconds before attempting to close door.")]
    [SerializeField] private float closeInterval = 1.5f;
    [Tooltip("Minimum unit distance from player before door is able to close.")]
    [SerializeField] private float closeDistance = 3.5f;

    private bool isInteractive = true, isLocked = false;

    private UIManager uiManager;
    private JointSpring baseSpring, newSpring;
    private IEnumerator openDoor;

    private void Awake() {
        newSpring = baseSpring = GetComponent<HingeJoint>().spring;
        LockDoor(startLocked);
    }

    private void Start() {
        uiManager = GameManager.instance.userInterface.GetComponent<UIManager>();
    }

    private void OnCollisionEnter(Collision other) {
        if (!isInteractive && other.gameObject == GameManager.instance.player) {
            StopCoroutine(openDoor);
            CloseDoor();
        }
    }

    public override void Interact() {
        if (isInteractive) {
            if (!isLocked) {
                isInteractive = false;
                StartCoroutine(openDoor = OpenDoor());
            }
            else {
                LockDoor(!GameManager.instance.player.GetComponentInChildren<PlayerInventory>().GetInventory().Exists(x =>
                    (x.GetComponentInChildren<DoorKey>() != null) && (x.GetComponentInChildren<DoorKey>().keyID == keyID)
                ));

                if (!isLocked) {
                    string message = "Door unlocked using " +
                        GameManager.instance.player.GetComponentInChildren<PlayerInventory>().GetInventory().Find(x =>
                            (x.GetComponentInChildren<DoorKey>() != null) && (x.GetComponentInChildren<DoorKey>().keyID == keyID)
                        ).GetComponentInChildren<DoorKey>().displayName + "!";
                    uiManager.console.GetComponent<Console>().Print(message);
                    Debug.Log(message);
                }
                else {
                    string message = "Door is locked!";
                    uiManager.console.GetComponent<Console>().Print(message);
                    Debug.Log(message);
                }
            }
        }
    }

    private void LockDoor(bool lockState) {
        GetComponent<Rigidbody>().isKinematic = lockState;
        isLocked = lockState;
    }

    private void CloseDoor() {
        GetComponent<HingeJoint>().spring = baseSpring;
        isInteractive = true;
    }

    IEnumerator OpenDoor() {
        const float springTarget = 89.0f;

        Vector3 doorDirection = transform.InverseTransformDirection(GameManager.instance.player.transform.TransformDirection(Vector3.forward));
        newSpring.targetPosition = (doorDirection.x <= 0.0f) ? springTarget : -springTarget;
        GetComponent<HingeJoint>().spring = newSpring;

        do {
            yield return new WaitForSeconds(closeInterval);
        } while (Vector3.Distance(GameManager.instance.player.transform.position, transform.position) < closeDistance);

        CloseDoor();
    }
}