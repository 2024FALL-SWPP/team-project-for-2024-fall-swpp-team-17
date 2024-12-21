using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles transitioning to the next scene when the player collides with a specific object.
/// </summary>
/// <remarks>
/// This script requires a reference to a <see cref="LoadScene"/> component, which manages the scene loading logic.
/// </remarks>
public class LoadNextSceneManager : MonoBehaviour
{
    public LoadScene loadScene; // Reference to the scene loading handler

    /// <summary>
    /// Detects player collision and triggers the next scene to load.
    /// </summary>
    /// <param name="collision">The collision event data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("collision");
            loadScene.LoadNextScene();
        }
    }
}