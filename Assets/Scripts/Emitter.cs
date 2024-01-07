using System;
using UnityEngine;
using Shapes;
using Random = System.Random;

public struct Particle {
    public Vector3 position;
    public Vector3 velocity;
    public float radius;
    public Color color;
    public int timeToLive;
}

public class Emitter : ImmediateModeShapeDrawer {
    public int totalParticles;
    public int particlesPerUpdate;
    public int coneAngle;
    
    // Initial parameters can be stochastic
    public float initialSpeed;
    public Vector3 initialDirection;
    public float initialRadius;
    public Color initialColor;
    public int initialTimeToLive;
    
    private Particle[] m_particlePool;
    private int m_numLiveParticles;
    private readonly Random m_random = new Random();

    // TODO: Refactor this and put it in its own script
    private readonly Gradient m_gradient = new Gradient();

    public void ValidateParameters() {
        totalParticles = Math.Max(totalParticles, 0);
        particlesPerUpdate = Math.Max(particlesPerUpdate, 0);
        coneAngle = Math.Clamp(coneAngle, 0, 180);
        initialSpeed = Math.Max(initialSpeed, 0);
        initialRadius = Math.Max(initialRadius, 0);
        
    }
    
    public void Start() {
        // TODO: Refactor this and put it in its own script (These are reversed because time1 is actually the birth of a particle)
        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(Color.red, 0.5f);
        colors[1] = new GradientColorKey(Color.yellow, 1.0f);

        var alphas = new GradientAlphaKey[3];
        alphas[0] = new GradientAlphaKey(0.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        m_gradient.SetKeys(colors, alphas);
        
        m_particlePool = new Particle[totalParticles];
        for (var i = 0; i < m_particlePool.Length; i++) {
            m_particlePool[i] = new Particle() {
                position = transform.position,
                velocity = Vector3.zero,
                radius = 1,
                color = Color.white,
                timeToLive = 100
            };
        }
    }
    
    public void Update() {
        var deltaSeconds = Time.deltaTime;
        ValidateParameters();
        
        for (var i = 0; i < m_numLiveParticles; i++) {
            UpdateParticle(i, deltaSeconds);
        }
        
        // Kill particles in reverse order to avoid swapping with dead particle.
        for (var i = m_numLiveParticles - 1; i >= 0; i--) {
            KillParticle(i);
        }
        
        EmitParticles(particlesPerUpdate);
    }
    
    public void UpdateParticle(int particleId, float deltaSeconds) {
        // TODO: Refactor this and put it in its own script
        m_particlePool[particleId].color =
            m_gradient.Evaluate((float) m_particlePool[particleId].timeToLive / initialTimeToLive);
        
        m_particlePool[particleId].position += m_particlePool[particleId].velocity * deltaSeconds;
        m_particlePool[particleId].timeToLive--;
    }
    
    public void EmitParticles(int numParticles) {
        var startingIndex = m_numLiveParticles;
        for (var i = startingIndex; i < startingIndex + numParticles; i++) {
            if (i >= m_particlePool.Length) {
                return;
            }
            
            InitializeParticle(i);
            
            m_numLiveParticles++;
        }
    }
    
    public void InitializeParticle(int particleId) {
        m_particlePool[particleId].position = transform.position;
        m_particlePool[particleId].velocity = RandomizeDirection(initialDirection).normalized * initialSpeed;
        m_particlePool[particleId].radius = initialRadius;
        m_particlePool[particleId].color = initialColor;
        m_particlePool[particleId].timeToLive = initialTimeToLive;
    }

    public Vector3 RandomizeDirection(Vector3 direction) {
        var zAngle = m_random.Next(-coneAngle, coneAngle);
        direction = Quaternion.AngleAxis(zAngle, transform.forward) * direction;
        
        var yAngle = m_random.Next(0, 360);
        direction = Quaternion.AngleAxis(yAngle, transform.up) * direction;

        return direction;
    }

    public void KillParticle(int particleId) {
        var particle = m_particlePool[particleId];
        if (particle.timeToLive > 0) {
            return;
        }
        
        m_particlePool[particleId] = m_particlePool[m_numLiveParticles - 1];
        m_particlePool[m_numLiveParticles - 1] = particle;
        m_numLiveParticles--;
    }
    
    public override void DrawShapes(Camera cam){
        using (Draw.Command(cam)) {
            Draw.Matrix = transform.localToWorldMatrix;
            
            for (var i = 0; i < m_numLiveParticles; i++) {
                DrawParticle(i);
            }
        }
    }
    
    public void DrawParticle(int particleId) {
        var particle = m_particlePool[particleId];
        particle.color.a = 1;
        Draw.Sphere(particle.position, particle.radius, particle.color);
    }
}