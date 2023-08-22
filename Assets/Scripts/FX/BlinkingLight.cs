// TODO: Create different selectable light events (flicker and fade).
//          -- Flicker or fade between (2 or more) colors or just on/off.
// TODO: Rename script to AnimatedLight.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class BlinkingLight : MonoBehaviour {
    [System.Serializable]
    private struct BlinkEvent {
        [Tooltip("Minimum time in seconds before light toggle.")]
        public float minSeconds;
        [Tooltip("Maximum time in seconds before light toggle. Ignored if value is less or equal to Min Seconds.")]
        public float maxSeconds;
        [Tooltip("Number of times the event should repeat.")]
        public int repetitions;
    }

    [Tooltip("Bulb material to use when light is off.")]
    [SerializeField] private Material offMaterial;
    [Tooltip("Renderer of the bulb.")]
    [SerializeField] private Renderer bulbRenderer;

    [Header("Customize Blinking")]
    [Tooltip("Whether the light starts on or off.")]
    [SerializeField] private bool startOn = true;
    [Tooltip("Custom sequence of light blink events.")]
    [SerializeField] private List<BlinkEvent> eventSequence;

    private Material bulbMaterial;
    private Light lightComponent;

    private void Awake() {
        bulbMaterial = bulbRenderer.material;
        lightComponent = bulbRenderer.GetComponentInChildren<Light>();

        if (lightComponent.enabled != startOn) ToggleLight();

        StartCoroutine(Blinking());
    }

    // Coroutine for animating a blinking light.
    IEnumerator Blinking() {
        float time;

        while (true) {
            foreach (BlinkEvent blinkEvent in eventSequence) {
                for (int i = 0; i <= blinkEvent.repetitions; i++) {
                    // Calculate time and wait before next light toggle.
                    time = (blinkEvent.maxSeconds > blinkEvent.minSeconds)
                        ? Random.Range(blinkEvent.minSeconds, blinkEvent.maxSeconds)
                        : blinkEvent.minSeconds;

                    yield return new WaitForSeconds(time);
                    ToggleLight();
                }
            }
        }
    }

    // Toggle light and associated emissive material.
    private void ToggleLight() {
        bulbRenderer.material = (bulbRenderer.material == bulbMaterial) ? offMaterial : bulbMaterial;
        lightComponent.enabled = (lightComponent.enabled) ? false : true;
    }
}