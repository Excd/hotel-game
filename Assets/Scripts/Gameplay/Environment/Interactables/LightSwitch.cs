using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable {
    [Tooltip("Transform of the light switch.")]
    [SerializeField] private Transform lightSwitch;

    [Header("Bulb Settings")]
    [Tooltip("Alternative bulb material to use when lights are toggled.")]
    [SerializeField] private Material altMaterial;
    [Tooltip("Light bulb GameObjects attached to this switch. Ensure any child light sources are NOT static!")]
    [SerializeField] private List<GameObject> bulbs;

    // Light switch position and rotation vectors.
    private readonly Vector3 switchUpPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private readonly Vector3 switchUpRotation = new Vector3(90.0f, 0.0f, 0.0f);
    private readonly Vector3 switchDownPosition = new Vector3(0.0f, 0.02f, 0.0f);
    private readonly Vector3 switchDownRotation = new Vector3(60.0f, 90.0f, 90.0f);

    private bool isSwitched = false;
    // List to hold starting material for each bulb.
    private List<Material> bulbMaterials = new List<Material>();

    private void Start() {
        // Populate list of initial bulb materials.
        foreach (GameObject bulb in bulbs)
            bulbMaterials.Add(bulb.GetComponent<Renderer>().material);
    }

    public override void Interact() {
        // Toggle each bulb and light source.
        for (int i = 0; i < bulbs.Count; i++)
            ToggleLight(bulbs[i], i);

        isSwitched = (isSwitched) ? false : true;
    }

    private void ToggleLight(GameObject bulb, int index) {
        // Cache current bulb renderer and all light sources.
        Renderer bulbRenderer = bulb.GetComponent<Renderer>();
        List<Light> lights = new List<Light>(bulb.GetComponentsInChildren<Light>());

        // Toggle light switch position.
        if (!isSwitched) {
            lightSwitch.localPosition = switchDownPosition;
            lightSwitch.localEulerAngles = switchDownRotation;
        }
        else {
            lightSwitch.localPosition = switchUpPosition;
            lightSwitch.localEulerAngles = switchUpRotation;
        }

        // Toggle bulb material.
        bulbRenderer.material = (bulbRenderer.material == bulbMaterials[index])
            ? altMaterial
            : bulbMaterials[index];

        // Toggle light sources.
        foreach (Light light in lights)
            light.enabled = (light.enabled) ? false : true;
    }

    private void OnDrawGizmosSelected() {
        if (bulbs[0] != null) {
            Gizmos.color = Color.yellow;

            //Draw a line between this transform and each bulb.
            foreach (GameObject bulb in bulbs)
                Gizmos.DrawLine(transform.position, bulb.transform.position);
        }
    }
}