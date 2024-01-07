using UnityEngine;
using Shapes;

[ExecuteAlways] public class MyScript : ImmediateModeShapeDrawer {
    public int numParticles = 1;
    public float radius = 0.1f;
    public override void DrawShapes( Camera cam ){

        using( Draw.Command( cam ) ) {
            
            // set static parameters
            Draw.Matrix = transform.localToWorldMatrix;
            
            // draw particles
            for (var i = 0; i < numParticles; i++) {
                Draw.Sphere(i * Vector3.up, radius, Color.red);
            }
        }

    }

}