using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : BasicOutput {

    // constants calculated with testing and graphing with desmos
    /* 1:100
	 * 2:150
	 * 3:210
	 * 4:265
	 * 5:320
	 */
    private const float slope = 55.5f;
    private const float intercept = 42.5f;

    public float targetMass = 1;

    GameObject button;
    SpringJoint spring;

    // Use this for initialization
    void Start() {
        button = transform.GetChild(0).gameObject;
        spring = button.GetComponent<SpringJoint>();
        spring.connectedAnchor = transform.position;

        spring.spring = (slope * targetMass) + intercept; // aaay linear equations

        Rigidbody buttonRB = button.GetComponent<Rigidbody>();
        buttonRB.constraints = RigidbodyConstraints.FreezeRotation;
        if (Mathf.Abs(button.transform.up.x) > 0.5) {
            buttonRB.constraints |= RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            spring.connectedAnchor += new Vector3(-0.05f, 0, 0);
        } else if (Mathf.Abs(button.transform.up.y) > 0.5) {
            buttonRB.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            spring.connectedAnchor += new Vector3(0, -0.05f, 0);
        } else if (Mathf.Abs(button.transform.up.z) > 0.5) {
            buttonRB.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            spring.connectedAnchor += new Vector3(0, 0, -0.05f);
        }

        Physics.IgnoreCollision(button.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public override bool UpdateState() {
        return button.transform.localPosition.y < 0.4;
    }

}
