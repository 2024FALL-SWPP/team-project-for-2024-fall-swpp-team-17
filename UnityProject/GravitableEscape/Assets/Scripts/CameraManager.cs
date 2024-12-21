using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using OurGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class manages camera movement and rotation.
/// In most cases it follows the player and rotates using mouse input.
/// It has some special effects like spiraling towards a wormhole.
/// </summary>
public class CameraManager : MonoBehaviour, GravityObserver, GameStateObserver
{
    Transform player; // Target to follow, which is the player in this case
    private float distance = 15f; // Default distance from the player
    private float targetDistance = 15f; // Target distance for smooth transitions
    float height = 5f; // Height offset for the camera relative to the player
    Quaternion gravityRot, targetGravityRot; // Current and target gravity rotation
    InputManager inputManager; // Reference to the input manager for controls
    GameManager gameManager; // Reference to the game manager for state control
    Vector3 targetPosition; // Camera's target position
    GameState gameState; // Current game state
    Transform wormhole = null; // Reference to the wormhole's position
    Vector3 boundsMin, boundsMax; // Camera movement boundaries
    public float corridors_z_min, corridors_z_max; // Z-axis bounds for corridors

    private AudioSource bgmAudioSource; // Background music audio source
    private bool isbgmPlaying = true; // Tracks whether the BGM is playing

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Locate the player object
        inputManager = FindObjectOfType<InputManager>(); // Find the input manager in the scene
        gameManager = FindObjectOfType<GameManager>(); // Find the game manager in the scene
        gravityRot = Quaternion.identity; // Initialize gravity rotation
        targetGravityRot = Quaternion.identity; // Initialize target gravity rotation
        bgmAudioSource = GetComponent<AudioSource>(); // Get the audio source for BGM
        boundsMin = new Vector3(-15f, -15f, corridors_z_min); // Minimum bounds for camera
        boundsMax = new Vector3(15f, 15f, corridors_z_max); // Maximum bounds for camera
    }

    /// <summary>
    /// Updates the camera's position and behavior based on the game state.
    /// </summary>
    void LateUpdate()
    {
        switch (gameState)
        {
            case GameState.Playing:
            case GameState.Stun:
            case GameState.Revived:
                ScrollDistance(); // Adjust distance based on mouse scroll
                RotateCamera(); // Rotate camera based on input and gravity
                FollowPlayer(); // Update the target position to follow the player
                ShiftToFront(); // Ensure the camera stays within corridor bounds
                transform.position = targetPosition; // Update the camera's position
                break;
            case GameState.WormholeEffect:
                SpiralTowardsWormhole(); // Handle special effect for wormhole
                break;
            case GameState.Gameover:
                GameOverCameraMove(); // Handle camera movement during game over
                break;
        }
    }

    /// <summary>
    /// Adjusts the camera position to circle around the player during the game over state.
    /// </summary>
    void GameOverCameraMove()
    {
        float radius = 15.0f; // Radius of the circular motion
        float speed = 1.5f; // Speed of the circular motion
        float angle = Time.time * speed; // Calculate angle over time
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius; // Offset for circular motion
        gravityRot = targetGravityRot; // Align gravity rotation
        transform.position = player.position + gravityRot * offset + gravityRot * new Vector3(0, 10, 0); // Calculate position
        transform.LookAt(player, -Physics.gravity); // Orient the camera towards the player
    }

    /// <summary>
    /// Updates the distance from the player based on scroll input.
    /// </summary>
    void ScrollDistance()
    {
        if (inputManager.scrollInput != 0)
        {
            targetDistance = Mathf.Clamp(distance - inputManager.scrollInput * 10f, 5f, 25f); // Clamp the target distance
        }
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * 10f); // Smoothly transition to the target distance
    }

    /// <summary>
    /// Rotates the camera based on input and gravity.
    /// </summary>
    void RotateCamera()
    {
        gravityRot = Quaternion.Slerp(gravityRot, targetGravityRot, Time.deltaTime * 10); // Smooth gravity rotation
        transform.rotation = gravityRot * Quaternion.Euler(inputManager.pitch, inputManager.yaw, 0); // Apply input rotation
    }

    /// <summary>
    /// Updates the target position to follow the player.
    /// </summary>
    void FollowPlayer()
    {
        targetPosition = player.position - transform.forward * distance + transform.up * height; // Calculate target position
    }

    /// <summary>
    /// Ensures the camera remains within corridor bounds.
    /// </summary>
    void ShiftToFront()
    {
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, boundsMin.x, boundsMax.x), // Clamp X
            Mathf.Clamp(targetPosition.y, boundsMin.y, boundsMax.y), // Clamp Y
            Mathf.Clamp(targetPosition.z, boundsMin.z, boundsMax.z) // Clamp Z
        );
    }

    float spiralAngle = 0.0f, spiralRadius, distanceToWormhole; // Variables for spiral motion
    float spiralSpeed = 15.0f; // Speed of spiral motion
    float spiralRadiusDenom = 25.0f; // Denominator for spiral radius calculation
    float moveSpeedNum = 2.5f; // Speed of movement towards wormhole
    float minRad = 0.5f; // Minimum spiral radius
    float moveSpeed = 10.0f; // Movement speed

    /// <summary>
    /// Moves the camera in a spiral motion towards the wormhole.
    /// </summary>
    private void SpiralTowardsWormhole()
    {
        if (Vector3.Distance(transform.position, wormhole.position) > 1f)
        {
            spiralAngle += spiralSpeed * Time.deltaTime; // Update spiral angle
            distanceToWormhole = Vector3.Distance(transform.position, wormhole.position); // Calculate distance to wormhole
            spiralRadius = Mathf.Min(minRad, distanceToWormhole / spiralRadiusDenom); // Calculate spiral radius
            Vector3 spiralOffset = new Vector3(
                Mathf.Cos(spiralAngle) * spiralRadius,
                Mathf.Sin(spiralAngle) * spiralRadius,
                0); // Calculate spiral offset

            transform.position = Vector3.Lerp(transform.position, wormhole.position, moveSpeed * Time.deltaTime) + spiralOffset; // Move camera
            moveSpeed = moveSpeedNum; // Set movement speed
            transform.LookAt(wormhole); // Orient the camera towards the wormhole
        }
        else
        {
            gameManager.exitWormhole(); // Exit wormhole effect
        }
    }

    /// <summary>
    /// Sets the wormhole's position for spiral motion.
    /// </summary>
    /// <param name="wh">Transform of the wormhole</param>
    public void SetWormhole(Transform wh)
    {
        wormhole = wh; // Assign wormhole transform
    }

    public void OnNotify<GravityObserver>(Quaternion rot)
    {
        targetGravityRot = targetGravityRot * rot; // Update target gravity rotation
    }

    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs; // Update game state
    }

    /// <summary>
    /// Toggles the background music on or off.
    /// </summary>
    public void ToggleBGM()
    {
        if (isbgmPlaying)
        {
            bgmAudioSource.Pause(); // Pause the music
        }
        else
        {
            bgmAudioSource.Play(); // Play the music
        }
        isbgmPlaying = !isbgmPlaying; // Toggle play state
    }
}