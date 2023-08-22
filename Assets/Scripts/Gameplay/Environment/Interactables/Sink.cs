using System.Collections;
using UnityEngine;

public class Sink : Interactable {
    [Tooltip("Time in seconds before sink stops running.")]
    [SerializeField] private float runningTime = 5.0f;
    [Tooltip("Water object.")]
    [SerializeField] private GameObject water;

    private bool isRunning = false;

    private IEnumerator runSink;

    public override void Interact() {
        if (!isRunning) {
            ToggleSink();
            StartCoroutine(runSink = RunSink());
        }
        else {
            StopCoroutine(runSink);
            ToggleSink();
        }
    }

    private void ToggleSink() {
        isRunning = !isRunning;
        water.SetActive(!water.activeSelf);
    }

    IEnumerator RunSink() {
        yield return new WaitForSeconds(runningTime);
        ToggleSink();
    }
}