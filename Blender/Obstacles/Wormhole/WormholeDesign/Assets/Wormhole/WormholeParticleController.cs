using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeParticleController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float radius = 2.55f; // Radius of the donut path
    public float speed = 1f; // Speed of the particle movement

    private ParticleSystem.Particle[] particles;

    void LateUpdate()
    {
        if (particleSystem == null) return;

        // Initialize particles array if not set
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];

        int numParticlesAlive = particleSystem.GetParticles(particles);

        // Move each particle in a donut path around the sphere
        for (int i = 0; i < numParticlesAlive; i++)
        {
            float angle = Time.time * speed + i; // Offset each particle by its index
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            float y = Mathf.Sin(angle * 2) * radius * 0.5f; // Optional: gives vertical wave motion
            
            // Set particle position in a circular path around a central point
            particles[i].position = new Vector3(x, y, z);
        }

        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}
