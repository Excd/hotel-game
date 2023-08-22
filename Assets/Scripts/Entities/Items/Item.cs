using System;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class Item : Interactable {
    public String displayName;
    public Sprite thumbnail;

    [NonSerialized] public bool isHeld = false;

    public override void Interact() {
        if (!isHeld) {
            bool isInventoryFull = GameManager.instance.player.GetComponentInChildren<PlayerInventory>().PickupItem(gameObject);

            if (!isInventoryFull) {
                isHeld = true;
            }
            else {
                string message = "Inventory full!";
                GameManager.instance.userInterface.GetComponent<UIManager>().console.GetComponent<Console>().Print(message);
                Debug.Log(message);
            }
        }
    }
}