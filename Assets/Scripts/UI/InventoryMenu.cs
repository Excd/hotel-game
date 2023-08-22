using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public class InventoryMenu : MonoBehaviour {
    private PlayerInventory inventoryObject;
    private List<GameObject> inventory;

    private void Start() {
        inventoryObject = GameManager.instance.player.GetComponentInChildren<PlayerInventory>();
        inventory = inventoryObject.GetInventory();
    }

    // Called when the inventory is opened.
    // Calls LateEnable to wait one frame before updating inventory.
    private void OnEnable() { StartCoroutine(LateEnable()); }

    private IEnumerator LateEnable() {
        yield return null;  // Wait one frame.
        UpdateInventory();
    }

    private void UpdateInventory() {
        int itemCount = 0;

        foreach (Transform child in transform)
            ClearInventorySlot(child.GetComponent<Image>());

        foreach (GameObject item in inventory) {
            Image inventorySlot = transform.GetChild(itemCount).GetComponent<Image>();
            inventorySlot.sprite = item.GetComponentInChildren<Item>().thumbnail;
            inventorySlot.color = Color.white;
            inventorySlot.GetComponent<Button>().interactable = true;
            itemCount++;
        }
    }

    private void ClearInventorySlot(Image inventorySlot) {
        inventorySlot.GetComponent<Button>().interactable = false;
        inventorySlot.sprite = null;
        inventorySlot.color = Color.black;
    }

    public void DropItem(GameObject itemBox) {
        inventoryObject.DropItem(inventory[itemBox.transform.GetSiblingIndex()]);
        UpdateInventory();
    }
}