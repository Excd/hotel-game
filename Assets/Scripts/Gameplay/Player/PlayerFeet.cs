// TODO: Add layermask to feet collisions to prevent physics exploits like with bouncy ball.
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerFeet : MonoBehaviour {
    private List<Collider> collisionList = new List<Collider>();

    private PlayerManager playerManager;
    private Rigidbody rb;

    private void Awake() {
        playerManager = transform.parent.GetComponent<PlayerManager>();
        rb = transform.parent.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject != transform.parent.gameObject) {
            collisionList.Add(other);
            CheckIsGrounded();
        }
    }

    private void OnTriggerExit(Collider other) {
        collisionList.Remove(other);
        CheckIsGrounded();
    }

    private void CheckIsGrounded() {
        if (collisionList.Count != 0) {
            playerManager.isGrounded = true;
        }
        else {
            playerManager.isGrounded = false;
            rb.drag = 0.0f;
        }

    }

    public void ClearCollisions() {
        foreach (Collider collision in collisionList.ToArray())
            if (collision == null) collisionList.Remove(collision);

        CheckIsGrounded();
    }
}