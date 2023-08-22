// TODO: Add player dropped/spawned items to the same empty parent container.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class PlayerInventory : MonoBehaviour {
    private const int inventoryLimit = 16;

    private bool isInventoryFull = false;
    private List<GameObject> inventory = new List<GameObject>(inventoryLimit);

    public bool PickupItem(GameObject item) {
        if (!isInventoryFull) {
            GameObject itemParent = item.transform.parent.gameObject;

            item.GetComponent<Item>().isHeld = true;
            itemParent.transform.SetParent(transform, false);
            itemParent.transform.localPosition = Vector3.zero;
            itemParent.transform.localEulerAngles = Vector3.zero;
            itemParent.SetActive(false);
            inventory.Add(itemParent);

            if (item.GetComponent<ItemDrop>() != null) {
                Destroy(item.GetComponent<ItemDrop>());
                Destroy(item.GetComponent<Rigidbody>());
            }

            if (inventory.Count == inventoryLimit) isInventoryFull = true;

            return false;
        }

        return true;
    }

    public void DropItem(GameObject item) {
        if (inventory.Contains(item)) {
            inventory.Remove(item);
            item.SetActive(true);
            item.GetComponentInChildren<Item>().isHeld = false;
            item.transform.SetParent(null);
            item.transform.localScale = Vector3.one;

            if (item.scene.name == "DontDestroyOnLoad")
                SceneManager.MoveGameObjectToScene(item, SceneManager.GetActiveScene());

            if (isInventoryFull) isInventoryFull = false;
        }

        item.AddComponent<ItemDrop>();
    }

    public List<GameObject> GetInventory() {
        return inventory;
    }

    public override string ToString() {
        string list = "Inventory:";

        foreach (GameObject item in inventory) {
            list += "\n -" + item.GetComponentInChildren<Item>().displayName + " : " + item.ToString();
        }

        return list;
    }
}