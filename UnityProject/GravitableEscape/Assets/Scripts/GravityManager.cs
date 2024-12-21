using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OurGame;
using UnityEngine;

/// <summary>
/// Manages gravity for the entire game by allowing the gravity direction to be rotated.
/// Gravity can be changed using number keys (1, 2, 3), and observers such as the player and camera are notified of changes via the observer pattern.
/// </summary>
public class GravityManager : MonoBehaviour, GameStateObserver
{
    public Vector3 initGravity = new Vector3(0, -35f, 0); // Initial gravity direction
    public float lastChangeTime = -100f; // Tracks the last time gravity was changed
    private GameState gameState; // Current game state
    Subject<GravityObserver, Quaternion> gravityChange; // Subject to notify gravity observers

    void Start()
    {
        // Find necessary components
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();

        // Initialize the gravity change subject and add observers
        gravityChange = new Subject<GravityObserver, Quaternion>();
        gravityChange.AddObserver(playerManager);
        gravityChange.AddObserver(cameraManager);

        // Set the initial gravity
        Physics.gravity = initGravity;
    }

    /// <summary>
    /// Checks if the game is in a state where gravity changes are allowed (Playing or Revived).
    /// Listens for key inputs to alter gravity direction.
    /// </summary>
    void Update()
    {
        switch (gameState)
        {
            case GameState.Playing:
                if (Input.GetKeyDown(KeyCode.Alpha1)) RotateAngle(-90); // Rotate gravity -90 degrees
                else if (Input.GetKeyDown(KeyCode.Alpha2)) RotateAngle(-180); // Rotate gravity -180 degrees
                else if (Input.GetKeyDown(KeyCode.Alpha3)) RotateAngle(-270); // Rotate gravity -270 degrees
                break;
            case GameState.Revived:
                if (Input.GetKeyDown(KeyCode.Alpha1)) RotateAngle(-90); // Same functionality for Revived state
                else if (Input.GetKeyDown(KeyCode.Alpha2)) RotateAngle(-180);
                else if (Input.GetKeyDown(KeyCode.Alpha3)) RotateAngle(-270);
                break;
        }
    }

    /// <summary>
    /// Rotates the gravity by the specified angle and notifies all registered observers.
    /// </summary>
    /// <param name="angle">The angle (in degrees) to rotate gravity by.</param>
    void RotateAngle(int angle)
    {
        // Ensure gravity changes are not too frequent
        if (Time.time - lastChangeTime > 0.5f)
        {
            // Rotate gravity direction
            Physics.gravity = Quaternion.Euler(0, 0, angle) * Physics.gravity;

            // Notify all gravity observers of the change
            gravityChange.NotifyObservers(Quaternion.Euler(0, 0, angle));

            // Update the last change time
            lastChangeTime = Time.time;
        }
    }

    /// <summary>
    /// Updates the current game state when notified by a subject.
    /// </summary>
    /// <param name="gs">The new game state.</param>
    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs;
    }
}