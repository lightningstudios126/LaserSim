using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Interactable : MonoBehaviour {

    [Header("Physics")]
    public float heldDamp = 0.8f;
    public float density = 1f;
    public bool normallyHasGravity = true;
    public bool normallyFrozenRotation = false;

    public bool frozen = false;

    public Rigidbody attachedRigidBody;

    Rigidbody selfRigidBody;

    private void Awake() {
        selfRigidBody = GetComponent<Rigidbody>();
        attachedRigidBody = selfRigidBody;
    }

    private void Start() {
        Interactable parent = transform.parent?.gameObject?.GetComponent<Interactable>();
        if (parent != null) {
            if (parent.attachedRigidBody != null) attachedRigidBody = parent.attachedRigidBody;
        }
    }

    public void Hold() {
        attachedRigidBody.useGravity = false;
        //attachedRigidBody.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //attachedRigidBody.freezeRotation = true;
        GetComponentsInChildren<Interactable>().ToList().ForEach(x => x.attachedRigidBody.useGravity = false);
        Unfreeze();
    }

    public void Unhold() {
        attachedRigidBody.useGravity = normallyHasGravity;
        //attachedRigidBody.freezeRotation = normallyFrozenRotation;
        GetComponentsInChildren<Interactable>().ToList().ForEach(x => x.attachedRigidBody.useGravity = x.normallyHasGravity);
    }

    public void Freeze() {
        frozen = true;
        attachedRigidBody.useGravity = false;
        attachedRigidBody.isKinematic = true;
    }

    public void Unfreeze() {
        frozen = false;
        attachedRigidBody.useGravity = normallyHasGravity;
        attachedRigidBody.isKinematic = false;
    }

    public void ChangeScale(Vector3 newScale) {
        transform.localScale = newScale;
        Debug.Log(newScale.x * newScale.y * newScale.z * density);
        attachedRigidBody.mass = (newScale.x * newScale.y * newScale.z) * density;
    }

    public void LateUpdate() {
        if (transform.position.y < -5) Destroy(gameObject);
        //if (selfRigidBody != attachedRigidBody) transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}