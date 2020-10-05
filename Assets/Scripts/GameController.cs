using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        DisableCursor();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) EnableCursor();
	}

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            StartCoroutine(DisableCursor());
        } else {
            StartCoroutine(EnableCursor());
        }
    }

    IEnumerator DisableCursor() {
        yield return new WaitForEndOfFrame();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    IEnumerator EnableCursor() {
        yield return new WaitForEndOfFrame();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
