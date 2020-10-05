using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolController : MonoBehaviour {

    public Camera cam;
    public Text feedback;
    public LineRenderer line;

    public List<Tool> tools;
    int toolIndex = 0;
    Tool selectedTool;


    // Use this for initialization
    void Start() {
        cam = Camera.main;
        foreach (Tool tool in tools) {
            tool.cam = cam;
            tool.feedback = feedback;
            tool.line = line;
        }

        ChangeTool(0);
    }

    // Update is called once per frame
    void Update() {
        bool shift = Input.GetKey(KeyCode.LeftShift);
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (shift && scroll != 0) {
            if (scroll > 0) IncrementTool();
            else DecrementTool();
        } else selectedTool.OnScroll(scroll);

        if (Input.GetKeyDown(KeyCode.Q)) DecrementTool();
        if (Input.GetKeyDown(KeyCode.E)) IncrementTool();

        if (Input.GetButtonDown("Fire1")) {
            selectedTool.OnLMouse(shift);
        }

        if (Input.GetButtonDown("Fire2")) {
            selectedTool.OnRMouse(shift);
        }

        if (Input.GetButtonDown("Fire3")) {
            selectedTool.OnMMouse(shift);
        }

        selectedTool.Active();
    }

    void FixedUpdate() {
        selectedTool.FixedActive();
    }

    void IncrementTool() { ChangeTool(toolIndex + 1); }

    void DecrementTool() { ChangeTool(toolIndex - 1); }

    void ChangeTool(int newIndex) {
        if (newIndex < 0) toolIndex = tools.Count - 1;
        else if (newIndex > tools.Count - 1) toolIndex = 0;
        else toolIndex = newIndex;

        if (selectedTool != null) selectedTool.Deactivate();
        selectedTool = (tools[toolIndex]);
        selectedTool.Activate();
        selectedTool.Feedback("Switched to " + selectedTool.toolName);
    }
}
