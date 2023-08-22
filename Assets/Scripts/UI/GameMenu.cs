using UnityEngine;
using UnityEngine.UI;

/*
    A monobehavior class.
    Handles the game menu UI elements and provides helper methods.
*/
[SelectionBase]
[DisallowMultipleComponent]
public class GameMenu : MonoBehaviour {
    [Header("UI Elements")]
    [Tooltip("Toggle for vsync.")]
    [SerializeField] private Toggle vsyncToggle;
    [Tooltip("Toggle for PixelBoy.")]
    [SerializeField] private Toggle pixelboyToggle;
    [Tooltip("Slider for PixelBoy resolution.")]
    [SerializeField] private Slider resolutionSlider, sensitivitySlider;
    [Tooltip("Label for the game version.")]
    [SerializeField] private Text versionLabel;

    private GameProperties gameProperties;
    private Camera playerCamera;
    private InputField sensitivityField, resolutionField;

    private void Awake() {
        gameProperties = GameManager.instance.GetComponent<GameProperties>();
        playerCamera = GameManager.instance.player.transform.parent.GetComponentInChildren<Camera>();
        Transform panel = transform.GetChild(0);
        resolutionField = resolutionSlider.GetComponentInChildren<InputField>();
        sensitivityField = sensitivitySlider.GetComponentInChildren<InputField>();
        versionLabel.text = "v" + Application.version;
    }

    // Called when the game menu is opened.
    private void OnEnable() { UpdateMenu(); }

    // Updates menu UI elements when called.
    public void UpdateMenu() {
        // Sensitivity.
        sensitivityField.text = gameProperties.sensitivity.ToString("f1");
        sensitivitySlider.value = gameProperties.sensitivity;
        // Vsync state.
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
        // Pixelboy state.
        pixelboyToggle.isOn = playerCamera.GetComponent<Pixelboy>().enabled;
        // Pixelboy resolution.
        resolutionSlider.value = playerCamera.GetComponent<Pixelboy>().height;
        resolutionField.text = resolutionSlider.value.ToString();
    }

    public void SetSensitivity(float sensitivity) {
        gameProperties.sensitivity = sensitivity;
        UpdateMenu();
    }

    public void SetSensitivity(string sensitivity) {
        gameProperties.sensitivity = float.Parse(sensitivity);
        UpdateMenu();
    }

    public void ToggleVsync(bool isEnabled) {
        QualitySettings.vSyncCount = (isEnabled) ? 1 : 0;
        UpdateMenu();
    }

    public void TogglePixelboy(bool isEnabled) {
        playerCamera.GetComponent<Pixelboy>().enabled = isEnabled;
        UpdateMenu();
    }

    public void SetPixelboyResolution(float resolution) {
        playerCamera.GetComponent<Pixelboy>().height = (int)resolution;
        UpdateMenu();
    }

    public void SetPixelboyResolution(string resolution) {
        playerCamera.GetComponent<Pixelboy>().height = (int)Mathf.Clamp(float.Parse(resolution), resolutionSlider.minValue, resolutionSlider.maxValue);
        UpdateMenu();
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}