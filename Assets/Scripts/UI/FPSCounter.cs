using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
    A monobehavior class.
    When applied to a text UI element calculates and displays the current frames per second value.
*/
[SelectionBase]
[DisallowMultipleComponent]
public class FPSCounter : MonoBehaviour {
    private int frameCount;
    private Text display;

    private void Awake() {
        display = GetComponent<Text>(); // Store text component.
        StartCoroutine(UpdateFPS());
    }

    // Calculate and display current frames per second every 100ms.
    private IEnumerator UpdateFPS() {
        while (true) {
            frameCount = (int)(1.0f / Time.unscaledDeltaTime);
            display.text = "FPS " + frameCount.ToString(); // Update display text.

            yield return new WaitForSeconds(0.1f);
        }
    }
}