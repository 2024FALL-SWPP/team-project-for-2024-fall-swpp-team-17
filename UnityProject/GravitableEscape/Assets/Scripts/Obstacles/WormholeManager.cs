using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;

/// <summary>
/// Manages the behavior of a wormhole in the game.
/// When the player collides with the wormhole, it triggers a teleportation effect to a target position.
/// </summary>
/// <remarks>
/// This class interacts with the <see cref="GameManager"/> to handle teleportation logic.
/// </remarks>
public class WormholeManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    public Vector3 targetPos; // Target position for teleportation
    private AudioSource wormholl; // Audio source for the wormhole effect

    void Start()
    {
        // Initialize references to the GameManager and AudioSource
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        wormholl = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Triggers the wormhole effect when the player collides with the wormhole.
    /// </summary>
    /// <param name="collision">The collision event data.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the wormhole effect via the GameManager
            gameManager.startWormhole(transform, targetPos);
        }
    }
}
