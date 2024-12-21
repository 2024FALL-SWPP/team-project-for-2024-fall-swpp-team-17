using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to spawn shooting stars in the title, Menu scene
/// </summary>
public class SpawnShootingStars : MonoBehaviour
{
    public GameObject shootingStarPrefab;
    private float spawnInterval = 4.0f; // Time between spawns
    private static SpawnShootingStars instance;


    private void Awake()
    {
        // Ensure only one instance persists
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }


    private void Start()
    {
        // To ensure first star is at the best pos
        SpawnFirstShootingStar();
        InvokeRepeating(nameof(SpawnShootingStar), 0f, spawnInterval);
    }


    private void Update()
    {
        // Destroy this object if we leave the Title or Menu scenes
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Title" && currentScene != "Menu")
        {
            Destroy(gameObject);
        }
    }


    private void SpawnFirstShootingStar()
    {
        Vector3 firstStarPosition = new Vector3(10f, 85f, 121f);
        GameObject star = Instantiate(shootingStarPrefab, firstStarPosition, Quaternion.identity);
        star.SetActive(true);

        Rigidbody rb = star.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = star.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        float fallSpeed = 10f;
        rb.velocity = new Vector3(-fallSpeed, -fallSpeed, 0f);

        star.transform.rotation = Quaternion.Euler(10f, 10f, 15f);
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Destroy(star, 32f);
    }


    private void SpawnShootingStar()
    {
        Vector3 spawnPosition = GetRandomPosition();
        GameObject star = Instantiate(shootingStarPrefab, spawnPosition, Quaternion.identity);
        star.SetActive(true);

        Rigidbody rb = star.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = star.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;

        float fallSpeed = Random.Range(6f, 11f);
        rb.velocity = new Vector3(-fallSpeed, -fallSpeed, 0f);

        // float zRotation = Random.Range(20f, 30f);
        star.transform.rotation = Quaternion.Euler(10f, 10f, 15f);

        Destroy(star, 32f);
    }


    private Vector3 GetRandomPosition()
    {
        float x, y, z;

        if (Random.value < 0.3f)
        {
            x = Random.Range(150f, 155f);
            y = Random.Range(-10f, 70f);
        }
        else
        {
            x = Random.Range(-100f, 100f);
            y = Random.Range(90f, 95f);
        }

        z = Random.Range(119f, 121f);

        return new Vector3(x, y, z);
    }
}
