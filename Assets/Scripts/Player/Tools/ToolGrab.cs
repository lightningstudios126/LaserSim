using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tools {
    class ToolGrab : Tool {
        [Header("Selecting")]
        public float maxSelectRange = 20f;

        [Header("Holding")]
        public Interactable heldInteract;
        public float defaultDistance = 3f;
        public float maxHeldRange = 5f;
        public float scrollSpeed = 1f;
        public float holdMagnitude = 100;
        public float ejectMagnitude = 20;

        [Header("Creating")]
        public GameObject createdObject;
        public Transform creationParent;
        public float minimumSize = 0.5f;
        public float maximumSize = 3;
        public float sizeIncrement = 0.5f;

        [Header("Line")]
        public Color holdColor = new Color(0, 1f, 0, 0.5f);
        public float holdWidth = 0.3f;

        Quaternion originalRotation;
        Quaternion startingCamRotation;

        Vector3 targetPos;
        float targetDistance = 2;

        public override void Activate() {
            this.toolName = "Grab Tool";
            line.positionCount = 2;
            heldInteract = null;
            line.startWidth = holdWidth;
            line.endWidth = holdWidth;
            line.startColor = holdColor;
            line.endColor = holdColor;
        }

        public override void Deactivate() {
            if (heldInteract != null) heldInteract.Unhold();
            targetDistance = 2;
            line.enabled = false;
            heldInteract = null;
        }

        public override void Active() {
            if (heldInteract != null) {
                line.enabled = true;

                line.SetPosition(0, cam.transform.position + cam.transform.TransformDirection(new Vector3(0, -0.3f, -0.5f)));
                line.SetPosition(1, heldInteract.transform.position);
            } else {
                line.enabled = false;
            }
        }

        public override void FixedActive() {
            // if holding an interactable, pull the interactable to the target location
            if (heldInteract != null) {
                Rigidbody heldRB = heldInteract.attachedRigidBody;

                // quaternion of y-offset
                Quaternion inter = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y - startingCamRotation.eulerAngles.y, Vector3.up);
                // rotate current rotation to the original rotation + the y-offset
                heldRB.angularVelocity *= heldInteract.heldDamp;
                heldRB.MoveRotation(Quaternion.Slerp(heldRB.rotation, inter * originalRotation, 0.2f));

                heldRB.velocity *= heldInteract.heldDamp;
                heldRB.AddForce((targetPos - heldRB.position) * holdMagnitude, ForceMode.Acceleration);
            }
        }

        public override void OnLMouse(bool shift) {
            // if there is no held interactable
            if (heldInteract == null) {
                // raycast for an interactable and save the results, and make sure it exists first
                if (RaycastForInteractable(maxSelectRange, ref heldInteract)) {
                    heldInteract.Hold();
                    originalRotation = heldInteract.attachedRigidBody.rotation;
                    startingCamRotation = cam.transform.rotation;
                    targetDistance = Vector3.Distance(heldInteract.attachedRigidBody.transform.position, transform.position);
                    Feedback("Grabbed " + heldInteract.gameObject.name);
                }
            } else {// otherwise, unhold the interactable
                Drop();
                Feedback("Dropped held interactable");
            }
        }

        public override void OnMMouse(bool shift) {
            if (heldInteract != null) { // eject held interactable!
                if (!shift) {
                    Vector3 ejectVector = (targetPos - transform.position).normalized * ejectMagnitude;
                    heldInteract.attachedRigidBody.AddForce(ejectVector, ForceMode.VelocityChange);

                    heldInteract.Unhold();
                    heldInteract = null;
                    targetDistance = defaultDistance;
                    Feedback("Ejected held interactable");
                } else {
                    heldInteract.Freeze();
                    Drop();
                }
            }
        }

        public override void OnRMouse(bool shift) {
            if (heldInteract == null) {
                // create new interactable and set its size to some random scale
                GameObject created = Instantiate(createdObject, targetPos, Quaternion.identity, creationParent);
                Vector3 scale = new Vector3(RandomSize(), RandomSize(), RandomSize());
                Interactable interact = created.GetComponent<Interactable>();

                interact.ChangeScale(scale);
                Feedback(string.Format("Created cuboid with size ({0}, {1}, {2}) and mass {3}", scale.x, scale.y, scale.z, interact.attachedRigidBody.mass.ToString()));
            } else {
                // delete held interactable
                heldInteract.Unhold();
                Destroy(heldInteract.gameObject);
                targetDistance = defaultDistance;
                heldInteract = null;
                Feedback("Destroyed held interactable");
            }
        }

        public override void OnScroll(float scroll) {
            // when holding an interactable, update the current target position with the current scroll
            if (heldInteract != null) targetDistance = UpdateTargetDistance(scroll);
            targetPos = cam.ScreenToWorldPoint(cam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)) + new Vector3(0, 0, targetDistance));
        }

        private float UpdateTargetDistance(float scroll) {
            return Mathf.Clamp((scroll * scrollSpeed) + targetDistance, 0.5f, maxHeldRange);
        }

        public float RandomSize() {
            int smallest = Mathf.CeilToInt(minimumSize / sizeIncrement);
            int largest = Mathf.CeilToInt(maximumSize / sizeIncrement);
            return UnityEngine.Random.Range(smallest, largest) * sizeIncrement;
        }

        public void Drop() {
            heldInteract.Unhold();
            targetDistance = defaultDistance;
            heldInteract = null;
        }
    }
}
