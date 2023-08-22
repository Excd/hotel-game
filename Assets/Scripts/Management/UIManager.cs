using UnityEngine;

[DisallowMultipleComponent]
public class UIManager : MonoBehaviour {
    [Header("UI Objects")]
    public GameObject gameMenu;
    public GameObject inventoryMenu;
    public GameObject console;
    public GameObject dialogueBox;

    private PlayerManager playerManager;
    private PlayerActions playerActions;

    protected void Awake() {
        DontDestroyOnLoad(gameObject);
        playerManager = GameManager.instance.player.GetComponent<PlayerManager>();
        playerActions = GameManager.instance.player.GetComponent<PlayerActions>();
    }

    private void Update() {
        if (!console.activeSelf && !gameMenu.activeSelf)
            if (Input.GetButtonDown("Inventory")) ToggleInventory();

        if (Input.GetButtonDown("Cancel")) ToggleGameMenu();
        if (Input.GetButtonDown("Console")) ToggleConsole();
    }

    public void Initialize() {
        gameMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        console.SetActive(false);
        LockCursor();
    }

    public void ToggleCursor() {
        if (Cursor.lockState == CursorLockMode.Locked) UnlockCursor();
        else if (!inventoryMenu.activeSelf && !gameMenu.activeSelf && !console.activeSelf) LockCursor();
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerManager.isInputActive = true;
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerManager.isInputActive) {
            playerActions.ResetInput();
            playerManager.isInputActive = false;
        }
    }

    public void ToggleGameMenu() {
        if (!inventoryMenu.activeSelf) {
            gameMenu.SetActive(!gameMenu.activeSelf);
            Time.timeScale = gameMenu.activeSelf ? 0.0f : 1.0f;
        }
        else {
            inventoryMenu.SetActive(false);
        }

        ToggleCursor();
    }

    public void ToggleInventory() {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
        ToggleCursor();
    }

    public void ToggleConsole() {
        console.SetActive(!console.activeSelf);
        ToggleCursor();
    }
}