// TODO: Slight camera bump for left/right movement.
using UnityEngine;

[DisallowMultipleComponent]
public class ViewAnimation : MonoBehaviour {
    private PlayerManager playerManager;
    private Rigidbody playerRigidbody;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        playerManager = GameManager.instance.player.GetComponent<PlayerManager>();
        playerRigidbody = GameManager.instance.player.GetComponent<Rigidbody>();
    }

    private void Update() {
        if (playerRigidbody.velocity.magnitude > 0.1f && playerManager.isGrounded)
            animator.SetBool("Moving", true);
        else animator.SetBool("Moving", false);

        animator.speed = playerRigidbody.velocity.magnitude > 0.1f
            ? playerRigidbody.velocity.magnitude / playerManager.maxSpeed
            : 1.0f;
        animator.SetFloat("BobMultiplier", playerManager.maxSpeed / 2.5f);
    }
}