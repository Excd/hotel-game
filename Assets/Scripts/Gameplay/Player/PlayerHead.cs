using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHead : MonoBehaviour {
    [Tooltip("Layers to include in headroom detection.")]
    [SerializeField] private LayerMask layerMask;

    private List<Collider> collisionList = new List<Collider>();

    private float heightOffset;
    private PlayerManager playerManager;

    private void Awake() {
        playerManager = transform.parent.GetComponent<PlayerManager>();
        heightOffset = transform.localPosition.y;
    }

    private void OnTriggerEnter(Collider other) {
        if (!playerManager.isStanding && ((1 << other.gameObject.layer) & layerMask.value) > 0 && !collisionList.Contains(other)) {
            collisionList.Add(other);
            CheckHasHeadroom();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (((1 << other.gameObject.layer) & layerMask.value) > 0 && collisionList.Contains(other)) {
            collisionList.Remove(other);
            CheckHasHeadroom();
        }
    }

    private void CheckHasHeadroom() {
        if (collisionList.Count != 0) playerManager.hasHeadroom = false;
        else playerManager.hasHeadroom = true;
    }

    public void UpdatePosition() {
        transform.localPosition = Vector3.up * (heightOffset + (-transform.parent.localScale.y + playerManager.height));
    }

    public void ClearCollisions() {
        foreach (Collider collision in collisionList.ToArray())
            if (collision == null) collisionList.Remove(collision);

        CheckHasHeadroom();
    }
}