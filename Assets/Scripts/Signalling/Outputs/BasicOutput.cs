using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicOutput : MonoBehaviour {

    public bool state;

    public virtual bool UpdateState() {
        return false;
    }

    private void Update() {
        state = UpdateState();
    }
}
