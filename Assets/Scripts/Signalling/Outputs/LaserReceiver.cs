using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : BasicOutput {
    bool hitByBeam;

    public override bool UpdateState() {
        return hitByBeam;
    }

    private void LateUpdate() {
        hitByBeam = false;
    }

    public void BeamHit() {
        hitByBeam = true;
        state = hitByBeam;
    }
}
