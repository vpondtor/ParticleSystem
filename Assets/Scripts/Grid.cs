using System;
using UnityEngine;
using Shapes;

[ExecuteAlways]
public class Grid : ImmediateModeShapeDrawer {
    public int n = 1;
    public float radius = 0.1f;
    public Color color = Color.white;

    public override void DrawShapes(Camera cam) {
        n = Math.Clamp(n, 0, 25);
        using (Draw.Command(cam)) {

            // set static parameters
            Draw.Matrix = transform.localToWorldMatrix;
            var origin = transform.position;

            // draw particles
            for (var x = 0; x < n; x++) {
                for (var y = 0; y < n; y++) {
                    for (var z = 0; z < n; z++) {
                        var current = origin + new Vector3(x, y, z);
                        Draw.Sphere(current, radius, color);
                    }
                }
            }
        }
    }
}
