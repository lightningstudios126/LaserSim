using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : BasicInput {
    public new Light light;

    private new void Start() {
        base.Start();
        light = light ?? GetComponent<Light>();
    }

    public override void Activate() {
        light.enabled = (input?.state).HasValue && (input?.state).Value;
    }
}