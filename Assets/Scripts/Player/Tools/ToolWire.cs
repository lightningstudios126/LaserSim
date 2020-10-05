using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolWire : Tool {
	[Header("Selecting")]
	public float maxSelectRange = 20f;
	public BasicInput selectedInput;
	public BasicOutput selectedOutput;	

	[Header("Line")]
	public Color holdColor = new Color(0, 1f, 1f, 0.5f);
	public float holdWidth = 0.3f;

	public override void Activate() {
		this.toolName = "Wire Tool";
		selectedInput = null;
		selectedOutput = null;

		line.positionCount = 2;
		line.startWidth = holdWidth;
		line.endWidth = holdWidth;
		line.startColor = holdColor;
		line.endColor = holdColor;
	}

	public override void Active() {
		line.enabled = selectedInput != null || selectedOutput != null;
		line.SetPosition(0, cam.transform.position + cam.transform.TransformDirection(new Vector3(0, 0, 2f)));
		if (selectedInput != null) {
			line.SetPosition(1, selectedInput.transform.position);
		} else if (selectedOutput != null) {
			line.SetPosition(1, selectedOutput.transform.position);
		}
	}

	public override void Deactivate() {
        ClearSelected();
		line.enabled = false;
	}

	public override void OnLMouse(bool shift) {
		if (RaycastForComponent<BasicInput>(maxSelectRange, ref selectedInput)) {

		} else if (RaycastForComponent<BasicOutput>(maxSelectRange, ref selectedOutput)) {

		}

		if (selectedInput != null && selectedOutput != null) {
			if (!shift) { // create connection not holding shift, delete connection with shift
				selectedInput.Connect(selectedOutput);
				ClearSelected();
			} else {
				if (selectedInput.input == selectedOutput) {
					selectedInput.Disconnect();
					ClearSelected();
				} else {
					ClearSelected();
				}
			}
		} 
	}

    public override void OnRMouse(bool shift) {
        ClearSelected();
    }

	void ClearSelected() {
		selectedInput = null;
		selectedOutput = null;
	}
}
