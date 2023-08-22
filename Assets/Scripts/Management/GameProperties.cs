// TODO: External config for game properties.
using UnityEngine;

[DisallowMultipleComponent]
public class GameProperties : MonoBehaviour {
    private float _sensitivity;
    private float _cameraVerticalFOV;

    private void Awake() {
        // Initialize variables.
        sensitivity = 2.0f;
    }

    private void Start() {
        cameraVerticalFOV = GetComponent<GameManager>().player.transform.parent.GetComponentInChildren<Camera>().fieldOfView;
    }

    public float sensitivity {
        get { return _sensitivity; }
        set { _sensitivity = Mathf.Round(Mathf.Clamp(value, 0.1f, 10.0f) * 10) / 10; } // Sensitivity always clamped and rounded to tenths.
    }

    public float cameraVerticalFOV {
        get { return _cameraVerticalFOV; }
        set {
            Camera camera = GameManager.instance.player.transform.parent.GetComponentInChildren<Camera>();
            _cameraVerticalFOV = camera.fieldOfView = camera.GetComponentInChildren<Camera>().fieldOfView = value;
        }
    }
}