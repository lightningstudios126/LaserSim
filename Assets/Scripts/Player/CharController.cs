using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CharController : MonoBehaviour {

    [Header("Movement")]
    public float maxSpeed = 5.0f;
    public float jumpSpeed = 7.0f;
    public float maxDeltaVelocity = 10.0f;
    public float initialSpeed = 0.2f;
    public float acceleration = 10f;
    [Range(0, 1)]
    public float aerialControllability = 0.05f;

    private float moveSpeed;
    private Vector3 inputDirection;
    private bool tryJump;

    new private Rigidbody rigidbody;
    new private CapsuleCollider collider;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        rigidbody.freezeRotation = true;
    }

    private void Update() {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDirection != Vector3.zero) {
            if (moveSpeed < maxSpeed)
                moveSpeed = Mathf.Clamp(moveSpeed + Time.deltaTime * acceleration, initialSpeed, maxSpeed);
        } else {
            moveSpeed = initialSpeed;
        }

        if (Input.GetButtonDown("Jump")) tryJump = true;
    }

    private void FixedUpdate() {
        Vector3 moveDirection = transform.TransformDirection(inputDirection).normalized;
        Vector3 deltaVelocity;

        if (IsGrounded()) {
            deltaVelocity = moveDirection * moveSpeed - rigidbody.velocity;
            deltaVelocity.y = 0;

            if (tryJump) {
                deltaVelocity += transform.up * jumpSpeed;
                //Debug.Log("jump");
            }
        } else {
            deltaVelocity = moveDirection == Vector3.zero
                ? Vector3.zero
                : aerialControllability * (moveDirection * moveSpeed - rigidbody.velocity);
            deltaVelocity.y = 0;
        }

        deltaVelocity.x = Mathf.Clamp(deltaVelocity.x, -maxDeltaVelocity, maxDeltaVelocity);
        deltaVelocity.z = Mathf.Clamp(deltaVelocity.z, -maxDeltaVelocity, maxDeltaVelocity);

        rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
        tryJump = false;
    }

    private bool IsGrounded() {
        return Physics.SphereCast(
            origin: transform.position,
            radius: collider.radius * 0.95f,
            direction: -transform.up,
            hitInfo: out _,
            maxDistance: collider.bounds.extents.y + 0.1f);
    }
}

