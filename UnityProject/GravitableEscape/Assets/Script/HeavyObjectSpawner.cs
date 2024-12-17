using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyObjectSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public Transform player;
    public float spawnPosition;

    private float lastSpawnTime = -15f;
    private float spawnCooldown = 15f;
    private GameObject spawnedBox;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        // check player's z position
        if (player.position.z >= spawnPosition && player.position.z <= spawnPosition + 10 && Time.time >= lastSpawnTime + spawnCooldown)
        {
            SpawnBoxAbovePlayer();
            lastSpawnTime = Time.time;
        }
        if (spawnedBox != null && Time.time >= lastSpawnTime + spawnCooldown)
        {
            Destroy(spawnedBox);
        }
    }

    void SpawnBoxAbovePlayer()
    {
        if (spawnedBox != null) Destroy(spawnedBox);

        // above player's head
        Vector3 spawnPosition = player.position + player.up * 15.0f + player.forward * 5.0f;

        spawnedBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
    }
}