using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class Interactable : MonoBehaviour {
    public abstract void Interact();
}