using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a heavy object above the player at specified intervals when the player enters a defined zone.
/// Automatically destroys the previously spawned object before creating a new one.
/// </summary>
/// <remarks>
/// This script requires:
/// - A `boxPrefab` to spawn.
/// - A `player` transform for determining the spawn location.
/// - A `spawnPosition` to define the z-coordinate threshold for triggering the spawn logic.
/// </remarks>
public class HeavyObjectSpawner : MonoBehaviour
{
    public GameObject boxPrefab; // Prefab of the heavy object to spawn
    public Transform player; // Reference to the player transform
    public float spawnPosition; // Z-coordinate threshold for spawning

    private float lastSpawnTime = -15f; // Tracks the last time a box was spawned
    private float spawnCooldown = 15f; // Minimum time between spawns
    private GameObject spawnedBox; // Reference to the currently spawned box

    void Start()
    {
        // Initialize the player reference
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        // Check if the player is within the spawn zone and cooldown has elapsed
        if (player.position.z >= spawnPosition && player.position.z <= spawnPosition + 10 && Time.time >= lastSpawnTime + spawnCooldown)
        {
            SpawnBoxAbovePlayer();
            lastSpawnTime = Time.time;
        }

        // Destroy the spawned box after the cooldown if it exists
        if (spawnedBox != null && Time.time >= lastSpawnTime + spawnCooldown)
        {
            Destroy(spawnedBox);
        }
    }

    /// <summary>
    /// Spawns a box above the player's current position.
    /// If a box already exists, it is destroyed before creating a new one.
    /// </summary>
    void SpawnBoxAbovePlayer()
    {
        if (spawnedBox != null) Destroy(spawnedBox);

        // Calculate spawn position above and slightly in front of the player
        Vector3 spawnPosition = player.position + player.up * 15.0f + player.forward * 5.0f;

        // Instantiate the box prefab
        spawnedBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
    }
}