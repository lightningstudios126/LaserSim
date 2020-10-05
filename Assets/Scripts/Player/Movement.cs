using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

    Camera cam;
    Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 1;
    public float maxSpeed = 2;
    public float jumpSpeed = 5;
    [Range(0, 1)]
    public float damping = 0.8f;

    [Header("Camera")]
    public float cameraClamp = 90;
    public float pitchSpeed = -5;
    public float yawSpeed = 5;

    private float distToGround;
    private float pitch;

    bool IsGrounded {
        get {
            return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        }
    }

    void Start() {
        SetCamera();

        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update() {

        if (cam == null) SetCamera();

        UpdateCamera();

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        rb.AddRelativeForce(moveDirection * moveSpeed, ForceMode.VelocityChange);

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x * damping, -maxSpeed, maxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z * damping, -maxSpeed, maxSpeed));

        // Jumping Input
        if (Input.GetKey(KeyCode.Space) && IsGrounded) {
            rb.velocity = new Vector3(0, jumpSpeed, 0);
        }

        /*
        // Walk Bobbing and Camera Lerping
        if (Input.GetKey("w") ||
            Input.GetKey("s") ||
            Input.GetKey("a") ||
            Input.GetKey("d")
        ) {
            float yPos = Mathf.Sin(Time.time * 15) / 15;
            cameraObj.transform.localPosition = new Vector3(cameraObj.transform.localPosition.x, yPos + 1.5f, cameraObj.transform.localPosition.z);
        } else {
            cameraObj.transform.localPosition = Vector3.Lerp(cameraObj.transform.localPosition, camStart, 0.3f);
        }
        */

    }

    void OnCollisionEnter(Collision collision) {
        if (!IsGrounded) {
            if (Input.GetAxis("Horizontal") != 0) {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
            if (Input.GetAxis("Vertical") != 0) {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }
        }
    }

    private void UpdateCamera() {
        float dYaw = yawSpeed * Input.GetAxis("Mouse X");
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, dYaw, 0);

        cam.transform.position = transform.position + new Vector3(0, 0.5f, 0);

        pitch += pitchSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -cameraClamp, cameraClamp);
        cam.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, 0);
        transform.GetChild(0).transform.localEulerAngles = new Vector3(pitch + 90, 0, 0);
    }

    private void OnDestroy() {
        Debug.Log("deleting");
        Cursor.lockState = CursorLockMode.None;
        cam.transform.position = new Vector3(0, 1, -10);
    }

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void SetCamera() {
        cam = Camera.main;
    }
}
