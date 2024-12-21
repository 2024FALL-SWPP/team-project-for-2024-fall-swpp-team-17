using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;
/// <summary>
/// Manages the behavior of spike hazards in the game.
/// Ensures spikes stay in a fixed position and handles player collisions to apply damage.
/// </summary>
/// <remarks>
/// This class:
/// - Keeps spikes stationary by resetting their position in `FixedUpdate`.
/// - Damages the player if they collide with the spike's pointed side.
/// - Plays a sound effect when the player is stunned due to the spike.
/// </remarks>
public class SpikeManager : HazardManager
{
    private Vector3 fixedPosition; // Fixed position to reset spike location
    private GameManager gameManager;
    private AudioSource spike;
    private float lastSoundTime = -10f; // Tracks the last time the spike sound played

    void Start()
    {
        damage = 1; // Amount of damage dealt by the spike
        fixedPosition = transform.position; // Save initial position
        gameManager = FindObjectOfType<GameManager>();
        spike = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Keep the spike at its initial position
        transform.position = fixedPosition;
    }

    /// <summary>
    /// Checks for continuous player collisions and applies damage.
    /// Plays a sound if the player is stunned by the spike.
    /// </summary>
    /// <param name="collision">The collision event data.</param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IMyCollision mycol = new CollisionWrapper(collision);

            // Check if the collision is upward (player hitting the spike's pointy side)
            if (IsCollisionUpward(mycol))
            {
                HarmPlayer(gameManager);

                // Play sound if the player is stunned and sound cooldown has passed
                if (gameManager.GetGameState() == GameState.Stun && (Time.time - lastSoundTime > 2f))
                {
                    lastSoundTime = Time.time;
                    spike.Play();
                }
            }
        }
    }

    /// <summary>
    /// Determines if the collision is on the pointed side of the spike.
    /// </summary>
    /// <param name="collision">The collision data wrapped in a custom interface.</param>
    /// <returns>True if the collision is upward, otherwise false.</returns>
    private bool IsCollisionUpward(IMyCollision collision)
    {
        MyContactPoint[] contacts = new MyContactPoint[10];
        bool isUpward = false;
        int cnt = collision.GetContacts(contacts);

        for (int i = 0; i < cnt; i++)
        {
            MyContactPoint contact = contacts[i];

            // Check if the collision normal aligns with the spike's upward direction
            if (Vector3.Dot(contact.normal, transform.up) < -0.9f)
            {
                isUpward = true;
            }
        }
        return isUpward;
    }

    /// <summary>
    /// Reduces the player's life when harmed by the spike.
    /// </summary>
    /// <param name="gm">The game manager interface for modifying player life.</param>
    protected override void HarmPlayer(ILifeManager gm)
    {
        gameManager.ModifyLife(-damage);
    }
}