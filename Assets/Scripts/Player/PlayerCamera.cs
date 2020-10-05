using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public float yawSpeed = 5f;
    public float pitchSpeed = 5f;
    public float cameraClamp = 89f;
    public float cameraHeight = 0.5f;

    private float pitch;

    private Camera cam;

    private void Awake() {
        cam = Camera.main;
        cam.transform.parent = transform;
    }

    void Update() {
        float dYaw = yawSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, dYaw, 0), Space.Self);

        cam.transform.localPosition = Vector3.up * cameraHeight;

        pitch += pitchSpeed * -Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -cameraClamp, cameraClamp);
        cam.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }
}
