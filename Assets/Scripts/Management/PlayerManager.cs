// TODO: Separate PlayerProperties from PlayerManager, make PlayerManager top-level (player parent).
// TODO: Hold references to important player components, refactor code to use these references.
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerManager : MonoBehaviour {
    private float _fovModifier;
    private float _sensitivityModifier;
    private float _speedModifier;
    private float _height;
    private float _maxSpeed;
    private float _maxAccel;
    private float _maxFriction;
    private float _crouchSpeed;
    private bool _isGrounded;
    private bool _hasHeadroom;
    private bool _isStanding;
    private bool _isInputActive;

    private void Awake() {
        DontDestroyOnLoad(transform.parent.gameObject);

        // Initialization references.
        GameObject player = gameObject;
        CapsuleCollider playerCollider = player.GetComponent<CapsuleCollider>();

        // Initialize variables.
        fovModifier = 0;
        sensitivityModifier = 0.0f;
        speedModifier = 0.0f;
        height = player.transform.localScale.y;
        maxSpeed = 3.0f;
        maxAccel = 8.0f;
        maxFriction = Mathf.Max(1.5f, playerCollider.material.dynamicFriction);
        crouchSpeed = 0.0f;
        isGrounded = true;
        hasHeadroom = true;
        isStanding = true;
        isInputActive = true;
    }

    public void Initialize(Transform spawnPoint) {
        PlayerController playerController = GetComponent<PlayerController>();
        PlayerActions playerActions = GetComponent<PlayerActions>();

        transform.parent.position = spawnPoint.position;
        transform.parent.rotation = spawnPoint.rotation;
        transform.localEulerAngles = Vector3.zero;
        transform.parent.GetComponentInChildren<PlayerCamera>().ResetRotation();
        playerController.Teleport(Vector3.up);
        playerActions.ResetInput();
        GetComponentInChildren<PlayerFeet>().ClearCollisions();
        GetComponentInChildren<PlayerHead>().ClearCollisions();
    }

    public float fovModifier {
        get { return _fovModifier; }
        set { _fovModifier = value; }
    }

    public float sensitivityModifier {
        get { return _sensitivityModifier; }
        set { _sensitivityModifier = value; }
    }

    public float speedModifier {
        get { return _speedModifier; }
        set { _speedModifier = value; }
    }

    public float height {
        get { return _height; }
        set { _height = value; }
    }

    public float maxSpeed {
        get { return _maxSpeed + _speedModifier; } // Returns with modifier.
        set { _maxSpeed = value; }
    }

    public float maxAccel {
        get { return _maxAccel; }
        set { _maxAccel = value; }
    }

    public float maxFriction {
        get { return _maxFriction; }
        set { _maxFriction = value; }
    }

    public float crouchSpeed {
        get { return _crouchSpeed; }
        set { _crouchSpeed = value; }
    }

    public bool isGrounded {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }

    public bool hasHeadroom {
        get { return _hasHeadroom; }
        set { _hasHeadroom = value; }
    }

    public bool isStanding {
        get { return _isStanding; }
        set { _isStanding = value; }
    }

    public bool isInputActive {
        get { return _isInputActive; }
        set { _isInputActive = value; }
    }
}