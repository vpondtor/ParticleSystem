using System;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using Random = UnityEngine.Random;

// TODO: Can this be monobehavior too since it has an update function?
public class Particle {
    public Vector3 position; 
    public Vector3 velocity;
    public float radius;
    public Color color;
    public int timeToLive;

    public Particle(Vector3 position, Vector3 velocity, float radius, Color color, int timeToLive) {
        // Position relative to parent
        this.position = position;
        this.velocity = velocity;
        this.radius = radius;
        this.color = color;
        this.timeToLive = timeToLive;
    }

    public void Update() {
        position += velocity;
        
        timeToLive--;
    }

    public void Draw() {
        if (timeToLive > 0) {
            Shapes.Draw.Sphere(position, radius, color);
        }
    }
}

public class ParticleSystem : ImmediateModeShapeDrawer
{
    public Vector3 initialVelocity;
    public int coneAngle;
    public float radius = 0.1f;
    public Color color = Color.white;
    public float frequency = 0.5f; // 0.5 particles / second
    public float gravity = 0.5f;
    public float gradient = 0.1f;
    public int timeToLive = 100;
    
    private List<Particle> m_particles = new List<Particle>();
    private System.Random m_random = new System.Random();
    private Gradient m_gradient = new Gradient();
    
    // Start is called before the first frame update
    private void Start() {
        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(Color.white, 0.0f);
        colors[1] = new GradientColorKey(Color.yellow, 0.5f);
        colors[2] = new GradientColorKey(Color.red, 1.0f);
        
        var alphas = new GradientAlphaKey[3];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 0.5f);
        alphas[2] = new GradientAlphaKey(1.0f, 1.0f);
        
        m_gradient.SetKeys(colors, alphas);
    }

    private void Update() {
        radius = Math.Max(0, radius);
        gravity = Math.Max(0, gravity);
        gradient = Math.Max(0, gradient);
        foreach (var particle in m_particles) {
            particle.velocity -= new Vector3(0, gravity * Time.deltaTime, 0);
            particle.color = m_gradient.Evaluate(Math.Clamp(particle.velocity.magnitude, 0, gradient) / gradient);
        }
        
        if (Input.GetKey(KeyCode.UpArrow)) {
            coneAngle++;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            coneAngle--;
        }

        coneAngle = Math.Clamp(coneAngle, 0, 180);
        frequency = Math.Clamp(frequency, 0, 1);
        if (Input.GetKey(KeyCode.Space)) {
            var roll = m_random.NextDouble();
            if (roll <= frequency) {
                GenerateParticle();
            }
        }
    }

    private void GenerateParticle() {
        var zAngle = m_random.Next(-coneAngle, coneAngle);
        var yAngle = m_random.Next(-180, 180);
        
        var velocity = Quaternion.AngleAxis(zAngle, transform.forward) * initialVelocity;
        velocity = Quaternion.AngleAxis(yAngle, transform.up) * velocity;
        
        m_particles.Add(new Particle(transform.position, velocity, radius, color, timeToLive));
    }

    public override void DrawShapes( Camera cam ){
        using( Draw.Command( cam ) ) {
            // set static parameters
            Draw.Matrix = transform.localToWorldMatrix;
            
            foreach (var particle in m_particles) {
                particle.Draw();
                particle.Update();
            }
        }

    }
}
