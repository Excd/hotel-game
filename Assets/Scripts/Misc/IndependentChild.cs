using UnityEngine;

[DisallowMultipleComponent]
public class IndependentChild : MonoBehaviour {
    protected Vector3 startPosition, positionOffset;

    public Vector3 GetTargetPosition(Transform target) {
        return new Vector3(
            target.localPosition.x + positionOffset.x,
            target.localPosition.y + (target.localScale.y * positionOffset.y),
            target.localPosition.z + positionOffset.z
        );
    }
}