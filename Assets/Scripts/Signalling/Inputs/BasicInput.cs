using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class BasicInput : MonoBehaviour {

    public BasicOutput input;

    LineRenderer signalLine;

    public void Start() {
        signalLine = GetComponent<LineRenderer>();
        signalLine.positionCount = 2;

        signalLine.startWidth = 0.2f;
        signalLine.endWidth = 0.2f;
    }

    public virtual void Activate() { }
    public virtual void Deactivate() { }

    // Update is called once per frame
    void Update() {
        if (input != null) {
            Activate();
        }
    }

    private void LateUpdate() {
        signalLine.enabled = input != null;
        if (input != null) {
            if (input.state) {
                DrawLine(input.transform.position, new Color(3 / 255f, 198 / 255f, 252 / 255f));
            } else {
                DrawLine(input.transform.position, new Color(232 / 255f, 51 / 255f, 23 / 255f));
            }
        }
    }

    private void DrawLine(Vector3 endpoint, Color color) {
        signalLine.startColor = color;
        signalLine.endColor = color;
        signalLine.SetPosition(0, transform.position);
        signalLine.SetPosition(1, endpoint);
    }

    public void Connect(BasicOutput input) {
        this.input = input;
    }

    public void Disconnect() {
        this.input = null;
        Deactivate();
        signalLine.enabled = false;
    }
}
