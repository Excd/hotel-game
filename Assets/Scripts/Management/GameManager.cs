using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager> {
    protected GameManager() { }

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject userInterfacePrefab;
    [Tooltip("Global list of spawnable item prefabs.")]
    public List<GameObject> items;

    public GameObject player { get; set; }          // Player reference.
    public GameObject userInterface { get; set; }   // UI reference.

    private Transform spawnPoint; // Current scene respawn point reference.

    protected override void Awake() {
        base.Awake();

        if (GameManager.instance.gameObject != gameObject) {
            GameManager.instance.Initialize();

            return;
        }

        Initialize();
    }

    public void Initialize() {
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            player = Instantiate(playerPrefab).transform.Find("Player").gameObject;

        if (userInterface == null)
            userInterface = Instantiate(userInterfacePrefab);

        player.GetComponent<PlayerManager>().Initialize(spawnPoint);
        userInterface.GetComponent<UIManager>().Initialize();
    }
}