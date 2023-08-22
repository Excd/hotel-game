using UnityEngine;

[DisallowMultipleComponent]
public class ItemDrop : MonoBehaviour {
    private const float dropForce = 3.5f;

    private Rigidbody itemRigidbody;

    private void Awake() {
        itemRigidbody = gameObject.AddComponent<Rigidbody>();
        itemRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        itemRigidbody.AddRelativeForce(Vector3.forward * dropForce, ForceMode.Impulse);
        Destroy(this);
    }
}