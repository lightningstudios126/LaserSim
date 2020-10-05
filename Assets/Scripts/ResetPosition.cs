using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -5) {
            transform.position = new Vector3(0, 2, 0);
            if (TryGetComponent<Rigidbody>(out var rb)) {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
