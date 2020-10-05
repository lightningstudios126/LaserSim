using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LogicGate2 : BasicOutput {
    public enum GateType {
        AND, OR, XOR
    }

    public GateType gate;
    public bool invert;

    public BasicInput input1;
    public BasicInput input2;

    public override bool UpdateState() {
        if (input1.input != null && input2.input != null) {
            switch (gate) {
                case GateType.AND:
                    return invert != (input1.input.state && input2.input.state);
                case GateType.OR:
                    return invert != (input1.input.state || input2.input.state);
                case GateType.XOR:
                    return invert != (input1.input.state != input2.input.state);
                default:
                    return false;
            }
        } else {
            return false;
        }
    }
}
