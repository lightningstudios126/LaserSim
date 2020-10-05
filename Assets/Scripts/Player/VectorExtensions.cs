using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions {
    [System.Flags]
    public enum VectorComponent {
        X = 1, Y = 2, Z = 4
    }

    public static Vector3 ExtractComponents(this Vector3 source, VectorComponent component) {
        return new Vector3(
            component.HasFlag(VectorComponent.X) ? source[0] : 0, 
            component.HasFlag(VectorComponent.Y) ? source[1] : 0, 
            component.HasFlag(VectorComponent.Z) ? source[2] : 0);
    }
}
